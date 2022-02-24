using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;

namespace WordGame
{
    public class WordGame
    {
        private const int NumberPlayers = 2;
        private const int MaxLength = 30;
        private const int MinLength = 8;
        private const int NumberOfMS = 3000;
        private bool _isFirstWord;
        private bool _isEnd;
        private bool _isFirstPlayer;
        private HashSet<char> _lettersInFirstWord;
        private Player[] _players;

        public WordGame()
        {
            _isFirstWord = true;
            _isFirstPlayer = true;            
            _isEnd = false;
        }
        
        public void StartGame()
        {
            InitializePlayers();
            while (true)
            {
                if (_isFirstWord)
                {
                    Console.Clear();
                    Console.WriteLine("Введите первоначальное слово: ");
                    StartTimer();
                    _isFirstWord = !_isFirstWord;
                    continue;
                }
                if (_isEnd)
                {
                    break;
                }
                Console.WriteLine($"{_players[Convert.ToInt32(!_isFirstPlayer)].Name} :");
                StartTimer();
                _isFirstPlayer = !_isFirstPlayer;              
            }
            Console.WriteLine($"Победил {_players[Convert.ToInt32(!_isFirstPlayer)].Name} !");
            Console.ReadLine();
        }

        private void StopGame(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Время закончилось!");
            Timer timer = (Timer)sender;
            timer.Stop();
            _isEnd = true;
        }
        
        private void StartTimer()
        {
            Timer timer = new Timer(NumberOfMS);
            timer.Elapsed += StopGame;
            if (!_isFirstWord)
            {                    
                timer.Start();
            }
            WriteWord();
            timer.Stop();
        }
        
        private void WriteWord()
        {
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
                    WriteWord();
                }
            }
        }

        private void InitializePlayers()
        {
            Console.Clear();
            _players = new Player[NumberPlayers]
            {
                new Player(),
                new Player()
            };
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
