using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Числовой_квест_PLUS
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        // Метод для перемешивания массива (карты подземелья)
        static void ShuffleArray(string[] array, Random random)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int j = random.Next(i, array.Length); // Генерация случайного индекса для перемешивания
                string temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
}
