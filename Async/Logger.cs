using System;

namespace Async
{
    public static class Logger
    {
        private static Action<string> Log(ConsoleColor color) =>
            msg =>
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
            };

        public static Action<string> LogInfo = Log(ConsoleColor.White);
        public static Action<string> LogWarning = Log(ConsoleColor.Yellow);
        public static Action<string> LogSuccess = Log(ConsoleColor.Green);
        public static Action<string> LogError = Log(ConsoleColor.Red);
    }
}
