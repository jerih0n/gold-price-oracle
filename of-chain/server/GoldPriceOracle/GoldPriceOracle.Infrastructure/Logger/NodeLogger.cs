using Newtonsoft.Json;
using System;

namespace GoldPriceOracle.Infrastructure.Integration.Logger
{
    public class NodeLogger : INodeLogger
    {
        public void LogInformation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
        }

        public void LogInformation(string message, object logData)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            var data = JsonConvert.SerializeObject(logData);
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}