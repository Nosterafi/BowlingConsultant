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
        private static ITelegramBotClient _botClient;

        private static ReceiverOptions _receiverOptions;

        static async Task Main(string[] args)
        {
            var token = "6623750163:AAFk0OFe8QIwpqVEPTVCxbJWWL1u9vOsl_Y";
            var _botClient = new TelegramBotClient(token);
            var _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]{ UpdateType.Message, UpdateType.CallbackQuery },
                ThrowPendingUpdates = true,
            };
            using var cts = new CancellationTokenSource();
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);
            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} запущен!");
            await Task.Delay(-1);
        }

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var message = update.Message;
                await Answers.AnswersToMessages[message.Text](botClient, message.Chat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

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
