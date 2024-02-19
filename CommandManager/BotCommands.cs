using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BowlingConsultant
{
    public interface ICommand
    {
        public Task Execute(Chat chat);
    }

    public abstract class Command
    {
        protected ReplySender Invocker { get; set; }

        public Command(ReplySender invocker)
        {
            Invocker = invocker;
        }
    }

    public class StartCommand : Command, ICommand
    {
        public StartCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat)
        {
            await Invocker.SendStart(chat);
        }
    }

    public class MenuCommand : Command, ICommand
    {
        public MenuCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat)
        {
            await Invocker.SendMenu(chat);
        }
    }

    public class ContactsCommand : Command, ICommand
    {
        public ContactsCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat)
        {
            await Invocker.SendContacts(chat);
        }
    }

    public class InvalidCommand : Command, ICommand
    {
        public InvalidCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat)
        {
            await Invocker.SendUnintendedMessage(chat);
        }
    }
}
