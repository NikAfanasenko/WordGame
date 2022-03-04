using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WordGame
{
    [Serializable]
    public class Player
    {
        //[DataMember]
        public string Name { get;private set; }
        //[DataMember]
        public int NumberWins { get; set; }

        public Player(string sentences)
        {
            Console.WriteLine(sentences);
            Name = Console.ReadLine();
            NumberWins = 0;            
        }
        
        public Player(string name, int wins)
        {
            Name = name;
            NumberWins = wins;
        }
        
        public void IncrementWins() => NumberWins += 1;
    }
}
