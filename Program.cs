using BowlingConsultant.CommandManager;
using BowlingConsultant.Configuration;
using BowlingConsultant.BeginingWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Net.Mime.MediaTypeNames;

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

            var activator = new BotActivator();
            activator.Start();
            var me = await activator.BotClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} started");
            Console.ReadLine();
        }
    }
}
