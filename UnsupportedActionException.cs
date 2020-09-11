using System;

namespace AutoCrud
{
    public class UnsupportedActionException : Exception
    {
        public UnsupportedActionException()
        {
        }

        public UnsupportedActionException(string message) : base(message)
        {
        }
    }
}