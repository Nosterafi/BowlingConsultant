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
            var _botClient = new TelegramBotClient(Configurathion.TelegramSettings.BotToken);

            var messageReceiver = new MessageReceiver();
            var replySender = new ReplySender(_botClient);
            messageReceiver.SetCommand("/start", new StartCommand(replySender));
            messageReceiver.SetCommand("Меню", new MenuCommand(replySender));
            messageReceiver.SetCommand("Контакты", new ContactsCommand(replySender));

            var _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
                ThrowPendingUpdates = true,
            };
            var cts = new CancellationTokenSource();

            var activator = new BotActivator(messageReceiver);
            activator.Start(_botClient, _receiverOptions, cts.Token);

            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} started");
            Console.ReadLine();
        }
    }
}
