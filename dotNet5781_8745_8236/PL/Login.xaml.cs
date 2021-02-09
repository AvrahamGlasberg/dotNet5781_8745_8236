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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        IBL bl;
        BO.User user;
        /// <summary>
        /// ctor of the window 
        /// </summary>
        public Login()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch(BO.MissingData ex) //creating BO failed
            {
                MessageBox.Show(ex.Message);
            }
            NameTB.Focus();
            NameTB.SelectAll();
        }
        /// <summary>
        /// open register window
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            Register reg = new Register();
            reg.ShowDialog();
        }
        /// <summary>
        /// check the user name and the password and send the answer to the sender window
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Log_In(object sender, RoutedEventArgs e)
        {
            try
            {
                user = bl.GetUser(NameTB.Text);
                if (user.Password == PassTB.Password)
                    this.DialogResult = true;
                else
                    MessageBox.Show("Incorrect Password!");
            } 
            catch(BO.UserNotFound ex) // can't find the user in bl
            {
                MessageBox.Show(ex.Message + string.Format(" Name {0} is not exists. choose another or register.", ex.Name));
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
            {
                PassTB.Focus();
                PassTB.SelectAll();
            }
        }
        /// <summary>
        ///  get func of user
        /// </summary>
        public BO.User User
        {
            get { return user; }
        }
    }
}
