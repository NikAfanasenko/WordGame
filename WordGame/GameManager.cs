using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    public class GameManager
    {
        private Menu _menu;
        private WordGame _game;
        private event Action _startGameEventHandler;
        
        public GameManager()
        {
            _menu = new Menu();
            _game = new WordGame();
            _startGameEventHandler += _game.StartGame;
        }
        public void Start()
        {
            _menu.ChoosePointMenu(_startGameEventHandler);
        }
    }
}
