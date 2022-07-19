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
            Platoon firstCountry = new Platoon(random, uniquePower);
            Platoon secondCountry = new Platoon(random, uniquePower);
            List<Soldier> soldiersOfFirstCountry = firstCountry.PrapareToBattle();
            List<Soldier> soldiersOfSecondCountry = secondCountry.PrapareToBattle();
            int numberOfSoldier = 0;
            int losingCountryNumber = 0;
            bool fight = true;

            while (fight)
            {
                Console.WriteLine("1.Показать бойцов стран. \n2.Начать бой. \n3.Выход. \nВыберите вариант:");
                string userInput = Console.ReadLine();
                Console.Clear();

                switch (userInput)
                {
                    case "1":
                        IntroduceCountries(firstCountry, secondCountry);
                        break;
                    case "2":
                        Fight(firstCountry, secondCountry, soldiersOfFirstCountry, soldiersOfSecondCountry, numberOfSoldier, ref losingCountryNumber);
                        break;
                    case "3":
                        fight = false;
                        break;
                }

                Console.WriteLine(" \nДля продолжнения нажмите любую клавишу: \n");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void Fight(Platoon firstCountry, Platoon secondCountry, List<Soldier> soldiersOfFirstCountry, 
                                                                        List<Soldier> soldiersOfSecondCountry, int numberOfSoldier, ref int losingCountryNumber)
        {
            if (soldiersOfFirstCountry.Count > 0 && soldiersOfSecondCountry.Count > 0)
            {
                bool fight = true;
                IntroduceCountries(firstCountry, secondCountry);

                while (fight)
                {
                    if (soldiersOfFirstCountry[numberOfSoldier].Power > soldiersOfSecondCountry[numberOfSoldier].Power)
                    {
                        soldiersOfSecondCountry.RemoveAt(numberOfSoldier);
                    }
                    else
                    {
                        soldiersOfFirstCountry.RemoveAt(numberOfSoldier);
                    }

                    fight = IsLose(firstCountry, secondCountry, ref losingCountryNumber);
                }
            }
            else
            {
                Console.WriteLine("Бой уже прошел!");
                WriteLoser(losingCountryNumber);
            }
        }

        private void ShowSoldiersOfCountry(Platoon country, int numberOfCountry)
        {
            Console.WriteLine($" \nСтрана номер {numberOfCountry}: \n");
            country.ShowSoldiers();
        }

        private void IntroduceCountries(Platoon firstCountry, Platoon secondCountry)
        {
            int numberOfCountry = 1;
            ShowSoldiersOfCountry(firstCountry, numberOfCountry);
            numberOfCountry = 2;
            ShowSoldiersOfCountry(secondCountry, numberOfCountry);
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
    }

    class Platoon
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Platoon(Random random, List<int> uniquePower)
        {
            AddSoldiers(random, uniquePower);
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
                    _soldiers[i].ShowInfo(i + 1);
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

        private void AddSoldiers(Random random, List<int> uniquePower)
        {
            int minSoldiers = 10;
            int maxSoldiers = 20;
            int minValue = 50;
            int maxValuePower = 100;
            int countOfSoldiers = random.Next(minSoldiers, maxSoldiers);

            for (int i = 0; i < countOfSoldiers;)
            {
                int power = random.Next(minValue, maxValuePower);
                bool isUniquePower = uniquePower.Contains(power);

                if (isUniquePower == false)
                {
                    uniquePower.Add(power);
                    _soldiers.Add(new Soldier(power));
                    i++;
                }
            }
        }
    }

    class Soldier
    {
        public int Power { get; private set; }

        public Soldier(int power)
        {
            Power = power;
        }

        public void ShowInfo(int id)
        {
            Console.WriteLine($"Боец номер {id}. Сила: {Power}.");
        }
    }
}
