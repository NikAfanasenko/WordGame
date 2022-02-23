using System;


namespace WordGame
{
    public class Player
    {
        public string Name { get;private set; }

        public Player()
        {
            Console.WriteLine("Введите имя игрока:");
            Name = Console.ReadLine();
        }

    }
}
