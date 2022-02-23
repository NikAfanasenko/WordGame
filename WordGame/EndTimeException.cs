using System;

namespace WordGame
{
    public class EndTimeException : Exception
    {
        public EndTimeException(string message): base(message)
        {

        }
    }
}
