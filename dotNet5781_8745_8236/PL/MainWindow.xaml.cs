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
        /// <summary>
        /// ctor of the window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) //creating BO failed
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// open login window for check accsses to the maneger area and if it ok open the ManagerPresentation window 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void OpenManagerOptions(object sender, RoutedEventArgs e)
        {
            Login dialog = new Login(); // open the login win and geting the answer
            if(dialog.ShowDialog() == true)
            {
                if (dialog.User.Admin)
                {
                    ManagerPresentation window = new ManagerPresentation();
                    window.Show();
                    this.Close();
                }
                else // the user do not have access to the maneger app
                    MessageBox.Show("You don't have access to manager app.", "Access denied", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        /// <summary>
        /// open the simulation window
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void OpenSimulator(object sender, RoutedEventArgs e)
        {
            Simulator window = new Simulator();
            window.Show(); 
            this.Close();
        }
        /// <summary>
        /// close the window
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            ExitApp();
        }
        /// <summary>
        /// close all the app
        /// </summary>
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// make animate on the Button  when the mouse enter
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Button_Mouse_Enter(object sender, MouseEventArgs e)
        {
            (sender as Button).Increase(7);
        }
        /// <summary>
        ///  Decrease the animate on the Button when the mouse leave
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Button_Mouse_Leave(object sender, MouseEventArgs e)
        {
            (sender as Button).Decrease();
        }
        /// <summary>
        /// Open  the window of user info
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
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
