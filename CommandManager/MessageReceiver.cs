﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BowlingConsultant
{
    public class MessageReceiver
    {
        private Dictionary<string, ICommand> Commands { get; set; } =
            new Dictionary<string, ICommand>();

        public void SetCommand(string messageText, ICommand command)
        {
            Commands[messageText] = command;
        }

        public async Task SendAnswer(string messageText, Chat chat)
        {
            if(Commands.ContainsKey(messageText))
                await Commands[messageText].Execute(chat, messageText);

            else await Commands["Непредусмотренная команда"].Execute(chat, messageText);
        }

        public async Task FillComment(string messageText, Chat chat)
        {
            if (messageText.Equals("Отмена"))
            {
                Commands["Отмена"].Execute(chat, messageText);
                return;
            }

            await Commands["Написать отзыв"].Execute(chat, messageText);
        }
    }
}
