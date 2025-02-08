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
            Random random = new Random(); // Инициализация генератора случайных чисел
                                          // Определение карты подземелья без босса
            string[] dungeonMap = { "Monster", "Trap", "Chest", "Merchant", "Empty", "Monster", "Trap", "Chest", "Merchant" };
            string[] inventory = new string[5]; // Инвентарь игрока
            int health = 100; // Здоровье игрока
            int potions = 3; // Количество зелий
            int gold = 0; // Количество золота
            int arrows = 5; // Количество стрел
            int currentRoom; // Текущая комната
            int experience = 0; // Опыт игрока
            int level = 1; // Уровень игрока
            int monstersDefeated = 0; // Счетчик побежденных монстров

            // Перемешиваем карту подземелья
            ShuffleArray(dungeonMap, random);

            // Проход по всем комнатам подземелья
            for (int room = 0; room < dungeonMap.Length; room++)
            {
                Console.WriteLine($"Вы входите в комнату {room + 1}. Здесь {dungeonMap[room]}.");
                currentRoom = room;

                switch (dungeonMap[currentRoom]) // Определяем, что происходит в текущей комнате
                {
                    case "Monster":
                        FightMonster(random, ref health, ref arrows, ref experience);
                        monstersDefeated++; // Увеличиваем счетчик побежденных монстров
                        break;
                    case "Trap":
                        TriggerTrap(random, ref health); // Попадание в ловушку
                        break;
                    case "Chest":
                        OpenChest(random, ref inventory, ref gold); // Открытие сундука
                        break;
                    case "Merchant":
                        VisitMerchant(ref gold, ref potions); // Посещение торговца
                        break;
                    case "Empty":
                        Console.WriteLine("Комната пустая. Ничего не происходит."); // Пустая комната
                        break;
                }

                if (health <= 0) // Проверка на смерть игрока
                {
                    Console.WriteLine("Вы погибли. Игра окончена.");
                    return; // Завершение игры
                }

                LevelUp(ref experience, ref level); // Проверка повышения уровня после каждой комнаты
                NumberQuest(ref health); // Числовой квест после каждой комнаты

                // Проверка на победу над 9 монстрами и бой с боссом
                if (monstersDefeated == 9)
                {
                    Console.WriteLine("Вы победили 9 монстров! Теперь вам предстоит сразиться с боссом!");
                    FightBoss(random, ref health, ref arrows, ref experience); // Бой с боссом
                    break; // Завершаем цикл после боя с боссом
                }
            }

            Console.WriteLine("Вы победили всех врагов и вышли из подземелья. Поздравляем!");
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

        // Метод для посещения торговца
        static void VisitMerchant(ref int gold, ref int potions)
        {
            Console.WriteLine("Торговец предлагает купить зелье за 30 золота.");
            Console.WriteLine($"У вас {gold} золота.");

            if (gold >= 30) // Проверка наличия достаточного количества золота для покупки зелья
            {
                gold -= 30; // Уменьшаем золото на стоимость зелья
                potions++; // Увеличиваем количество зелий на 1
                Console.WriteLine("Вы купили зелье. Теперь у вас " + potions + " зелий и " + gold + " золота.");
            }
            else
            {
                Console.WriteLine("У вас недостаточно золота для покупки зелья."); // Сообщение о недостаточном золоте
            }
        }

        // Метод для боя с боссом
        static void FightBoss(Random random, ref int health, ref int arrows, ref int experience)
        {
            int bossHealth = random.Next(50, 101); // Здоровье босса в диапазоне от 50 до 100
            Console.WriteLine($"Вы встретили босса с {bossHealth} HP.");

            while (bossHealth > 0 && health > 0) // Бой продолжается пока живы и игрок, и босс
            {
                Console.WriteLine("Выберите оружие: 1 - Меч, 2 - Лук");
                string choice = Console.ReadLine();

                int damage = 0; // Урон, который игрок наносит боссу
                if (choice == "1") // Если выбрано оружие "меч"
                {
                    damage = random.Next(10, 21); // Урон меча от 10 до 20
                    Console.WriteLine($"Вы нанесли боссу {damage} урона.");
                }
                else if (choice == "2" && arrows > 0) // Если выбрано оружие "лук" и есть стрелы
                {
                    damage = random.Next(5, 16); // Урон лука от 5 до 15
                    arrows--; // Уменьшаем количество стрел на 1
                    Console.WriteLine($"Вы нанесли боссу {damage} урона. Осталось стрел: {arrows}");
                }
                else if (choice == "2" && arrows <= 0) // Если выбрано оружие "лук", но стрел нет
                {
                    Console.WriteLine("У вас нет стрел для лука!");
                    continue; // Пропускаем итерацию цикла и продолжаем бой
                }

                bossHealth -= damage; // Уменьшаем здоровье босса на полученный урон
                if (bossHealth > 0) // Если босс все еще жив
                {
                    int bossDamage = random.Next(10, 21); // Урон от босса от 10 до 20
                    health -= bossDamage; // Уменьшаем здоровье игрока на урон босса
                    Console.WriteLine($"Босс нанес вам {bossDamage} урона. Осталось здоровья: {health}");
                }
            }

            if (health > 0) // Если игрок победил босса
            {
                Console.WriteLine("Вы победили босса! Поздравляем!");
                experience += 50; // Награда за победу - получение опыта
            }
        }

        // Метод для проверки повышения уровня игрока
        static void LevelUp(ref int experience, ref int level)
        {
            if (experience >= level * 100) // Для повышения уровня требуется 100 опыта на уровень
            {
                level++; // Повышаем уровень игрока на единицу
                experience -= level * 100; // Сбрасываем опыт после повышения уровня 
                Console.WriteLine($"Поздравляем! Вы достигли уровня {level}!");
            }
        }

        // Метод для выполнения числового квеста после каждой комнаты
        static void NumberQuest(ref int health)
        {
            int a = new Random().Next(1, 10); // Случайное число для квеста 
            int b = new Random().Next(1, 10);

            Console.WriteLine($"Решите: {a} + {b} = ?");

            while (true) // Бесконечный цикл для проверки ответа игрока 
            {
                try
                {
                    int answer = Convert.ToInt32(Console.ReadLine()); // Чтение ответа игрока

                    if (answer == a + b) // Если ответ правильный 
                    {
                        Console.WriteLine("Правильно! Вы восстановили здоровье на 10 HP.");
                        health += 10; // Восстанавливаем здоровье игрока на 10 HP 
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неправильный ответ. Вы потеряли 5 HP.");
                        health -= 5;
                        if (health <= 0)
                        {
                            Console.WriteLine("Вы погибли от потери здоровья. Игра окончена.");
                            Environment.Exit(0); // Завершаем игру 
                        }
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Пожалуйста, введите число.");
                }
            }

            Console.WriteLine($"Ваше текущее здоровье: {health}");
        }

    }
}
