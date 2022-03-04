using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordGame
{
    public class EnglishLanguage : ILanguage
    {
        public bool CheckLetters(string word) => new Regex("[a-z]").IsMatch(word);

        public string[] GetMessages() =>new string[] { 
            "The word is not the right length!",
            "Word is not correct!",
            "Letters is not match",
            "Enter first word: ",
            "First word",
            "Win ",
            "Time out!",
            "Write nikname: "
        };        

        public string[] GetOptionsMenu() => new string[] { "Russian", "English" };

        public string[] GetPointsMenu() => new string[] {"Start", "Options","Exit" };
        
    }
}
