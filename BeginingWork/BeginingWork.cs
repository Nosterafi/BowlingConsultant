using BowlingConsultant.CommandManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using System.Runtime.CompilerServices;

namespace BowlingConsultant.BeginingWork
{
    public class BotActivator
    {
        private MessageReceiver MessageReceiver { get; set; }
        
        public BotActivator(MessageReceiver messageReceiver)
        { 
            MessageReceiver = messageReceiver; 
        }
        public void Start(ITelegramBotClient botClient, ReceiverOptions receiverOptions, CancellationToken token)
        {
            botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, token);
        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update != null && update.Type != UpdateType.Message)
                return;

            var message = update.Message;
            await MessageReceiver.SendAnswer(message.Text, message.Chat);
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(error));
            return Task.CompletedTask;
        }
    }
}
