using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BowlingConsultant
{
    public static class Answers
    {
        public readonly static Dictionary<string, Func<ITelegramBotClient, Chat, Task>> AnswersToMessages;

        static Answers()
        {
            AnswersToMessages = new Dictionary<string, Func<ITelegramBotClient, Chat, Task>>
            {
                {"/start", AnswerToStart}
            };
        }

        private async static Task AnswerToStart(ITelegramBotClient botClient, Chat chat)
        {
            var me = await botClient.GetMeAsync();
            var name = me.FirstName;
            var buttons = new ReplyKeyboardMarkup(
            new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Меню"),
                    new KeyboardButton("Режим работы")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Что у нас есть?"),
                    new KeyboardButton("Наши контакты")

                }
            })
            { ResizeKeyboard = true };
            await botClient.SendTextMessageAsync(chat.Id, $"Привет, я бот {name}.\nЧто вас интерисует", replyMarkup: buttons);
        }
    }
}