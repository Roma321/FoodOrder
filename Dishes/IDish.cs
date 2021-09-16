using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishes
{
    public interface IDish
    {
        /// <summary>
        /// Устанавливает или получает название блюда
        /// </summary>
        string Name { set; get; }
        /// <summary>
        /// Получает вес блюда. Для пиццы он свой, для роллов - свой, но стандартный
        /// </summary>
        int Weight { get; }
        /// <summary>
        /// Устанавливает или получает цену блюда
        /// </summary>
        int Price { set; get; }
    }
}
