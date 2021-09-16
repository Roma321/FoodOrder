using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishes
{

    
    public class Rolls : IDish
    {
        public static void Main(string[] args)
        {
            //какой-то код
        }
        private string name;
        private int price;
        //аксессоры использованы

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }

        }
        public int Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }

        }

        //лямбда-выражение использовано
        public int Weight { get => 320; }

        /// <summary>
        /// Выводит краткую инфоомацию о блюде
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            return Name + " Цена: " + Price + " руб. "+ "Вес: " + Weight;
        }
    }
}
