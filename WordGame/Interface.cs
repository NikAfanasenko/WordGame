using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    public interface ILanguage
    {
        public string[] GetPointsMenu();

        public string[] GetOptionsMenu();

        public string[] GetMessages();

        public bool CheckLetters(string word);
    }
}
