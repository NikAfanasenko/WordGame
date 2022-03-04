using System;
using System.Text.RegularExpressions;

namespace WordGame
{
    public class RussianLanguage : ILanguage
    {
        public bool CheckLetters(string word) => new Regex("[а-я]").IsMatch(word);

        public string[] GetMessages()=>new string[] {
            "Cлово не подходящей длины!",
            "Слово некорректно!",
            "Буквы в слове не совпадают!",
            "Введите первоначальное слово: ",
            "Первоначальное слово",
            "Победил",
            "Время закончилось!",
            "Введите имя игрока: "
        };
        

        public string[] GetOptionsMenu() => new string[] {"Русский","Английский" };

        public string[] GetPointsMenu() => new string[] { "Старт", "Настройка", "Выход" };
    }
}
