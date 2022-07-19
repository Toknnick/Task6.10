using System;
using System.Collections.Generic;

namespace Task6._10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();
            arena.Work();
        }
    }

    class Arena
    {
        public void Work()
        {
            Random random = new Random();
            List<int> uniquePower = new List<int>();
            List<int> uniqueHealth = new List<int>();
            List<string> logs = new List<string>();
            int numberOfSoldier = 0;
            int losingCountryNumber = 0;
            int totalSoldiers = 0;
            bool isFight = true;
            Platoon firstCountry = new Platoon(random, uniquePower, uniqueHealth, ref totalSoldiers);
            Platoon secondCountry = new Platoon(random, uniquePower, uniqueHealth, ref totalSoldiers);
            List<Soldier> soldiersOfFirstCountry = firstCountry.PrapareToBattle();
            List<Soldier> soldiersOfSecondCountry = secondCountry.PrapareToBattle();

            while (isFight)
            {
                Console.WriteLine("1.Показать бойцов стран. \n2.Начать бой. \n3.Просмотреть логи. \n4.Выход. \nВыберите вариант:");
                string userInput = Console.ReadLine();
                Console.Clear();

                switch (userInput)
                {
                    case "1":
                        IntroduceCountries(soldiersOfFirstCountry, soldiersOfSecondCountry, firstCountry, secondCountry);
                        break;
                    case "2":
                        Fight(firstCountry, secondCountry, soldiersOfFirstCountry, soldiersOfSecondCountry, numberOfSoldier, ref losingCountryNumber, logs);
                        break;
                    case "3":
                        ShowLogs(logs);
                        break;
                    case "4":
                        isFight = false;
                        break;
                }

                Console.WriteLine(" \nДля продолжнения нажмите любую клавишу: \n");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void IntroduceCountries(List<Soldier> soldiersOfFirstCountry, List<Soldier> soldiersOfSecondCountry, Platoon firstCountry, Platoon secondCountry)
        {
            int numberOfCountry = 1;
            ShowSoldiersOfCountry(soldiersOfFirstCountry, firstCountry, numberOfCountry);
            numberOfCountry = 2;
            ShowSoldiersOfCountry(soldiersOfSecondCountry, secondCountry, numberOfCountry);
        }

        private void Fight(Platoon firstCountry, Platoon secondCountry, List<Soldier> soldiersOfFirstCountry,
                                                                        List<Soldier> soldiersOfSecondCountry, int numberOfSoldier, ref int losingCountryNumber, List<string> logs)
        {
            int numberOfFirstCountry = 1;
            int numberOfSecondCountry = 2;
            int optionAddingLogs = 0;

            if (soldiersOfFirstCountry.Count > 0 && soldiersOfSecondCountry.Count > 0)
            {
                bool isFight = true;
                IntroduceCountries(soldiersOfFirstCountry, soldiersOfSecondCountry, firstCountry, secondCountry);

                while (isFight)
                {
                    if (soldiersOfFirstCountry[numberOfSoldier].Power >= soldiersOfSecondCountry[numberOfSoldier].Health)
                    {
                        optionAddingLogs = 1;
                        logs.Add(AddLogs(soldiersOfFirstCountry, soldiersOfSecondCountry, optionAddingLogs, numberOfSoldier,numberOfFirstCountry));
                        soldiersOfSecondCountry.RemoveAt(numberOfSoldier);
                    }
                    else if (soldiersOfSecondCountry[numberOfSoldier].Power >= soldiersOfFirstCountry[numberOfSoldier].Health)
                    {
                        optionAddingLogs = 1;
                        logs.Add(AddLogs(soldiersOfSecondCountry, soldiersOfFirstCountry, optionAddingLogs, numberOfSoldier, numberOfSecondCountry));
                        soldiersOfFirstCountry.RemoveAt(numberOfSoldier);
                    }
                    else
                    {
                        optionAddingLogs = 2;
                        logs.Add(AddLogs(soldiersOfSecondCountry, soldiersOfFirstCountry, optionAddingLogs, numberOfSoldier));
                        soldiersOfFirstCountry[numberOfSoldier].TakeDamage(soldiersOfSecondCountry[numberOfSoldier].Power);
                        soldiersOfSecondCountry[numberOfSoldier].TakeDamage(soldiersOfFirstCountry[numberOfSoldier].Power);
                    }

                    isFight = IsLose(firstCountry, secondCountry, ref losingCountryNumber);
                }
            }
            else
            {
                Console.WriteLine("Бой уже прошел!");
                WriteLoser(losingCountryNumber);
            }
        }

        private void ShowLogs(List<string> logs)
        {
            if (logs.Count > 0)
            {
                foreach (string log in logs)
                {
                    Console.WriteLine(log);
                }
            }
            else
            {
                Console.WriteLine("Логи пусты!");
            }
        }

        private string AddLogs(List<Soldier> firstCountry, List<Soldier> secondCountry, int optionAddingLogs, int numberOfSoldier, int numberOfCountry = 0)
        {
            string text = "";

            if (optionAddingLogs == 1)
            {
                text = $"Боец {firstCountry[numberOfSoldier].Name} страны номер {numberOfCountry} убил(а) вражеского бойца {secondCountry[numberOfSoldier].Name}.";
            }
            else
            {
                text = $"Боец {firstCountry[numberOfSoldier].Name} нанес(ла) урон вражескому бойцу " +
                       $"{secondCountry[numberOfSoldier].Name} и получил(а) урон в ответ";
            }

            return text;
        }

        private void ShowSoldiersOfCountry(List<Soldier> soldiersOfCountry, Platoon country, int numberOfCountry)
        {
            Console.WriteLine($" \nСтрана номер {numberOfCountry}: \n");
            country.ShowSoldiers();

            if (soldiersOfCountry.Count > 0)
            {
                Console.WriteLine($"Военная мощь страны: {GetTotalPowerOfCountry(soldiersOfCountry)}");
            }
        }

        private bool IsLose(Platoon firstCountry, Platoon secondCountry, ref int losingCountryNumber)
        {
            bool isLose = true;

            if (firstCountry.IsLive() == false || secondCountry.IsLive() == false)
            {
                isLose = false;
                Console.WriteLine();

                if (firstCountry.IsLive() == false)
                {
                    losingCountryNumber = 1;
                    WriteLoser(losingCountryNumber);
                }
                else if (secondCountry.IsLive() == false)
                {
                    losingCountryNumber = 2;
                    WriteLoser(losingCountryNumber);
                }
            }

            return isLose;
        }

        private void WriteLoser(int losingCountryNumber)
        {
            Console.WriteLine($"Страна номер {losingCountryNumber} приняла поражение!");
        }

        private int GetTotalPowerOfCountry(List<Soldier> soldiersOfCountry)
        {
            int totalPowerOfCountry = 0;
            if (soldiersOfCountry.Count > 0)
            {
                for (int i = 0; i < soldiersOfCountry.Count; i++)
                {
                    totalPowerOfCountry += soldiersOfCountry[i].Power;
                }
            }

            return totalPowerOfCountry;
        }
    }

    class Platoon
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Platoon(Random random, List<int> uniquePower, List<int> uniqueHealth, ref int totalSoldiers)
        {
            AddSoldiers(random, uniquePower, uniqueHealth, ref totalSoldiers);
        }

        public List<Soldier> PrapareToBattle()
        {
            return _soldiers;
        }

        public void ShowSoldiers()
        {
            if (_soldiers.Count > 0)
            {
                for (int i = 0; i < _soldiers.Count; i++)
                {
                    _soldiers[i].ShowInfo();
                }
            }
            else
            {
                Console.WriteLine("В стране все бойцы погибли.");
            }
        }

        public bool IsLive()
        {
            bool isLive = true;

            if (_soldiers.Count <= 0)
            {
                isLive = false;
            }

            return isLive;
        }

        private void AddSoldiers(Random random, List<int> uniquePower, List<int> uniqueHealth, ref int totalSoldiers)
        {
            int minSoldiers = 10;
            int maxSoldiers = 20;
            int minValue = 50;
            int maxValue = 100;
            int numberOfName = 0;
            int countOfSoldiers = random.Next(minSoldiers, maxSoldiers);

            for (int i = 0; i < countOfSoldiers;)
            {
                int health = random.Next(minValue, maxValue);
                int power = random.Next(minValue, maxValue);
                bool isUniquePower = uniquePower.Contains(power);
                bool isUniqueHealth = uniqueHealth.Contains(health);

                if (isUniquePower == false && isUniqueHealth == false)
                {
                    uniquePower.Add(power);
                    uniqueHealth.Add(health);
                    _soldiers.Add(new Soldier((NamesOfSoldiers)(numberOfName + totalSoldiers), health, power));
                    i++;
                    totalSoldiers++;
                }
            }
        }
    }

    enum NamesOfSoldiers
    {
        Amy,
        Roy,
        Jack,
        Rose,
        Jean,
        Toni,
        Ruby,
        Gary,
        John,
        Billy,
        James,
        Megan,
        Marie,
        Diane,
        Annie,
        Kevin,
        Frank,
        Donald,
        Johnny,
        Willie,
        Gladys,
        Marion,
        Edward,
        Thomas,
        Carrie,
        Robert,
        Bessie,
        Adrian,
        Steven,
        Sheila,
        Joseph,
        Frances,
        Beverly,
        Barbara,
        Dorothy,
        Michael,
        Richard,
        Veronica,
        Victoria,
        Jennifer,
    }

    class Soldier
    {
        public NamesOfSoldiers Name { get; private set; }

        public int Health { get; private set; }
        public int Power { get; private set; }

        public Soldier(NamesOfSoldiers name, int health, int power)
        {
            Health = health;
            Power = power;
            Name = name;
        }

        public void TakeDamage(int power)
        {
            float armor = 0.6f;
            Health -= (int)(power * armor);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Боец {Name}. Здоровье: {Health}. Сила: {Power}.");
        }
    }
}
