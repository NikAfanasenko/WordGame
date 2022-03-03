using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WordGame
{
    [DataContract]

    public class WordGame
    {
        private const int NumberPlayers = 2;
        private const int MaxLength = 30;
        private const int MinLength = 8;
        private const int NumberOfMS = 3000;
        private bool _isFirstWord;
        private bool _isEnd;
        private bool _isFirstPlayer;
        private bool _isRepeated;        
        private HashSet<char> _lettersInFirstWord;
        [DataMember]
        //private List<string> _words;
        private List<(string name, string word)> _words;
        [DataMember]
        private Player[] _players;
        private Dictionary<string,Action> _commands;
        //private string[] _commands;
        private string[]  _messages;
        private int _numberOfCommand;
        
        public WordGame()
        {
            _isFirstWord = true;
            _isFirstPlayer = true;
            _isEnd = false;
            _isRepeated = false;
            _words = new List<(string name, string word)>();
            //_words = new List<string>();
            _messages = new string[3] { "Cлово не подходящей длины!", "Слово некорректно!","Буквы в слове не совпадают!" };
            _commands = new Dictionary<string, Action>()
            {
                {"/show-words",LoadGame},
                {"/score",LoadGame},
                {"/total-score",LoadGame}
            };
            //_commands = new string[]{ "/show-words","/score","/total-score"};

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
            _isRepeated = false;
            MatchingWord();
            timer.Stop();
        }

        private void RewriteWord(KindOfMessages kind)
        {
            Console.WriteLine(_messages[(int)kind]);
            MatchingWord();
        }

        private void MatchingWord()
        {
            _isRepeated = false;
            string word = Console.ReadLine();
            if (!CheckLetters(word))
            {
                RewriteWord(KindOfMessages.MatchingWordError);
                _isRepeated = true;
            }
            if (_isFirstWord && CheckLenght(word) || !_isFirstWord && !CheckLenght(word, _words[0].word.Length))
            {
                RewriteWord(KindOfMessages.LengthError);
                _isRepeated = true;
            }            
            if (_isEnd)
            {
                return;
            }
            if (_isFirstWord)
            {
                _lettersInFirstWord = new HashSet<char>(word.ToLower().ToCharArray());
            }
            else
            {                
                if (!MatcingLetters(new HashSet<char>(word.ToLower().ToCharArray())))
                {
                    RewriteWord(KindOfMessages.MatchingLetterError);
                    _isRepeated = true;
                }
            }
            if (_isRepeated || _isEnd)
            {
                return;
            }
            if (_isFirstWord)
            {
                _words.Add(("Первоначальное слова",word));
            }
            else
            {
                _words.Add((_players[Convert.ToInt32(_isFirstPlayer)].Name, word));
            }

            SaveGame();          
        }

        private void InitializePlayers()
        {
            Console.Clear();
            _players = new Player[NumberPlayers]
            {
                new Player(),
                new Player()
            };
            _isEnd = false;
            _isFirstWord  = true;
        }
       
        private bool MatcingLetters(HashSet<char> letters)
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

        private bool CheckLetters(string word) => _isEnd ? true: new Regex("[а-я]").IsMatch(word);

        private bool CheckLenght(string word) => word.Length > MaxLength || word.Length<MinLength;

        private bool CheckLenght(string word, int lenght) => _isEnd ? true : word.Length < lenght ;
        
        private void SaveGame()
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(List<(string name, string word)>));
            using (var file = new FileStream("WordsInGame.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(file,_words);
            }     
        }
        
        private void LoadGame()
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(List<(string name, string word)>));
            using (var file = new FileStream("WordsInGame.json", FileMode.OpenOrCreate))
            {
                var words = jsonFormatter.ReadObject(file) as List<(string name, string word)>;
                if (words != null)
                {
                    foreach (var word in words)
                    {
                        Console.WriteLine(word.name +":"+word.word);
                    }
                }
            }
            Console.ReadLine();            
        }
    }
}
