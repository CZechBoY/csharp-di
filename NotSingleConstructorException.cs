using System;

namespace ChServer.DI
{
    class NotSingleConstructorException : Exception
    {
        private readonly Type type;

        public NotSingleConstructorException(Type type)
        {
            this.type = type;
        }
    }
}