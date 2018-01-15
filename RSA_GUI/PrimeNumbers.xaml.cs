using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
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

namespace RSA_GUI
{
    /// <summary>
    /// Interaction logic for PrimeNumbers.xaml
    /// </summary>
    public partial class PrimeNumbers : Window
    {
        public PrimeNumbers()
        {
            InitializeComponent();

            ObservableCollection<int> olist = new ObservableCollection<int>(MainWindow.rsa.PrimeNumbersDict.Values);
            ListBoxPrimes.DataContext = olist;

            Binding binding = new Binding();
            ListBoxPrimes.SetBinding(ListBox.ItemsSourceProperty, binding);
        }
    }
}
