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
using System.Xml;


using Dishes;

namespace FoodOrder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<IDish> listOfPizza=new List<IDish>();
        private List<IDish> listOfRolls=new List<IDish>();
        private List<IDish> order= new List<IDish>();

        private int shippingCost = 60;
        private int payedSum;

        PayWindow payWindow;

        delegate void MyDelegate(int a);
        

        public MainWindow()
        {
            InitializeComponent();
            GetData();
            
        }
        /// <summary>
        /// Загрузка блюд в зависимости от их типа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DishType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            switch (((TextBlock)comboBox.SelectedItem).Text)
            {
                case "Пицца":
                    SetSecondComboBox(listOfPizza);
                    break;
                case "Роллы":
                    SetSecondComboBox(listOfRolls);
                    break;

            };

        }
        /// <summary>
        /// Загрузка информации из xml-файла
        /// </summary>
        private void GetData()
        {
            try
            {
                //Загрузка пицц, а так же работа с файлом
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("../../pizza.xml");
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                // обход всех узлов в корневом элементе
                foreach (XmlNode xnode in xRoot)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    Pizza pizza = new Pizza();
                    pizza.Name = attr.Value;
                    attr = xnode.Attributes.GetNamedItem("price");
                    try
                    {
                        pizza.Price = Int32.Parse(attr.Value);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ошибка получения данных");
                        continue;
                    }
                    listOfPizza.Add(pizza);
                }

                //Загрузка роллов
                xDoc.Load("../../rolls.xml");
                // получим корневой элемент
                xRoot = xDoc.DocumentElement;
                // обход всех узлов в корневом элементе
                foreach (XmlNode xnode in xRoot)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    Rolls rolls = new Rolls();
                    rolls.Name = attr.Value;
                    attr = xnode.Attributes.GetNamedItem("price");
                    try
                    {
                        rolls.Price = Int32.Parse(attr.Value);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ошибка получения данных");
                        continue;
                    }
                    listOfRolls.Add(rolls);
                }
                //применение LINQ to objects, а попутно сортировка по алфавиту
                listOfPizza = (from i in listOfPizza
                              orderby i.Name
                              select i).ToList();
                listOfRolls = (from i in listOfRolls
                               orderby i.Name
                               select i).ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка получения данных");
            }
        }
        /// <summary>
        /// Установка выбора блюд с указанием цены
        /// </summary>
        /// <param name="list"></param>
        private void SetSecondComboBox(List<IDish> list)
        {
            DishVariant.Items.Clear();
            foreach (IDish el in list)
            {
                string str = el.Name + " (" + el.Price + ")";
                DishVariant.Items.Add(str);
            }
            
        }
        /// <summary>
        /// Добавление выбранного блюда в заказ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddToOrder(object sender, RoutedEventArgs e)
        {
            string str;
            try 
            {
                str = ((TextBlock)DishType.SelectedItem).Text;
            
            switch (str){
                case "Пицца":
                        order.Add(listOfPizza[DishVariant.SelectedIndex]);
                        yourOrder.Items.Add("Пицца " + listOfPizza[DishVariant.SelectedIndex].ToString());
                        break;
                    case "Роллы":
                        order.Add(listOfRolls[DishVariant.SelectedIndex]);
                        yourOrder.Items.Add("Роллы " + listOfRolls[DishVariant.SelectedIndex].ToString());
                        break;
                    default:
                        break;
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите блюдо");
            }
        }

        /// <summary>
        /// Удаляет выбранный пользователем элемент из заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Item(object sender, RoutedEventArgs e)
        {
            if (yourOrder.SelectedIndex < 0)
            {
                MessageBox.Show("Ни один элемент не выбран");
                return;
            }
            order.RemoveAt(yourOrder.SelectedIndex);
            yourOrder.Items.RemoveAt(yourOrder.SelectedIndex);
        }
        /// <summary>
        /// Удаляет последний добавленный в заказ элемент
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Addition(object sender, RoutedEventArgs e)
        {
            if (yourOrder.Items.Count>0)
            {
                yourOrder.Items.RemoveAt(yourOrder.Items.Count - 1);
                order.RemoveAt(order.Count - 1);
            }
        }
        /// <summary>
        /// Расчитывает стоимость доставки. Поскольку задача учебная, то алгоритм неважен. Пусть стоимость зависит от количества символов в адресе.
        /// Также это прекрасное место, чтобы продемонстрировать использование делегатов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Calc_Delivery(object sender, RoutedEventArgs e)
        {
           
            int len = Address.Text.Length;
            MyDelegate myDelegateInstance = null;
            myDelegateInstance += (a =>
            {
                if (a < 1)
                {
                    MessageBox.Show("Мы не можем доставить по пустому адресу");
                    return;
                }
                double coef;
                coef = Math.Log10(a);
                shippingCost = (int)(50 * coef);
                shippingCostLabel.Content = (shippingCost) + " руб.";
            });
            myDelegateInstance(len);

        }

        /// <summary>
        /// Сохранение чека в выбранную пользователем дирек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Receipt(object sender, RoutedEventArgs e)
        {
            payedSum = payWindow.sum;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "receipt"; // Имя файла
            dlg.DefaultExt = ".text"; // Его расширение
            dlg.Filter = "Text documents (.txt)|*.txt"; // Отобразим только .txt файлы, чтобы упростить выбор имени для чека

            // Запустим окно выбора места сохранения
            bool? result = dlg.ShowDialog();

            // Сохраним файл
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename, false))
                {
                    //запись информации о заказе
                    int sum = 0;
                    foreach(IDish el in order)
                    {
                        file.WriteLine(el.ToString());
                        sum += el.Price;
                    }
                    file.WriteLine("Сумма заказа: " + sum + " руб.");
                    file.WriteLine("Стоимость доставки: " + shippingCost + " руб.");
                    file.WriteLine("Общая сумма: " + (sum+shippingCost) + " руб.");
                    file.WriteLine("Оплачено: " + payedSum + " руб.");
                    if (payWindow.isChangeTaken)
                        file.WriteLine("Сдача: " + (payedSum - sum - shippingCost) + " руб.");
                    else file.WriteLine("Сдачу мы оставили себе. Спасибо за чаевые");
                    file.WriteLine("Адрес доставки: " + Address.Text);
                    file.WriteLine("Дата заказа: " + DateTime.Now.ToString("D"));
                }
            }

        }
        /// <summary>
        /// Оформление заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeOrder(object sender, RoutedEventArgs e)
        {
            int sum = 0;
            int weight = 0;
            foreach (IDish el in order) {
                sum += el.Price;
                weight += el.Weight;
            }
            if (weight > 30000) {
                MessageBox.Show("Курьеру столько не донести. В стоимость включена аренда автомобиля");
                shippingCost += 200;
            }
            //вызов окна оплаты
            payWindow = new PayWindow(sum+shippingCost);
            payWindow.Show();
            
            Save_Receipt_Button.IsEnabled = true;
            ((Button)sender).IsEnabled = false;
            Add_Button.IsEnabled = false;
            Address.IsEnabled = false;
        }
    }
}
 