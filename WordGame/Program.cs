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
            Console.WriteLine("Введите первоначальное слово:");
            string firstWord = Console.ReadLine();
            
            CheckWord(ref firstWord);
            Console.WriteLine(firstWord);
            char[] allLettersInFirstWord = firstWord.ToLower().ToCharArray();
            List<char> lettersInFirstWord = RemoveRepeatLetters(allLettersInFirstWord);
            bool IsFirstPlayer = true;
            bool IsEndGame = false;
            while (true)
            {
                IsFirstPlayer = ChangePlayer(IsFirstPlayer, in lettersInFirstWord, out IsEndGame);
                if (IsEndGame)
                {
                    break;
                }
                
            }
            if (IsFirstPlayer)
            {
                Console.Write("Победил первый игрок");
            }
            else
            {
                Console.Write("Победил второй игрок");
            }
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

        static bool CheckLetters(in List<char> startLetters,in List<char> letters)
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

        static bool ChangePlayer(bool IsFirst,in List<char> startLetters, out bool IsEnd)
        {
            
            string word = Console.ReadLine();
            CheckWord(ref word);
            char[] allLettersInWord = word.ToLower().ToCharArray();
            List<char> lettersInWord = RemoveRepeatLetters(allLettersInWord);
            IsEnd = CheckLetters(in startLetters,in lettersInWord);
            return !IsFirst;
        }

        static void CheckWord(ref string word)
        {
            if (word == ""|| word.Length>30)
            {
                Console.WriteLine("Неккоректное слово");
                word = Console.ReadLine();
                CheckWord(ref word);
            }
        }
    }
}
