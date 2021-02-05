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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PL.Dialogs;
using BLAPI;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) //creating bo failed
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void OpenManagerOptions(object sender, RoutedEventArgs e)
        {
            Login dialog = new Login();
            if(dialog.ShowDialog() == true)
            {
                if (dialog.User.Admin)
                {
                    ManagerPresentation window = new ManagerPresentation();
                    window.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("You don't have access to manager app.", "Access denied", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void OpenSimulator(object sender, RoutedEventArgs e)
        {
            Simulator window = new Simulator();
            window.Show();
            this.Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            ExitApp();
        }
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }

        private void Button_Mouse_Enter(object sender, MouseEventArgs e)
        {
            (sender as Button).Increase(7);
        }
        private void Button_Mouse_Leave(object sender, MouseEventArgs e)
        {
            (sender as Button).Decrease();
        }

        private void WindowActivated(object sender, EventArgs e)
        {
        }

        private void OpenUserInfo(object sender, RoutedEventArgs e)
        {
            Login win = new Login();
            if (win.ShowDialog() == true)
            {
                UserInfo newWin = new UserInfo(win.User);
                newWin.Show();
                this.Close();
            }
        }
    }
}
