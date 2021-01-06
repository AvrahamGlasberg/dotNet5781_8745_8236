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
        public Register()
        {
            InitializeComponent();
            bl = BLFactory.GetBL();
            NameTB.Focus();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddUser(new BO.User()
                {
                    UserName = NameTB.Text,
                    Password = PassTB.Text,
                    Admin = IsAdmin.IsChecked == true
                });
                this.DialogResult = true;
            }
            catch
            {
                MessageBox.Show("This user already exists.");
            }
        }
        private void Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PassTB.Focus();
        }

    }
}
