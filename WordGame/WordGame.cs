using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;

namespace WordGame
{
    public class WordGame
    {
        
        private const int MaxLength = 30;
        private const int MinLength = 8;
        private const int NumberOfMS = 3000;
        private bool _isFirstWord;
        private bool _isEnd;
        private string[] _names;
        private HashSet<char> _lettersInFirstWord;
        

        public bool IsFirstPlayer { get; private set; }


        public WordGame(params string[] names)
        {
            _isFirstWord = true;
            IsFirstPlayer = true;
            _isEnd = false;
            _names = names;
        }
        public void StartGame()
        {
            while (true)
            {
                if (_isFirstWord)
                {
                    Console.Clear();
                    Console.WriteLine("Введите первоначальное слово: ");
                    WriteWord();
                    _isFirstWord = !_isFirstWord;
                    continue;
                }
                if (_isEnd)
                {
                    break;
                }
                /*if (IsFirstPlayer)
                {*/
                Console.WriteLine($"{_names[Convert.ToInt32(!IsFirstPlayer)]} :");
                WriteWord();
                IsFirstPlayer = !IsFirstPlayer;
                //}                
            }
            Console.WriteLine($"Победил {_names[Convert.ToInt32(!IsFirstPlayer)]} !");
            Console.ReadLine();
        }
        public void StopGame(object sender, ElapsedEventArgs e)
        {
            Timer timer = (Timer)sender;
            timer.Stop();
            _isEnd = true;
            //throw new EndTimeException("Время закончилось!");
        }
        public void WriteWord()
        {
            //try
            //{
                Timer timer = new Timer(NumberOfMS);
                timer.Elapsed += StopGame;
                if (!_isFirstWord)
                {                    
                    timer.Start();
                }
                string word = Console.ReadLine();
                CheckWord(ref word);                
                if (_isFirstWord)
                {
                    CheckLenght(ref word);
                    _lettersInFirstWord = new HashSet<char>(word.ToLower().ToCharArray());                
                }
                else
                {
                    bool isHaveAllLetters = CheckLetters(new HashSet<char>(word.ToLower().ToCharArray()));
                    if (!isHaveAllLetters)
                    {
                        Console.WriteLine("Есть буквы которых нет в превоначальном слове!");
                        //WriteWord();
                    }
                    timer.Stop();
                }
            //}
            /*catch (EndTimeException e)
            {
                Console.WriteLine($"Победил {_names[Convert.ToInt32(!IsFirstPlayer)]} !") ;
                Console.WriteLine(e.Message);
            }*/
        }
        
       private bool CheckLetters(HashSet<char> letters)
        {
            foreach (char letter in letters)
            {
                if (!_lettersInFirstWord.Contains(letter))
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckWord(ref string word)
        {
            Regex regex = new Regex("[а-я]");
            if (!regex.IsMatch(word))
            {
                Console.WriteLine("Слово должно состоять только из русских букв!");
                word = Console.ReadLine();
                
                CheckWord(ref word);
            }
        }

        private void CheckLenght(ref string word)
        {
            if (word.Length > MaxLength || word.Length < MinLength)
            {
                Console.WriteLine("Длина первоначального слова некорректная");
                word = Console.ReadLine();
                CheckLenght(ref word);
            }
        }
    }
}
