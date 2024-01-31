using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.IO.File;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BowlingConsultant.CommandManager
{
    public class ReplySender
    {
        private ITelegramBotClient BotClient { get; set; }

        public ReplySender(ITelegramBotClient botClient)
        {
            BotClient = botClient;
        }

        public async Task SendStart(Chat chat)
        {
            await BotClient.SendTextMessageAsync(chat, "Привет. Чем я могу помочь?");
        }

        public async Task SendMenu(Chat chat)
        {
            await BotClient.SendTextMessageAsync(chat, ReadAllText("User data\\Menu.txt"));
        }

        public async Task SendContacts(Chat chat)
        {
            await BotClient.SendTextMessageAsync(chat, ReadAllText("User data\\Contacts.txt"));
        }
    }
}
