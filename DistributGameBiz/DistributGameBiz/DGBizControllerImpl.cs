using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DistributGameData;

namespace DistributGameBiz
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DGBizControllerImpl: IDGBizController
    {
        private Dictionary<String, IDGBizControllerCallback> clients;
        private Dictionary<String, List<String>> userFriendList;
        private Dictionary<String, String> usersState;
        private Dictionary<String, String> lastEnterServer;
        private Dictionary<String, String> serverList;
        private Dictionary<String, int> numberOfClientInServer;
        private object locker;

        IDGDataController m_DGData;
        CheckServer checkServer;
        UserInfo userInfo;
        FriendState friendState;
        Friend friend;
        ServerInfo serverInfo;
        //MemberInServer memberInServer;
        ServerState serverState;


        public DGBizControllerImpl()
        {
            //Biz server connects to server
            ChannelFactory<IDGDataController> DGData;
            NetTcpBinding tcpBinding = new NetTcpBinding();

            //release the limitation of the size of message which can be sent
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            try
            {
                DGData = new ChannelFactory<IDGDataController>(tcpBinding, "net.tcp://localhost:50001/DGData");
                m_DGData = DGData.CreateChannel();
                userInfo = m_DGData.GetUserInfo();
                
            }
            catch (CommunicationObjectFaultedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("From DGBizControllerImpl.cs > DGBizControllerImpl\n" + e.Message);
            }

            inituserState();
            clients = new Dictionary<String, IDGBizControllerCallback>();
            lastEnterServer = new Dictionary<String, String>();
            numberOfClientInServer = new Dictionary<string, int>();
            serverList = new Dictionary<string, string>();
            numberOfClientInServer.Add("Server1", 0);
            numberOfClientInServer.Add("Server2", 0);
            numberOfClientInServer.Add("Server3", 0);
            serverList.Add("Server1", "net.tcp://localhost:50003/DGServer1");
            serverList.Add("Server2", "net.tcp://localhost:50004/DGServer2");
            locker = new object();

            startCount();
            

            Console.WriteLine("Biz Server Started");
        }

        ~DGBizControllerImpl()
        {
            Console.WriteLine("A client is no longer serviced!");
        }

        //initial users status
        private void inituserState()
        {

            usersState = new Dictionary<String, String>();
            userFriendList = new Dictionary<String, List<String>>();
            for (int i = 0; i < userInfo.Users.Count; i++)
            {
                usersState.Add(userInfo.Users[i].userName,"offline");
                List<String> friends = new List<String>();
                foreach (int friendID in userInfo.Users[i].friendList)
                {
                    friends.Add(userInfo.Users[friendID].userName);
                }
                userFriendList.Add(userInfo.Users[i].userName, friends);
            }
            
        }

        //delegate function for checking server status
        private delegate void CheckServerDel();

        private void startCount()
        {
            CheckServerDel checkServerDel;
            checkServerDel = this.checking;
            //fork a thread to continusly check server status
            checkServerDel.BeginInvoke(null,null);

        }

        //check servers' status
        private void checking()
        {
            while (true)
            {
                checkServer = new CheckServer(serverList);
                serverInfo = checkServer.getServerInfo();
                serverState = checkServer.getServerState();
                NotifyServer();
                NotifyServerState();
                Thread.Sleep(2000);
            }
            
        }

        //return server name and URL
        public Dictionary<String, String> GetServerList()
        {
            return serverList;
        }

        //return Server inforamtion
        public ServerInfo GetServerInfo()
        {
            return checkServer.getServerInfo();
        }

        //return boss detail
        public List<String> GetBossInfoList()
        {
            return checkServer.getBossInServer();
        }

        //verify user
        public int CheckAuthority(String username, String password)
        {
            int id = 0;
            foreach (var user in userInfo.Users)
            {
                if (user.userName == username && user.password == password)
                {
                    return id;
                }
                id++;
            }
            return -1;
        }

        //check whether a user has already login
        public Boolean CheckDuplicateLogin(String username)
        {
            if (clients.Keys.Contains(username))
            {
                return true;
            }
            return false;
        }

        //register connected client
        public void RegisterClient(String username)
        {
            if (username != null && username != "")
            {
                try
                {
                    //create a remote callback object
                    IDGBizControllerCallback callback = OperationContext.Current.GetCallbackChannel<IDGBizControllerCallback>();
                    lock (locker)
                    {
                        //remove the old client
                        if (clients.Keys.Contains(username))
                        {
                            clients.Remove(username);
                            MinusClientInServer(lastEnterServer[username]);
                            lastEnterServer.Remove(username);
                        }
                        clients.Add(username, callback);
                        lastEnterServer.Add(username, "");
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        //broadcast the status of users' friends to users
        public void NotifyServer()
        {
            lock (locker)
            {
                var inactiveClients = new List<String>();
                foreach (var client in clients)
                {
                    setFriendState(client.Key);
                    try
                    {
                       client.Value.BroadcastToClient(friendState);
                    }
                    catch (Exception)
                    {
                        inactiveClients.Add(client.Key);
                    }
                    
                }
                //remove the client who is already disconnected
                if (inactiveClients.Count > 0)
                {
                    foreach (var client in inactiveClients)
                    {
                        clients.Remove(client);
                        MinusClientInServer(lastEnterServer[client]);
                        lastEnterServer.Remove(client);
                        usersState[client] = "offline";
                    }
                }
            }
        }

        //broadcast the status of all the server
        public void NotifyServerState()
        {
            lock (locker)
            {
                var inactiveClients = new List<String>();
                foreach (var client in clients)
                {
                    setFriendState(client.Key);
                    try
                    {
                        client.Value.BroadcastServerState(serverState);
                    }
                    catch (Exception)
                    {
                        inactiveClients.Add(client.Key);
                    }

                }

                //remove the client who is already disconnected
                if (inactiveClients.Count > 0)
                {
                    foreach (var client in inactiveClients)
                    {
                        clients.Remove(client);
                        MinusClientInServer(lastEnterServer[client]);
                        lastEnterServer.Remove(client);
                    }
                }
            }
        }

        //get the id of users' friends
        public List<int> GetFriendID(int userID)
        {
            List<int> friendIDList = new List<int>();

            friendIDList = userInfo.Users[userID].friendList;

            return friendIDList;
        }

        //set friend status of users
        private void setFriendState(String username)
        {
            friendState = new FriendState();
            int i = 0;
            foreach (String friendName in userFriendList[username])
            {
                friend = new Friend(friendName,usersState[friendName]);
                friendState.FriendList.Insert(i, friend);
            }
        }

        //chage the status of user
        public void ChangeUserState(int userID, String state)
        {
            lastEnterServer[userInfo.Users[userID].userName] = state;
            usersState[userInfo.Users[userID].userName] = state;
        }

        //increase the number of client in certain server
        public void AddClientInServer(String serverX)
        {
            if (numberOfClientInServer.Keys.Contains(serverX))
            {
                numberOfClientInServer[serverX]++;
            }
        }

        //decrease the number of client in certain server
        private void MinusClientInServer(String serverX)
        {
            if (numberOfClientInServer.Keys.Contains(serverX))
            {
                numberOfClientInServer[serverX]--;
            }
        }

        //get the number of client in certain server
        public int GetClientInServer(String serverX)
        {
            int returnValue = 0;
            if (numberOfClientInServer.Keys.Contains(serverX))
            {
                returnValue = numberOfClientInServer[serverX];
            }
            return returnValue;
        }

        //check whether the server is currently available
        public Boolean CheckServerAvailaility(int serverIndex)
        {
            if (serverState.ServerStateList[serverIndex].state == "Open")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
