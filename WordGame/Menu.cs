using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    public class Menu
    {
        private const int NumberPointsMenu = 3;
        private const int NumberPointsOptions = 2;
        private int _points;
        private string[] _pointsMenu;        
        private int _currunt;        
        private bool _isOptions;
        private bool _isContinue;
        private GameManager _manager;
        
        public Menu(GameManager gameManager)
        {
            _manager = gameManager;
            _isOptions = false;
            _isContinue = false;
            _pointsMenu = _manager.Language.GetPointsMenu();
            _points = NumberPointsMenu;
            _currunt = 0;
        }

        public void ChoosePointMenu(Action startGameEventHandler)
        {
            while (true)
            {
                PrintMenu(current: _currunt, menu: _pointsMenu);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_currunt != 0)
                        {
                            _currunt--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_currunt != _points - 1)
                        {
                            _currunt++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (_isOptions)
                        {
                            case false:
                                switch (_currunt)
                                {
                                    case 0:
                                        startGameEventHandler?.Invoke();
                                        break;
                                    case 1:
                                        _pointsMenu = _manager.Language.GetOptionsMenu();
                                        _points = NumberPointsOptions;
                                        _isOptions = true;
                                        break;
                                    case 2:
                                        Environment.Exit(0);
                                        break;
                                }
                                break;
                            case true:
                                switch (_currunt)
                                {
                                    case 0:
                                        if (_manager.Language is RussianLanguage)
                                        {
                                            break;
                                        }
                                        _manager.Language = new RussianLanguage();                                                                             
                                        break;
                                    case 1:
                                        if (_manager.Language is EnglishLanguage)
                                        {
                                            break;
                                        }
                                        _manager.Language = new EnglishLanguage();
                                        break;
                                }
                                _isOptions = false;
                                _pointsMenu = _manager.Language.GetPointsMenu();
                                _points = NumberPointsMenu;
                                break;
                        }                        
                        break;
                    case ConsoleKey.Escape:
                        if (_isOptions)
                        {
                            _pointsMenu = _manager.Language.GetPointsMenu();
                            _points = NumberPointsMenu;
                        }                        
                        break;
                }
            }
        }

        public void PrintMenu(int current, string[] menu)
        {
            Console.Clear();
            
            for (int i = 0; i < _points; i++)
            {
                Console.WriteLine("{0} {1}", current == i ? "-->" : "  ", menu[i]);
            }
        }
    }
}
