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
using BLAPI;
using BO;
namespace PL.Dialogs
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        IBL bl;
        /// <summary>
        /// window ctor
        /// </summary>
        public Register()
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
            NameTB.Focus();
        }
        /// <summary>
        /// add user to the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddUser(new BO.User()
                {
                    UserName = NameTB.Text,
                    Password = PassTB.Text,
                    Admin = IsAdmin.IsChecked == true, 
                    Cash = 0
                });
                this.DialogResult = true;
            }
            catch(BO.UserExists ex) // user already exists
            {
                MessageBox.Show(ex.Message + string.Format(" Name {0} already exists as a member. please choose another or log-in.", ex.Name), "Object already exists", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// move to the next textbBlock when press Enter key
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PassTB.Focus();
        }

    }
}
