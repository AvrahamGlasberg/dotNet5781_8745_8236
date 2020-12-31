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
using System.Collections.ObjectModel;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for LinesPresentaion.xaml
    /// </summary>
    public partial class LinesPresentaion : Window
    {
        IBL bl = BLFactory.GetBL();
        private ObservableCollection<BO.BusLine> Lines;
        public LinesPresentaion()
        {
            InitializeComponent();
            Lines = new ObservableCollection<BO.BusLine>(bl.GetAllBusLines());
            Start();
        }
        private void Start()
        {
            DataContext = Lines;
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            //Lines.Remove(Lines.First<BO.BusLine>());
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
