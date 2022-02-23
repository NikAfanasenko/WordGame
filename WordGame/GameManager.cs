using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    public class GameManager
    {


        private const int NumberPlayers = 2;
        private Player[] _players;
        private Menu _menu;
        private WordGame _game;

        //public event Action Begin;
        private event Action _startGameEventHandler;
        

        public GameManager()
        {
            _players = new Player[NumberPlayers]{
                new Player(),
                new Player()
            };

            _menu = new Menu();
            _game = new WordGame(_players[0].Name,_players[1].Name);
            _startGameEventHandler += _game.StartGame;
            //Begin += Start;
        }
        public void Start()
        {
            _menu.ChoosePointMenu(_startGameEventHandler);
        }
    }
}
