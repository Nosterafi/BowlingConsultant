using BowlingConsultant.CommandManager;
using BowlingConsultant.Configuration;
using BowlingConsultant.BotWorker;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BowlingConsultant
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Configurathion.SetProperities(config);

            var activator = new BotWorker.BotWorker();
            activator.Start();

            var me = await activator.BotClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} started");
            Console.ReadLine();
        }
    }
}
