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
        //Словарь с методами, отправляющими ответы пльзователю.
        //Ключ - текст сообщения, отправленного боту. Значение - метод для ответа на конкретное сообщение.
        public readonly static Dictionary<string, Func<ITelegramBotClient, Chat, Task>> AnswersToMessages;

        //Клавиатура, с помощью которой пользователь получает информацию.
        public readonly static ReplyKeyboardMarkup KeyBoard;

        //Статический конструктор.
        static Answers()
        {
            AnswersToMessages = new Dictionary<string, Func<ITelegramBotClient, Chat, Task>>
            {
                {"/start", AnswerToStart},
                {"Меню" , AnswerToMenu }
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
                    new KeyboardButton("Что у нас есть?"),
                    new KeyboardButton("Наши контакты")

                }
            })
            { ResizeKeyboard = true };
        }

        //Реакция на стартовое сообщение.
        private async static Task AnswerToStart(ITelegramBotClient botClient, Chat chat)
        {
            var me = await botClient.GetMeAsync();
            var name = me.FirstName;
            await botClient.SendTextMessageAsync(chat.Id, $"Привет, я бот {name}.\nЧто вас интерисует", replyMarkup: KeyBoard);
        }

        //Реакция на просьбу пользователя показать меню.
        private async static Task AnswerToMenu(ITelegramBotClient botClient, Chat chat)
        {
            //Данные о меню боулинг-центра храниться в файле Menu.txt. 
            //Данный файл расположен в папке User data, находящейся в директории проекта.
            var text = System.IO.File.ReadAllText("D:\\BowlingConsultant\\BowlingConsultant\\User data\\Menu.txt");
            await botClient.SendTextMessageAsync(chat.Id, text, replyMarkup: KeyBoard);
        }
    }
}