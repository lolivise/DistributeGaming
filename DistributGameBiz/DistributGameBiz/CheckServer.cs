using DistributGameServer1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributGameBiz
{
    public class CheckServer : IDGSerControllerCallback
    {

        private IDGSerController m_DGSer;
        private Dictionary<String, String> playerSelectHero;
        private Dictionary<String, String> playerHeroInfo;
        private ServerState serverState;
        private ServerInfo serverInfo;
        private Server server;
        private MemberInServer memberInServer;
        private Member member;
        private int serverCount;
        private List<String> bossInServer;
        /**
         * Check the connection of every servers in Server Tier
         * **/
        public CheckServer(Dictionary<String, String> serverList)
        {
            playerSelectHero = new Dictionary<String, String>();
            playerHeroInfo = new Dictionary<String, String>();
            serverState = new ServerState();
            serverInfo = new ServerInfo();
            bossInServer = new List<String>();

            serverCount = 0;
            //run each server
            foreach (var server in serverList)
            {
                DuplexChannelFactory<IDGSerController> DGSer;
                NetTcpBinding tcpBinding = new NetTcpBinding();
                try
                {
                    //connect server
                    DGSer = new DuplexChannelFactory<IDGSerController>(this, tcpBinding, server.Value);
                    m_DGSer = DGSer.CreateChannel();
                    playerSelectHero = m_DGSer.GetPlayersHeroName();
                    playerHeroInfo = m_DGSer.GetPlayersHeroHPInfo();

                    bossInServer.Add(m_DGSer.GetSelectedBossInfo());

                    setServerState(server.Key, true);
                    setServerInfo(server.Key);
                    serverInfo.serverInfoList.Insert(serverCount, memberInServer);
                    serverCount++;
                }
                catch (Exception)//if fail to connect
                {
                    bossInServer.Add("");
                    setServerState(server.Key, false);
                    memberInServer = new MemberInServer();
                    serverInfo.serverInfoList.Insert(serverCount, memberInServer);
                    serverCount++;
                }

            }
        }


        // get detail of players' HP information in the server
        private void setServerInfo(String serverName)
        {
            memberInServer = new MemberInServer();

            int index = 0;
            foreach (var player in playerSelectHero)
            {
                String name = player.Key;
                member = new Member(name, playerSelectHero[name], playerHeroInfo[name]);
                memberInServer.memberList.Insert(index, member);
                index++;
            }
        }

        //Set current server status
        private void setServerState(String ServerName, Boolean isAccessable)
        {
            if (isAccessable)
            {
                server = new Server(ServerName, "Open", playerSelectHero.Count);
                serverState.ServerStateList.Insert(serverCount, server);
                
            }
            else
            {
                server = new Server(ServerName, "Close", 0);
                serverState.ServerStateList.Insert(serverCount, server);
            }


        }

        //functions for getting value
        public ServerInfo getServerInfo()
        {
            return serverInfo;
        }

        public ServerState getServerState()
        {
            return serverState;
        }

        public List<String> getBossInServer()
        {
            return bossInServer;
        }

        //remote calback functions from server
        public void BroadcastToClient(int numOfPlayer, List<int> heroSelectTimes)
        {
            throw new NotImplementedException();
        }

        public void BroadcastToPlayer(Allies allies, int selectedHeroHP, int bossCurrentHP, string lastTarget, string combatResult)
        {
            throw new NotImplementedException();
        }

        public void BroadcastDecisionTime(int decisionTime, Allies allies)
        {
            throw new NotImplementedException();
        }
    }
}
