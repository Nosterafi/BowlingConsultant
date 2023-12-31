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
    //Класс, в котором реализована логика ответов на сообщения пользователя.
    public static class Answers
    {
        //Словарь, с путями к файлам с информацией для пользователей.
        //Ключи - тексты сообщений, для ответов на которые нужна информация
        //из конкретного файла.
        public readonly static Dictionary<string, string> PathsToAnswersInfo;

        //Клавиатура, с помощью которой пользователь общается с ботом.
        public readonly static ReplyKeyboardMarkup KeyBoard;

        //Статический конструктор.
        static Answers()
        {
            PathsToAnswersInfo = new Dictionary<string, string>
            {
                {"Меню" , "User data\\Menu.txt" },
                {"Режим работы", "User data\\Shedule.txt" },
                {"Что у вас есть?", "User data\\Description.txt"},
                {"Как с вами связаться?", "User data\\Contacts.txt"}

            };
            KeyBoard = new ReplyKeyboardMarkup(
            new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Меню"),
                    new KeyboardButton("Режим работы")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Что у вас есть?"),
                    new KeyboardButton("Как с вами связаться?")

                }
            })
            { ResizeKeyboard = true };
        }

        //Реакция на стартовое сообщение.
        public async static Task AnswerToStart(ITelegramBotClient botClient, Chat chat)
        {
            var me = await botClient.GetMeAsync();
            var name = me.FirstName;
            await botClient.SendTextMessageAsync(chat.Id, $"Привет, я бот {name}.\nЧто вас интерисует", replyMarkup: KeyBoard);
        }

        //Реакция на все остальные сообщения. Текст ответа зависит от 
        //текста в файле, к которому ведёт путь pathToInfo.
        public async static Task SendAnswer(ITelegramBotClient botClient, Chat chat, string pathToInfo)
        {
            var text= System.IO.File.ReadAllText(pathToInfo);
            await botClient.SendTextMessageAsync(chat.Id, $"{text}\n\nКакую информацию вы ещё хотели бы получить?", replyMarkup: KeyBoard);
        }
    }
}