using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using BowlingConsultant.Configuration;

namespace BowlingConsultant
{
    public class BotWorker
    {
        private readonly string botToken;

        public readonly ITelegramBotClient BotClient;

        private  MessageReceiver messageReceiver;

        private  ReplySender replySender;

        private  CancellationTokenSource cts;

        private ReceiverOptions receiverOptions;

        private CommentCreater commentCreater;

        public BotWorker()
        {
            botToken = Configurathion.TelegramSettings.BotToken;

            if(botToken == null && botToken == string.Empty)
                throw new NullReferenceException("Invalid token");

            BotClient = new TelegramBotClient(botToken);

            messageReceiver = new MessageReceiver();
            replySender = new ReplySender(BotClient);
            commentCreater = new CommentCreater(BotClient);
            messageReceiver.SetCommand("/start", new StartCommand(replySender));
            messageReceiver.SetCommand("Меню", new MenuCommand(replySender));
            messageReceiver.SetCommand("Контакты", new ContactsCommand(replySender));
            messageReceiver.SetCommand("Написать отзыв", new NewCommentCommand(commentCreater));
            messageReceiver.SetCommand("Заполнить отзыв", new FillCommentCommand(commentCreater));
            messageReceiver.SetCommand("Отмена", new CanselCommentCommand(commentCreater));

            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
                ThrowPendingUpdates = true,
            };
            cts = new CancellationTokenSource();
        }

        public void Start()
        {
            BotClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);
        }

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update != null && update.Type != UpdateType.Message)
                return;

            var message = update.Message;

            await messageReceiver.SendAnswer(message.Text, message.Chat);
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(error));
            return Task.CompletedTask;
        }
    }
}
