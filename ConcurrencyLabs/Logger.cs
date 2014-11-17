using System.Diagnostics;

namespace ConcurrencyLabs
{
    static class Logger
    {
        public static void WriteLine(object message)
        {
            Trace.WriteLine(message);
        }
    }
}