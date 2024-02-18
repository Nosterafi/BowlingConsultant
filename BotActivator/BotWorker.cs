using BowlingConsultant.CommandManager;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using BowlingConsultant.Configuration;

namespace BowlingConsultant.BotWorker
{
    public class BotWorker
    {
        private readonly string _botToken;

        public readonly ITelegramBotClient BotClient;

        private  MessageReceiver _messageReceiver;

        private  ReplySender _replySender;

        private  CancellationTokenSource _cts;

        private ReceiverOptions _receiverOptions;

        public BotWorker()
        {
            _botToken = Configurathion.TelegramSettings.BotToken;

            if(_botToken == null && _botToken == string.Empty)
                throw new NullReferenceException("Invalid token");

            BotClient = new TelegramBotClient(_botToken);

            _messageReceiver = new MessageReceiver();
            _replySender = new ReplySender(BotClient);
            _messageReceiver.SetCommand("/start", new StartCommand(_replySender));
            _messageReceiver.SetCommand("Меню", new MenuCommand(_replySender));
            _messageReceiver.SetCommand("Контакты", new ContactsCommand(_replySender));
            _messageReceiver.SetCommand("Непредусмотренная команда", new InvalidCommand(_replySender));

            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
                ThrowPendingUpdates = true,
            };
            _cts = new CancellationTokenSource();
        }

        public void Start()
        {
            BotClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, _cts.Token);
        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update != null && update.Type != UpdateType.Message)
                return;

            var message = update.Message;
            await _messageReceiver.SendAnswer(message.Text, message.Chat);
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(error));
            return Task.CompletedTask;
        }
    }
}
