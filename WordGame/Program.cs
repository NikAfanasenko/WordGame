using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;

namespace WordGame
{
    public class Program
    {
        private static bool IsFinishTime = false;
        private static int PointMenu = 2;
        private static int NumberofMS = 3000;
        private static int MaxLength = 30;
        private static int MinLength = 8;
        
        
        static void Main(string[] args)
        {
            Menu();
        }

        static void Menu()
        {
            string[] pointMenu = {"Старт","Выход"};
            int currunt = 1;
            while (true)
            {
                PrintMenu(currunt,menu: pointMenu);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currunt!=0)
                        {
                            currunt--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currunt != PointMenu - 1)
                        {
                            currunt++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (currunt)
                        {
                            case 0:
                                Game();
                                break;
                            case 1:
                                Environment.Exit(0);
                                break;
                        }
                        break;
                }
            }    
        }

        static void PrintMenu(int current,string[] menu)
        {
            Console.Clear();
            Console.Title = "Игра в слова";
            for (int i = 0; i < PointMenu; i++)
            {
                Console.WriteLine("{0} {1}",current==i?"-->":"  ",menu[i]);
            }
        }

        static void Game()
        {          
            bool IsFirstPlayer = true;
            bool IsEndGame;
            string[] players = {"первый игрок", "второй игрок"};
            Console.Clear();
            Console.WriteLine("Введите первоначальное слово:");
            string firstWord = Console.ReadLine();
            CheckWord(ref firstWord);
            CheckLenght(ref firstWord);                       
            HashSet<char> lettersInFirstWord = new HashSet<char>(firstWord.ToLower().ToCharArray());
            while (true)
            {
                Console.WriteLine($"{players[Convert.ToInt32(!IsFirstPlayer)]} :");
                IsFirstPlayer = ChangePlayer(IsFirst: IsFirstPlayer,startLetters: lettersInFirstWord, out IsEndGame);
                if (IsEndGame)
                {
                    break;
                }
            }
            Console.WriteLine($"Победил {players[Convert.ToInt32(!IsFirstPlayer)]}");
            Console.ReadLine();
        }

        static bool CheckLetters(HashSet<char> startLetters, HashSet<char> letters)
        {
            
            foreach (char letter in letters)
            {
                if (!startLetters.Contains(letter))
                {
                    return true;
                }
            }
            return false;
        }

        static bool ChangePlayer(bool IsFirst, HashSet<char> startLetters, out bool IsEnd)
        {
            Timer timer = new Timer(NumberofMS);
            timer.Elapsed += EndTime;
            timer.Start();
            string word = Console.ReadLine();
            /*try
            {
                word = Console.ReadLine();
            }
            catch (Exception)
            {

                throw;
            }*/
            //string word = Console.ReadLine();
            
            if (IsFinishTime)
            {
                IsEnd = true;
                return !IsFirst;
            }
            timer.Stop();
            CheckWord(ref word);
            HashSet<char> lettersInWord = new HashSet<char>(word.ToLower().ToCharArray());          
            IsEnd = CheckLetters(startLetters: startLetters,letters: lettersInWord);
            timer.Elapsed -= EndTime;
            return !IsFirst;
        }

        static void EndTime(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            timer.Stop();
            IsFinishTime = true;
        }

        static void CheckWord(ref string word)
        {
            Regex regex = new Regex("[а-я]");
            if (!regex.IsMatch(word))
            {
                Console.WriteLine("Слово должно состоять только из русских букв!");
                word = Console.ReadLine();
                CheckWord(ref word);
            }                   
        }

        static void CheckLenght(ref string word)
        {
            if (word.Length > MaxLength|| word.Length<MinLength)
            {
                Console.WriteLine("Длина первоначального слова не подхоит!");
                word = Console.ReadLine();
                CheckLenght(ref word);
            }
        }
    }
}
