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
        public Task Execute(Chat chat, string messageText);
    }

    public abstract class ReplySenderCommand
    {
        protected ReplySender Invocker { get; set; }

        public ReplySenderCommand(ReplySender invocker)
        {
            Invocker = invocker;
        }
    }

    public abstract class CommentCreaterCommand
    {
        protected CommentCreater Invocker { get; set; }

        public CommentCreaterCommand(CommentCreater invocker)
        {
            Invocker = invocker;
        }
    }
    
    public class StartCommand : ReplySenderCommand, ICommand
    {
        public StartCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat, string messageText)
        {
            await Invocker.SendStart(chat);
        }
    }

    public class MenuCommand : ReplySenderCommand, ICommand
    {
        public MenuCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat, string messageText)
        {
            await Invocker.SendMenu(chat);
        }
    }

    public class ContactsCommand : ReplySenderCommand, ICommand
    {
        public ContactsCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat, string messageText)
        {
            await Invocker.SendContacts(chat);
        }
    }

    public class InvalidCommand : ReplySenderCommand, ICommand
    {
        public InvalidCommand(ReplySender invocker) : base(invocker) { }

        public async Task Execute(Chat chat, string messageText)
        {
            await Invocker.SendUnintendedMessage(chat);
        }
    }

    public class FillCommentCommand : CommentCreaterCommand, ICommand
    {
        public FillCommentCommand(CommentCreater invocker) : base(invocker) { }

        public async Task Execute(Chat chat, string messageText)
        {
            Invocker.FillComment(chat, messageText);
        }
    }

    public class CanselCommentCommand : CommentCreaterCommand, ICommand
    {
        public CanselCommentCommand(CommentCreater invocker) : base(invocker) { }

        public async Task Execute(Chat chat, string messageText)
        {
            Invocker.Cansel(chat);
        }
    }


}
