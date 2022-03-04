using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    public class GameManager
    {
        public ILanguage Language { get; set; }
        private Menu _menu;
        private WordGame _game;        
        private event Action _startGameEventHandler;
        
        public GameManager()
        {
            Language = new RussianLanguage();
            _menu = new Menu(this);
            _game = new WordGame(this);
            _startGameEventHandler += _game.StartGame;
        }
        public void Start()
        {
            _menu.ChoosePointMenu(_startGameEventHandler);
        }
    }
}
