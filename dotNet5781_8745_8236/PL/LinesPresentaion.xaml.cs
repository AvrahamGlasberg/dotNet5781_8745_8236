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
            LineListBox.DataContext = Lines;
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
        private void DeleteLine(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusLine LineToDel = bt.DataContext as BO.BusLine;
            bl.DeleteBusLine(LineToDel);
            Lines.Remove(LineToDel);
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LineListBox.SelectedItem != null)// to prevent bug. event happened when item is removed from the list.
            {
                BO.BusLine LineSelected = LineListBox.SelectedItem as BO.BusLine;
                MessageBox.Show(LineSelected.LineNumber.ToString());
            }
        }
    }
}
