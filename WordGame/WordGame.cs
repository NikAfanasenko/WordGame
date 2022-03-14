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
        [DataMember]        
        private List<string> _words;
        [DataMember]
        private Player[] _players;
        private string[] _messages;
        private Dictionary<string,Action> _commands;
        private HashSet<char> _lettersInFirstWord;
        private GameManager _manager;
        private Player[] _allPlayers;

        public WordGame(GameManager gameManager)
        {
            _manager = gameManager;
            _isFirstWord = true;
            _isFirstPlayer = true;
            _isEnd = false;
            _isRepeated = false;            
            _commands = new Dictionary<string, Action>()
            {
                {"/show-words",ShowWords},
                {"/score",LoadScore},
                {"/total-score",PrintFile}
            };

        }
        public void StartGame()
        {
            _messages = _manager.Language.GetMessages();
            InitializePlayers();
            while (true)
            {
                if (_isFirstWord)
                {
                    Console.Clear();
                    Console.WriteLine(_messages[(int)KindOfMessages.InputFirstWord]);
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
            Console.WriteLine($"{_messages[(int)KindOfMessages.Congratulation]} {_players[Convert.ToInt32(!_isFirstPlayer)].Name} !");
            _players[Convert.ToInt32(!_isFirstPlayer)].IncrementWins();
            SaveScore("Score.json",_players);
            Save();
            Console.ReadLine();
        }

        private void StopGame(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"{_messages[(int)KindOfMessages.TimeOutError]}");
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
            if (CheckCommand(word))
            {
                RunCommand(word);
                MatchingWord();
                return;
            }
            if (!CheckLetters(word))
            {
                RewriteWord(KindOfMessages.MatchingWordError);
                _isRepeated = true;
            }
            if (_isFirstWord && CheckLenght(word) || !_isFirstWord && !CheckLenght(word, _words[0].Length))
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
            _words.Add(word);         
        }

        private void InitializePlayers()
        {
            Console.Clear();            
            _players = new Player[NumberPlayers]
            {
                new Player(_messages[(int)KindOfMessages.InputName]),
                new Player(_messages[(int)KindOfMessages.InputName])
            };           
            _isEnd = false;
            _isFirstWord  = true;
            _isFirstPlayer = true;
            _words = new List<string>();
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
        private bool CheckCommand(string command) => new Regex(@"\/[a-z]").IsMatch(command);

        private void RunCommand(string command) => _commands[command]?.Invoke();

        private bool CheckLetters(string word) => _isEnd ? true : _manager.Language.CheckLetters(word);

        private bool CheckLenght(string word) => word.Length > MaxLength || word.Length<MinLength;

        private bool CheckLenght(string word, int lenght) => _isEnd ? true : word.Length < lenght;
        
        private void SaveScore(string path,Player[] players)
        {
            var saveScore = new DataContractJsonSerializer(typeof(Player[]));
            using (var file = new FileStream(path, FileMode.OpenOrCreate))
            {
                saveScore.WriteObject(file, players);
            }
        }
        
        private void LoadScore()
        {
            var score = new DataContractJsonSerializer(typeof(Player[]));
            using (var file = new FileStream("Score.json",FileMode.OpenOrCreate))
            {
                var players = score.ReadObject(file) as Player[];
                if (players != null)
                {
                    foreach (var player in players)
                    {
                        Console.WriteLine(player.Name +":"+ player.NumberWins);
                    }
                }
            }
        }

        private void ShowWords()
        {
            foreach (string word in _words)
            {
                Console.WriteLine(word);
            }
        }

        private void Save()
        {
            List<Player> players = new List<Player>();
            Load();
            bool isCopy = false;
            if (_allPlayers.Length == 0)
            {
                _allPlayers = _players;
                isCopy = true;
            }
            foreach (var totalPlayer in _allPlayers)
            {
                bool isAdd = false;
                if (!isCopy)
                {
                    foreach (var player in _players)
                    {
                        if (player.Name == totalPlayer.Name)
                        {
                            players.Add(new Player(totalPlayer.Name, totalPlayer.NumberWins + player.NumberWins));
                            isAdd = true;
                            break;
                        }
                    }
                }                                
                if (!isAdd)
                {
                    players.Add(totalPlayer);
                }
            }
            foreach (var player in _players)
            {
                bool isAdd = false;
                foreach (var totalPlayer in _allPlayers)
                {
                    if (player.Name == totalPlayer.Name)
                    {
                        isAdd = true;
                        break;
                    }
                }
                if (!isAdd)
                {
                    players.Add(player);
                }
            }
            _allPlayers = new Player[players.Count];
            int i = 0;
            foreach (var player in players)
            {
                _allPlayers[i] = player;
                i++;
            }
            isCopy = false;
            var saveScore = new DataContractJsonSerializer(typeof(Player[]));
            using (var file = new FileStream("TotalScore.json", FileMode.OpenOrCreate))
            {
                saveScore.WriteObject(file, _allPlayers);
            }
        }

        private void PrintFile()
        {
            Load();
            foreach (var item in _allPlayers)
            {
                Console.WriteLine(item.Name + ":" + item.NumberWins);
            }
        }

        private void Load()
        {
            var score = new DataContractJsonSerializer(typeof(Player[]));
            using (var file = new FileStream("TotalScore.json", FileMode.OpenOrCreate))
            {
                try
                {
                    var players = score.ReadObject(file) as Player[];
                    if (players != null)
                    {
                        _allPlayers = players;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка с файлом");
                }
                    
            }                            
        }
    }
}
