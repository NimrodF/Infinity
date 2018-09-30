using System;

namespace RequestEngine
{
    public class CreateFailException : Exception
    {
        public CreateFailException(string createMessage) : base(message: "Create method failed- throw an exception")
        {
            CreateMessage = createMessage;
        }

        public string CreateMessage { get; }
    }

    public class NoKeyException : Exception
    {
        public NoKeyException() : base(message: "No Key- Key does not exist in Factory") { }
    }

    public class NullCreatorException : Exception
    {
        public NullCreatorException() : base(message: "No Creator- Creator method not provided") { }
    }
}
