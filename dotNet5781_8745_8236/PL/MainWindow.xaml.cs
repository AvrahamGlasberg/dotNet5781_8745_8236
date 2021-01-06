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
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenManagerOptions(object sender, RoutedEventArgs e)
        {
            Login dialog = new Login();
            if(dialog.ShowDialog() == true)
            {
                if (dialog.IsAdmin)
                {
                    ManagerPresentation window = new ManagerPresentation();
                    window.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("You don't have access to manager app.");
            }
        }
        private void OpenUserOptions(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            ExitApp();
        }
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
    }
}
