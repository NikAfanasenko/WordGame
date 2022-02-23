using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    public class Menu
    {
        private const int NumberPointsMenu = 2;
        private string[] _pointsMenu;
        private int _currunt;
        
        public Menu()
        {
            _pointsMenu = new string[NumberPointsMenu] { "Старт", "Выход" };
            _currunt = 1;
            Console.Title = "Игра в слова";
        }

        public void ChoosePointMenu(Action startGameEventHandler)
        {
            while (true)
            {
                PrintMenu(_currunt, menu: _pointsMenu);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_currunt != 0)
                        {
                            _currunt--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (_currunt != NumberPointsMenu - 1)
                        {
                            _currunt++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        switch (_currunt)
                        {
                            case 0:
                                startGameEventHandler?.Invoke();
                                break;
                            case 1:
                                Environment.Exit(0);
                                break;
                        }
                        break;
                }
            }
        }

        public void PrintMenu(int current, string[] menu)
        {
            Console.Clear();
            
            for (int i = 0; i < NumberPointsMenu; i++)
            {
                Console.WriteLine("{0} {1}", current == i ? "-->" : "  ", menu[i]);
            }
        }
    }
}
