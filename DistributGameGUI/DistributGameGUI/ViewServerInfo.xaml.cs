using DistributGameBiz;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for ViewServerInfo.xaml
    /// View the Detail of the server
    /// </summary>
    
    public partial class ViewServerInfo : Window
    {
        MemberInServer memberInServer;
        String bossInfo;
        String serverName;

        //initial the server value
        public ViewServerInfo(String serverName, MemberInServer members, String bossInfo)
        {
            InitializeComponent();
            memberInServer = members;
            this.bossInfo = bossInfo;
            this.serverName = serverName;
        }

        //close the window
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //load the window
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            server_Name.Content = "";
            lvPlayerInfo.ItemsSource = memberInServer.memberList;
            boss_Info.Text = bossInfo;
        }

        //load grid
        private void Grid_Loaded(object sender, RoutedEventArgs e) { }
    }
}
