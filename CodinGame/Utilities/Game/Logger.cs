using System;
using System.Text.Json;

namespace CodinGame.Utilities.Game
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Console.Error.WriteLine(message);
        }

        public static void Log(string message, object package)
        {
            var serializedPackage = JsonSerializer.Serialize(package);
            Console.Error.WriteLine($"{message} --> {serializedPackage}");
        }

        public static void Log(object package)
        {
            var serializedPackage = JsonSerializer.Serialize(package);
            Console.Error.WriteLine(serializedPackage);
        }
    }
}