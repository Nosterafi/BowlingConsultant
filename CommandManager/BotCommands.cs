using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BowlingConsultant.CommandManager
{
    public interface IBotCommands
    {
        public Task Execute(Chat chat);
    }

    public class MenuCommand : IBotCommands
    {
        public ReplySender Invocker {  get; set; }

        public MenuCommand(ReplySender invocker)
        {
            Invocker = invocker;
        }

        public async Task Execute(Chat chat) 
        {
            await Invocker.SendMenu(chat);
        }
    }

    public class ContactsCommand : IBotCommands 
    {
        public ReplySender Invocker { get; set; }

        public ContactsCommand(ReplySender invocker)
        {
            Invocker = invocker;
        }

        public async Task Execute(Chat chat)
        {
            await Invocker.SendContacts(chat);
        }
    }
}
