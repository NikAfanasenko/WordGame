using System;
using System.Collections.Generic;
using System.Timers;

namespace WordGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            Game();
        }

        static void Game()
        {
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
