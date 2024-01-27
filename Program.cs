using BowlingConsultant.CommandManager;
using BowlingConsultant.Configuration;
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
        public static MessageReceiver MessageReceiver { get; set; }

        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Configurathion.SetProperities(config);
            var _botClient = new TelegramBotClient(Configurathion.TelegramSettings.BotToken);

            var replySender = new ReplySender(_botClient);
            var menuCommand = new MenuCommand(replySender);
            var contactsCommand = new ContactsCommand(replySender);
            MessageReceiver = new MessageReceiver();

            MessageReceiver.SetCommand("Меню", menuCommand);
            MessageReceiver.SetCommand("Контакты", contactsCommand);

            var _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
                ThrowPendingUpdates = true,
            };
            var cts = new CancellationTokenSource();

            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);
            var me = await _botClient.GetMeAsync();

            Console.WriteLine($"{me.FirstName} started");
            Console.ReadLine();
        }

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message) 
                return;

            var message = update.Message;

            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat, "Привет. Что вас интерисует?");
                return;
            }

            await botClient.SendTextMessageAsync(message.Chat, message.Text);
            await MessageReceiver.SendAnswer(message.Text, message.Chat);
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(error));
            return Task.CompletedTask;
        }
    }
}
