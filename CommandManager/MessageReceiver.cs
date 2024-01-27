using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BowlingConsultant.CommandManager
{
    public class MessageReceiver
    {
        public Dictionary<string, IBotCommands> Commands {  get; private set; }

        public MessageReceiver() 
        {
            Commands = new Dictionary<string, IBotCommands>();
        }

        public void SetCommand(string messageText, IBotCommands command)
        {
            Commands[messageText] = command;
        }

        public async Task SendAnswer(string messageText, Chat chat)
        {
            if(Commands.ContainsKey(messageText))
                await Commands[messageText].Execute(chat);
        }
    }
}
