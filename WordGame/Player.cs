using System;

namespace WordGame
{
    [Serializable]
    public class Player
    {
        public string Name { get;private set; }

        private int _numberWins;

        private bool _isFirstGame;

        public Player()
        {
            Console.WriteLine("Введите имя игрока:");
            Name = Console.ReadLine();
            _isFirstGame = true;
            if (_isFirstGame)
            {
                _numberWins = 0;
            }
        }

    }
}
