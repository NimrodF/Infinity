using System.Threading;

namespace RequestEngine
{
    public class BadApple : IExecutable
    {
        public BadApple() { }

        public void Execute()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
