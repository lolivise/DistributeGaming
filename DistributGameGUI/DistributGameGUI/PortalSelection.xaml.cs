using DistributGameBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DistributGameGUI
{
    /// <summary>
    /// Interaction logic for PortalSelection.xaml
    /// </summary>
    /// 

    public partial class PortalSelection : IDGBizControllerCallback
    {
        private int userID;
        private String username;
        private List<String> userState;
        private List<int> friendIDList;
        private Dictionary<String, String> serverList;

        private IDGBizController m_DGBiz;


        //initial page
        public PortalSelection(String username, int userID)
        {
            InitializeComponent();
            this.userID = userID;
            this.username = username;

            userState = new List<String>();
            friendIDList = new List<int>();
        }

        //load window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DuplexChannelFactory<IDGBizController> DGBiz;

            NetTcpBinding tcpBinding = new NetTcpBinding();

            //release the limitation of the size of message which can be sent
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            string sURL = "net.tcp://localhost:50002/DGBiz";
            try
            {
                //connect to Biz Tier
                DGBiz = new DuplexChannelFactory<IDGBizController>(this, tcpBinding, sURL);
                m_DGBiz = DGBiz.CreateChannel();

            }
            catch (CommunicationObjectFaultedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            //Setup page
            welcome.Content = "welcome " + username;
            try
            {
                m_DGBiz.RegisterClient(username);
                serverList = m_DGBiz.GetServerList();
                friendIDList = m_DGBiz.GetFriendID(userID);
                m_DGBiz.ChangeUserState(userID, "online");
                m_DGBiz.NotifyServer();
                m_DGBiz.NotifyServerState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //select a server to enter
        private void btn_Server_Click(object sender, RoutedEventArgs e)
        {
            int index = lvServerState.SelectedIndex;
            try
            {
                //if server selected is available
                if (index != -1 && m_DGBiz.CheckServerAvailaility(index))
                {
                    String serverName = serverList.Keys.ElementAt(index);
                    String server_URL = serverList.Values.ElementAt(index);
                    
                    //if the player in the server is less than 12
                    if (m_DGBiz.GetClientInServer(serverName) < 12)
                    {
                        m_DGBiz.AddClientInServer(serverName);
                        m_DGBiz.ChangeUserState(userID, serverName);
                        m_DGBiz.NotifyServer();
                        enterCharaterSelection(index, server_URL);
                    }
                    else
                    {
                        MessageBox.Show(serverName + " is crowded!");
                    }
                }
                else
                {
                    MessageBox.Show("The Current Selection is not Available!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        //View the detail of selected server
        private void btn_Server_View_Click(object sender, RoutedEventArgs e)
        {
            int index = lvServerState.SelectedIndex;
            try
            {
                if (index != -1)
                {
                    String serverName = serverList.Keys.ElementAt(index);
                    List<String> bossInfo = m_DGBiz.GetBossInfoList();

                    ServerInfo serverInfo = new ServerInfo();
                    serverInfo = m_DGBiz.GetServerInfo();
                    MemberInServer memberInServer = serverInfo.serverInfoList[index];

                    ViewServerInfo viewServerInfo = new ViewServerInfo(serverName, memberInServer, bossInfo[index]);
                    viewServerInfo.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        public void BroadcastToClient(FriendState friendState)
        {
            lvFriendState.ItemsSource = friendState.FriendList;
        }

        public void BroadcastServerState(ServerState serverState)
        {
            lvServerState.ItemsSource = serverState.ServerStateList;
        }

        private void enterCharaterSelection(int index, String selectedServerURL)
        {

            if (index == 0)
            {
                CharacterSelection W = new CharacterSelection(username, userID, selectedServerURL);
                this.Close();
                W.Show();
            }
            else
            {
                CharacterSelection2 W = new CharacterSelection2(username, userID, selectedServerURL);
                this.Close();
                W.Show();
            }
            
        }

    }
}
