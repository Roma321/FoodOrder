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

namespace FoodOrder
{
    /// <summary>
    /// Логика взаимодействия для PayWindow.xaml (для окна оплаты)
    /// </summary>
    public partial class PayWindow : Window
    {
        public int sum;
        public bool isChangeTaken;
        
        public PayWindow(int sum)
        {
            InitializeComponent();
            this.sum = sum;
            PaySum.Content += (" " + sum + " руб.");
        }

        private void Pay(object sender, RoutedEventArgs e)
        {
            try
            {
                int change = Int32.Parse(PayedSum.Text) - sum;
                if (change < 0) MessageBox.Show("Этого мало. Внесите другую сумму");
                else
                {
                    ((Button)sender).IsEnabled = false;
                    takeChangeButton.IsEnabled = true;
                    justGoButton.IsEnabled = true;
                    changeLabel.Content = "Ваша сдача: " + change + " руб.";
                    sum=Int32.Parse(PayedSum.Text);
                    

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Извольте оплатить числами");
            }
        }

        private void Take_Change(object sender, RoutedEventArgs e)
        {
            isChangeTaken = true;
            this.Close();
        }

        private void Just_Go(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Спасибо!");
            isChangeTaken = false;
            this.Close();
        }
    }
}
