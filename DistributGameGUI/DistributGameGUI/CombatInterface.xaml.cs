using DistributGameServer1;
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
    /// Interaction logic for CombatInterface.xaml
    /// For displaying the combat
    /// </summary>
    public partial class CombatInterface : IDGSerControllerCallback
    {
        private IDGSerController m_DGSer;
        List<String> alliesNameList;
        String username;
        String server_URL;
        int userID;
        int selectedHeroID;
        int bossMaxHP;
        Boolean isEnd;

        //initial page
        public CombatInterface(String username, int userID, int selectedHeroID, String server_URL)
        {
            InitializeComponent();

            this.username = username;
            this.userID = userID;
            this.selectedHeroID = selectedHeroID;
            this.server_URL = server_URL;
            alliesNameList = new List<String>();
        }

        //load window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DuplexChannelFactory<IDGSerController> DGSer;

            NetTcpBinding tcpBinding = new NetTcpBinding();

            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            try
            {
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

            //set up the information
            try
            {
                m_DGSer.RegisterPlayer(username, selectedHeroID);
                int heroHP = 0;
                while (heroHP == 0)
                {
                    heroHP = m_DGSer.GetSelectedHeroHP(username);
                }
                currentHP.Content = heroHP;
                selfState.Content = "/" + heroHP;
                hero_type.Content = username + "(" + m_DGSer.GetHeroName(selectedHeroID) + ")";
                boss_name.Content = m_DGSer.GetBossName();
                bossMaxHP = m_DGSer.GetBossMaxHP();
                boss_hp.Content = "HP: " + bossMaxHP;
                lvAbilitySelection.ItemsSource = m_DGSer.GetAbilityInfo(selectedHeroID).abilitesList;
                m_DGSer.NotifyAllPlayer();
            }
            catch (TimeoutException et)
            {
                MessageBox.Show(et.Message);
                m_DGSer.RegisterPlayer(username, selectedHeroID);
                int heroHP = 0;
                while (heroHP == 0)
                {
                    heroHP = m_DGSer.GetSelectedHeroHP(username);
                }
                currentHP.Content = heroHP;
                selfState.Content = "/" + heroHP;
                hero_type.Content = username + "(" + m_DGSer.GetHeroName(selectedHeroID) + ")";
                boss_name.Content = m_DGSer.GetBossName();
                bossMaxHP = m_DGSer.GetBossMaxHP();
                boss_hp.Content = "HP: " + bossMaxHP;
                lvAbilitySelection.ItemsSource = m_DGSer.GetAbilityInfo(selectedHeroID).abilitesList;
                m_DGSer.NotifyAllPlayer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //eneter Portal interface
        private void enterSelectServerPage()
        {
            PortalSelection W = new PortalSelection(username, userID);
            this.Close();
            W.Show();
        }

        //decide the ability
        private void btn_decision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isEnd)
                {
                    m_DGSer.SetPlayerAbility(username, lvAbilitySelection.SelectedIndex, lvAllies.SelectedIndex);
                    ability_for_next_round.Content = m_DGSer.ShowSelection(selectedHeroID, lvAbilitySelection.SelectedIndex, lvAllies.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void BroadcastToClient(int numOfPlayer, List<int> heroSelectTimes)
        {
            throw new NotImplementedException();
        }

        //update the combat result
        public void BroadcastToPlayer(Allies allies, int selectedHeroHP, int bossCurrentHP, string lastTarget, string combatResult)
        {
            boss_hp.Content = bossCurrentHP + "/" + bossMaxHP;
            boss_last_target.Content = "Last Target: " + lastTarget;
            lvAllies.ItemsSource = allies.alliesList;
            if (selectedHeroHP != 0)
            {
                currentHP.Content = selectedHeroHP;
                ability_for_next_round.Content = "Select a ability for next round.";
            }
            else
            {
                isEnd = true;
                currentHP.Content = 0;
                ability_for_next_round.Content = "Unable to do anything.";
            }

            if (combatResult != "")
            {
                isEnd = true;
                MessageBox.Show("You " + combatResult + " !!!");
                enterSelectServerPage();
            }
        }
        
        //secondly update the remain decision time and allies' detail
        public void BroadcastDecisionTime(int decisionTime, Allies allies)
        {
            Boolean isChange = false;
            decision_Time.Content = "" + decisionTime;

            //check whether allies' detail has changed
            if (allies.alliesList.Count == alliesNameList.Count)
            {
                for (int i = 0; i < alliesNameList.Count;i++)
                {
                    if (alliesNameList[i] != allies.alliesList[i].name)
                    {
                        isChange = true;
                    }
                }
            }
            else
            {
                isChange = true;
            }
            //if it is change update the allies' detail
            if (isChange)
            {
                alliesNameList = new List<String>();
                for (int j = 0; j < allies.alliesList.Count; j++)
                {
                    alliesNameList.Add(allies.alliesList[j].name);
                }
                lvAllies.ItemsSource = allies.alliesList;
            }
        }

    }
}
