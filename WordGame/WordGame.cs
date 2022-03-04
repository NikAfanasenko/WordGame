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
        private List<(string name, string word)> _words;
        [DataMember]
        private Player[] _players;
        private Dictionary<string,Action> _commands;        
        private string[]  _messages; 
        private GameManager _manager;

        public WordGame(GameManager gameManager)
        {
            _manager = gameManager;
            _isFirstWord = true;
            _isFirstPlayer = true;
            _isEnd = false;
            _isRepeated = false;
            _words = new List<(string name, string word)>();
            _commands = new Dictionary<string, Action>()
            {
                {"/show-words",LoadWordsInGame},
                {"/score",LoadScore},
                {"/total-score",LoadWordsInGame}
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
                SaveScore();
                Console.ReadLine();
                //check = true;
            //}
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
                _words.Add((_messages[(int)KindOfMessages.FirstWord],word));
            }
            else
            {
                _words.Add((_players[Convert.ToInt32(_isFirstPlayer)].Name, word));
            }

            SaveWords();          
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

        private bool CheckLetters(string word) => _isEnd ? true : _manager.Language.CheckLetters(word);

        private bool CheckLenght(string word) => word.Length > MaxLength || word.Length<MinLength;

        private bool CheckLenght(string word, int lenght) => _isEnd ? true : word.Length < lenght;
        
        private void SaveWords()
        {
            var saveWords = new DataContractJsonSerializer(typeof(List<(string name, string word)>));
            using (var file = new FileStream("WordsInGame.json", FileMode.OpenOrCreate))
            {
                saveWords.WriteObject(file,_words);                
            }            
        }
        
        private void SaveScore()
        {
            var saveScore = new DataContractJsonSerializer(typeof(Player[]));
            using (var file = new FileStream("Score.json", FileMode.OpenOrCreate))
            {
                saveScore.WriteObject(file, _players);
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
            Console.ReadLine();
        }

        private void LoadWordsInGame()
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

        private void SaveTotalScore()
        {
            /*var totalScore = new DataContractJsonSerializer(typeof(Player[]));
            Player[] totalPlayers;
            using (var file = new FileStream("TotalScore.json", FileMode.OpenOrCreate))
            {
                var players = totalScore.ReadObject(file) as Player[];
                totalPlayers = players;
                if (players != null)
                {
                    foreach (var player in _players)
                    {
                        bool isHave = false;
                        int i = 0;
                        foreach (var totalPlayer in players)
                        {
                            if (player.Name.ToLower()==totalPlayer.Name.ToLower())
                            {                                
                                isHave = true;
                                break;
                            }
                            i++;
                        }                       
                        if (isHave)
                        {
                            players[i].NumberWins += player.NumberWins;
                        }
                        else
                        {
                            Player[] players1 = totalPlayers;
                            totalPlayers = new Player[players.Length + 1];
                            int k = 0;
                            foreach (var plyr in players1)
                            {
                                totalPlayers[k] = plyr;
                            }
                            totalPlayers[^1] = player;
                        }
                    }                    
                }
            }
            foreach (var item in totalPlayers)
            {

                Console.WriteLine(item.Name);
            }
            using (var file = new FileStream("TotalScore.json", FileMode.OpenOrCreate))
            {
                totalScore.WriteObject(file, totalPlayers);
            }*/
        }
        private void LoadTotalScore()
        {
            /*var totalScore = new DataContractJsonSerializer(typeof(Player[]));
            using (var file = new FileStream("TotalScore.json", FileMode.OpenOrCreate))
            {
                var players = totalScore.ReadObject(file) as Player[];
                if (players != null)
                {
                    foreach (var player in players)
                    {
                        Console.WriteLine(player.Name + ":" + player.NumberWins);
                    }
                }
            }
            Console.ReadLine();*/
        }
    }
}
