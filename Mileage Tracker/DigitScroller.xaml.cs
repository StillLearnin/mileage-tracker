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

namespace MileageTracker
{
    /// <summary>
    /// Interaction logic for DigitScroller.xaml
    /// </summary>
    public partial class DigitScroller : UserControl
    {
        #region Digit
        public int Digit
        {
            get { return (int)GetValue(DigitProperty); }
            set { SetValue(DigitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Digit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DigitProperty =
            DependencyProperty.Register("Digit", typeof(int), typeof(DigitScroller), new PropertyMetadata(0));
        #endregion //Digit

        public DigitScroller()
        {
            InitializeComponent();
        }

        private void Up(object sender, RoutedEventArgs e)
        {
            Digit++;
        }

        private void Down(object sender, RoutedEventArgs e)
        {
            Digit--;
        }
    }
}
