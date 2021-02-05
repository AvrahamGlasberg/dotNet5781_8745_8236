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
        public Login()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch(BO.MissingData ex)//creating bo failed
            {
                MessageBox.Show(ex.Message);
            }
            NameTB.Focus();
            NameTB.SelectAll();
        }
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            Register reg = new Register();
            reg.ShowDialog();
        }

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
            catch(BO.UserNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" Name {0} is not exists. choose another or register.", ex.Name));
            }
            
        }
        private void Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PassTB.Focus();
                PassTB.SelectAll();
            }
        }
        public BO.User User
        {
            get { return user; }
        }

    }
}
