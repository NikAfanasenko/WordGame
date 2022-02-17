using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Timers;

namespace WordGame
{
    public class Program
    {
        private static int POINT_MENU = 2;

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
                PrintMenu(currunt,pointMenu);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currunt!=0)
                        {
                            currunt--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currunt != 1)
                        {
                            currunt++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (currunt == 0)
                        {
                            Game();

                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
            }
            
        }

        static void PrintMenu(int current,string[] menu)
        {
            Console.Clear();
            for (int i = 0; i < POINT_MENU; i++)
            {
                Console.WriteLine("{0} {1}",current==i?"-->":"  ",menu[i]);
            }
        }

        static void Game()
        {
            Console.Clear();
            bool IsFirstPlayer = true;
            bool IsEndGame;
            string[] players = {"первый игрок", "второй игрок"};
            Console.WriteLine("Введите первоначальное слово:");
            string firstWord = Console.ReadLine();
            CheckNull(ref firstWord);
            List<char> lettersInFirstWord = RemoveRepeatLetters(firstWord.ToLower().ToCharArray());
            
            while (true)
            {
                Console.WriteLine($"{players[Convert.ToInt32(!IsFirstPlayer)]} :");
                IsFirstPlayer = ChangePlayer(IsFirstPlayer, lettersInFirstWord, out IsEndGame);
                if (IsEndGame)
                {
                    break;
                }
            }
            Console.WriteLine($"Победил {players[Convert.ToInt32(!IsFirstPlayer)]}");
            Console.ReadLine();
        }

        static List<char> RemoveRepeatLetters(char[] letters)
        {
            bool IsRepeat;
            List<char> notRepeatLetters = new List<char>();
            foreach (char letter in letters)
            {
                IsRepeat = false;
                foreach (char notRepeatLetter in notRepeatLetters)
                {
                    if (notRepeatLetter == letter)
                    {
                        IsRepeat = true;
                        break;
                    }
                }
                if (!IsRepeat)
                {
                    notRepeatLetters.Add(letter);
                }
            }
            return notRepeatLetters;
        }

        static bool CheckLetters(List<char> startLetters, List<char> letters)
        {
            bool IsExists;
            foreach (char letter in letters)
            {
                IsExists = false;
                foreach (char letterInStart in startLetters)
                {
                    if (letterInStart == letter)
                    {
                        IsExists = true;
                        break;
                    }
                }
                if (!IsExists)
                {
                    Console.WriteLine("Есть буквы, которых нет в первоначальном слове");
                    return true;
                }
            }
            return false;
        }

        static bool ChangePlayer(bool IsFirst, List<char> startLetters, out bool IsEnd)
        {
            
            string word = Console.ReadLine();
            CheckNull(ref word);
            List<char> lettersInWord = RemoveRepeatLetters(word.ToLower().ToCharArray());
            IsEnd = CheckLetters(startLetters, lettersInWord);
            return !IsFirst;
        }

        static void CheckNull(ref string word)
        {
            if (word == "")
            {
                Console.WriteLine("Неккоректное слово");
                word = Console.ReadLine();
                CheckNull(ref word);
            }
        }
    }
}
