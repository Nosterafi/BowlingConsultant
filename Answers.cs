using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BowlingConsultant
{
    public static class Answers
    {
        public readonly static InlineKeyboardMarkup MainKeyBoard;

        public static Dictionary<string, Func<ITelegramBotClient, Chat, Task>> BotAnswers { get; set; }

        static Answers()
        {
            BotAnswers = new Dictionary<string, Func<ITelegramBotClient, Chat, Task>>()
            {
                {"Menu" ,  AnswerToMenu },
                {"Shedule", AnswerToShedule },
                {"Description", AnswerToDescription },
                {"Contacts", AnswerToContacts }
            };
            MainKeyBoard = new InlineKeyboardMarkup(
            new InlineKeyboardButton[][]
            {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Меню", "Menu"),
                        InlineKeyboardButton.WithCallbackData("Режим работы", "Shedule")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Что у вас есть?", "Description"),
                        InlineKeyboardButton.WithCallbackData("Наши контакты", "Contacts")
                    }
            });
        }

        public async static Task AnswerToStart(ITelegramBotClient botClient, Chat chat)
        {
            var me = await botClient.GetMeAsync();
            var name = me.FirstName;
            await botClient.SendTextMessageAsync(chat.Id, $"Привет, я бот {name}.\nЧто вас интерисует", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToMenu(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Меню");
            var text = System.IO.File.ReadAllText("User data\\Menu.txt");
            await botClient.SendTextMessageAsync(chat.Id, $"{text}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToShedule(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Режим работы");
            var text = System.IO.File.ReadAllText("User data\\Shedule.txt");
            await botClient.SendTextMessageAsync(chat.Id, $"{text}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToDescription(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Что у вас есть?");
            var text = System.IO.File.ReadAllText("User data\\Description.txt");
            await botClient.SendTextMessageAsync(chat.Id, $"{text}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }

        public async static Task AnswerToContacts(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendTextMessageAsync(chat.Id, "Наши контакты");
            var text = System.IO.File.ReadAllText("User data\\Contacts.txt");
            await botClient.SendTextMessageAsync(chat.Id, $"{text}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: MainKeyBoard);
        }
    }
}