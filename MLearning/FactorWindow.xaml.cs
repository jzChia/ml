using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Common.Entities;
using Common.DAO;
using Common;

namespace MLearning
{
    /// <summary>
    /// FactorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FactorWindow : Window
    {
        public FactorWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mlContext ml;
            List<Factor> factors = FactorDB.GetFactors();

            datag.ItemsSource = factors;
            
        }
    }
}
