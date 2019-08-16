using DistributGameServer2;
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
using System.Windows.Shapes;

namespace DistributGameGUI
{
    /// <summary>
    /// Interaction logic for CharacterSelection.xaml
    /// a interface for user to select a hero to join combat
    /// </summary>
    public partial class CharacterSelection2 : IDGSerControllerCallback
    {
        private String server_URL;
        private String username;
        private int userID;
        private int selectedHeroID;
        private IDGSerController m_DGSer;
        private List<String> heroInfoList;
        private List<String> heroNameList;
        private String selectedBossInfo;
        private List<Button> buttons;
        private List<Label> labels;
        private Boolean pressGO;

        public CharacterSelection2(String username, int userID, String server_URL)
        {
            InitializeComponent();
            this.server_URL = server_URL;
            this.username = username;
            this.userID = userID;
            setupList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DuplexChannelFactory<IDGSerController> DGSer;

            NetTcpBinding tcpBinding = new NetTcpBinding();

            //release the limitation of the size of message which can be sent
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            try
            {
                //connect to Server Tier
                DGSer = new DuplexChannelFactory<IDGSerController>(this, tcpBinding, server_URL);
                m_DGSer = DGSer.CreateChannel();

            }
            catch (CommunicationObjectFaultedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            //set up page information
            user.Content = "Welcome " + username + ", \nplease select a hreo and press \"GO\" to enter the battle.";
            pressGO = false;

            try
            {
                m_DGSer.RegisterClient(username);
                heroInfoList = m_DGSer.GetHeroInfoList();
                heroNameList = m_DGSer.GetHeroNameList();
                selectedBossInfo = m_DGSer.GetSelectedBossInfo();
                setupInfo();
                m_DGSer.NotifyServer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        //wrap buttons and labels in list
        private void setupList()
        {
            buttons = new List<Button>();
            buttons.Add(btn_char1);
            buttons.Add(btn_char2);
            buttons.Add(btn_char3);
            buttons.Add(btn_char4);
            buttons.Add(btn_char5);
            buttons.Add(btn_char6);
            labels = new List<Label>();
            labels.Add(hero1_select_time);
            labels.Add(hero2_select_time);
            labels.Add(hero3_select_time);
            labels.Add(hero4_select_time);
            labels.Add(hero5_select_time);
            labels.Add(hero6_select_time);
        }

        //print name of the hero on the button
        private void setupInfo()
        {
            for (int i = 0; i < heroNameList.Count; i++)
            {
                buttons[i].Content = heroNameList[i];
            }

            bossDetail.Text = selectedBossInfo;
        }
        //select the first hero
        private void btn_char1_Click(object sender, RoutedEventArgs e)
        {
            if (!pressGO)
            {
                hero_info.Text = heroInfoList[0];
                selectedHeroID = 0;
            }
        }

        //select the second hero
        private void btn_char2_Click(object sender, RoutedEventArgs e)
        {
            if (!pressGO)
            {
                hero_info.Text = heroInfoList[1];
                selectedHeroID = 1;
            }
        }

        //select the third hero
        private void btn_char3_Click(object sender, RoutedEventArgs e)
        {
            if (!pressGO)
            {
                hero_info.Text = heroInfoList[2];
                selectedHeroID = 2;
            }
        }

        //select the fourth hero
        private void btn_char4_Click(object sender, RoutedEventArgs e)
        {
            if (!pressGO)
            {
                hero_info.Text = heroInfoList[3];
                selectedHeroID = 3;
            }
        }

        //select the fifth hero
        private void btn_char5_Click(object sender, RoutedEventArgs e)
        {
            if (!pressGO)
            {
                hero_info.Text = heroInfoList[4];
                selectedHeroID = 4;
            }
        }

        //select the sixth hero
        private void btn_char6_Click(object sender, RoutedEventArgs e)
        {
            if (!pressGO)
            {
                hero_info.Text = heroInfoList[5];
                selectedHeroID = 5;
            }
        }

        //decide hero
        private void btn_Go_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!pressGO)
                {
                    pressGO = true;
                    m_DGSer.AddNumberOfPlayer();
                    m_DGSer.AddHeroSelectTimes(selectedHeroID);
                    m_DGSer.NotifyServer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //enter the combat
        private void enterCombatInterface()
        {

            pressGO = false;
            CombatInterface2 W = new CombatInterface2(username, userID, selectedHeroID, server_URL);
            this.Close();
            W.Show();
        }

        //get the player number and hero list
        public void BroadcastToClient(int numOfPlayer, List<int> heroSelectTimes)
        {

            for (int i = 0; i < heroSelectTimes.Count; i++)
            {
                labels[i].Content = heroSelectTimes[i];
            }

            if (pressGO && numOfPlayer >= 5)
            {
                enterCombatInterface();
            }
        }

        //reomte callback
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
