using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BowlingConsultant
{
    class Program
    {
        //Точка начала выполнения программы.
        static async Task Main(string[] args)
        {
            var token = "6623750163:AAFk0OFe8QIwpqVEPTVCxbJWWL1u9vOsl_Y";
            var _botClient = new TelegramBotClient(token);
            var _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]{ UpdateType.Message },
                ThrowPendingUpdates = true,
            };
            using var cts = new CancellationTokenSource();
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);
            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} started");
            await Task.Delay(-1);
        }

        //Обработчик сообщений пользователей.
        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var message = update.Message;
                var chat=message.Chat;
                //Вызов метода для отправки ответа пользователю.
                //Для удобства методы храняться в словаре Ancwers.AnswersToMessages.
                if (message.Text != "/start") await botClient.SendTextMessageAsync(chat.Id, message.Text);
                await Answers.AnswersToMessages[message.Text](botClient, chat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //Обработчик ошибок, связанных с Bot API.
        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
