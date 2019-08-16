using DistributGameBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DistributGameGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDGBizControllerCallback
    {
        private IDGBizController m_DGBiz;

        //initial window
        public MainWindow()
        {
            InitializeComponent();
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

        }

        //Function of login
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            String username = userName.Text;
            String password = passWord.Text;
            try
            {
                //verify the username and password
                int userID = m_DGBiz.CheckAuthority(username, password);
                //if correct
                if (userID != -1)
                {
                    //if the user havs not login yet
                    if (!m_DGBiz.CheckDuplicateLogin(username))
                    {
                        enterPortalSelection(username, userID);
                    }
                    else
                    {
                        MessageBox.Show("The user has Already Login!");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Correct Username and Password!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
      
        }

        //enter to Portal Interface
        private void enterPortalSelection(String username, int userID)
        {
            PortalSelection W = new PortalSelection(username, userID);
            this.Close();
            W.Show();
        }

        //remote callback function
        public void BroadcastToClient(FriendState friendState)
        {
            throw new NotImplementedException();
        }

        public void BroadcastServerState(ServerState serverState)
        {
            throw new NotImplementedException();
        }
    }
}
