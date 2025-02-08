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

        // Метод для боя с монстром
        static void FightMonster(Random random, ref int health, ref int arrows, ref int experience)
        {
            int monsterHealth = random.Next(20, 51); // Здоровье монстра в диапазоне от 20 до 50
            Console.WriteLine($"Вы встретили монстра с {monsterHealth} HP.");

            while (monsterHealth > 0 && health > 0) // Бой продолжается, пока живы и игрок, и монстр
            {
                Console.WriteLine("Выберите оружие: 1 - Меч, 2 - Лук");
                string choice = Console.ReadLine();

                int damage = 0; // Урон, который игрок наносит монстру
                if (choice == "1") // Если выбрано оружие "меч"
                {
                    damage = random.Next(10, 21); // Урон меча от 10 до 20
                    Console.WriteLine($"Вы нанесли монстру {damage} урона.");
                }
                else if (choice == "2" && arrows > 0) // Если выбрано оружие "лук" и есть стрелы
                {
                    damage = random.Next(5, 16); // Урон лука от 5 до 15
                    arrows--; // Уменьшаем количество стрел на 1
                    Console.WriteLine($"Вы нанесли монстру {damage} урона. Осталось стрел: {arrows}");
                }
                else if (choice == "2" && arrows <= 0) // Если выбрано оружие "лук", но стрел нет
                {
                    Console.WriteLine("У вас нет стрел для лука!");
                    continue; // Пропускаем итерацию цикла и продолжаем бой
                }

                monsterHealth -= damage; // Уменьшаем здоровье монстра на полученный урон
                if (monsterHealth > 0) // Если монстр все еще жив
                {
                    int monsterDamage = random.Next(5, 16); // Урон от монстра от 5 до 15
                    health -= monsterDamage; // Уменьшаем здоровье игрока на урон монстра
                    Console.WriteLine($"Монстр нанес вам {monsterDamage} урона. Осталось здоровья: {health}");
                }
            }

            if (health > 0) // Если игрок победил монстра
            {
                Console.WriteLine("Вы победили монстра!");
                experience += 10; // Награда за победу - получение опыта
            }
        }

        // Метод для активации ловушки
        static void TriggerTrap(Random random, ref int health)
        {
            int trapDamage = random.Next(10, 21); // Урон от ловушки от 10 до 20
            health -= trapDamage; // Уменьшаем здоровье игрока на урон ловушки
            Console.WriteLine($"Вы попали в ловушку и потеряли {trapDamage} HP. Осталось здоровья: {health}");
        }

        // Метод для открытия сундука с загадкой
        static void OpenChest(Random random, ref string[] inventory, ref int gold)
        {
            int a = random.Next(1, 10); // Случайное число для загадки
            int b = random.Next(1, 10);
            Console.WriteLine($"Чтобы открыть сундук, решите: {a} + {b} = ?");

            while (true) // Бесконечный цикл для проверки ответа игрока
            {
                int answer = Convert.ToInt32(Console.ReadLine()); // Чтение ответа игрока

                if (answer == a + b) // Если ответ правильный
                {
                    Console.WriteLine("Сундук открыт! Вы нашли золото или зелье.");
                    if (random.Next(0, 2) == 0) // Случайно определяем, что найдено: золото или зелье
                    {
                        int foundGold = random.Next(5, 21); // Случайное количество золота от 5 до 20
                        gold += foundGold; // Добавляем найденное золото к общему количеству золота игрока
                        Console.WriteLine($"Теперь у вас {gold} золота.");
                    }
                    else // Если найдено зелье
                    {
                        for (int i = 0; i < inventory.Length; i++)
                        {
                            if (inventory[i] == null) // Находим первое пустое место в инвентаре
                            {
                                inventory[i] = "Healing Potion"; // Добавляем зелье исцеления в инвентарь
                                Console.WriteLine("Вы нашли зелье исцеления и добавили его в инвентарь.");
                                break;
                            }
                        }
                    }
                    break; // Выход из цикла после успешного открытия сундука
                }
                else
                {
                    Console.WriteLine("Неправильный ответ. Попробуйте снова."); // Сообщение о неправильном ответе
                }
            }
        }

    }
}
