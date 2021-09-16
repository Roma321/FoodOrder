using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishes
{
    public class Pizza : IDish
    {
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
        public int Weight { get => 700; }

        override public string ToString()
        {
            return Name + " Цена: " + Price + " руб. "+ "Вес: " + Weight;
        }
    }
}
