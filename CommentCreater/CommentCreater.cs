using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BowlingConsultant
{
    public class CommentCreater
    {
        private readonly ITelegramBotClient botClient;

        private Comment actualComment {  get; set; }

        public FillingStages Stage { get; private set; }

        public CommentCreater(ITelegramBotClient botClient)
        {
            Stage = FillingStages.NotStart;
            this.botClient = botClient;
        }

        public async Task FillComment(Chat chat, string messageText)
        {

            if (Stage == FillingStages.Name) 
            {
                await FillName(chat, messageText);

                return;
            }

            if(Stage == FillingStages.Surname)
            {
                await FillSurname(chat, messageText);
                
                return;
            }

            if(Stage == FillingStages.PhoneNumber) 
            {
                await FillPhoneNumber(chat, messageText);

                return;
            }

            if(Stage == FillingStages.CommentText)
            { 
                await FillCommentText(chat, messageText);

                return;
            }
        }

        public async Task StartFilling(Chat chat)
        {
            if(Stage != FillingStages.NotStart)
            {
                await botClient.SendTextMessageAsync(chat, "Вы уже начали писать отзыв." +
                    " Вы можете закончить его или же начать сначала с помощью комманды \"Отмена\".");

                return;
            }

            actualComment = new Comment();
            Stage = FillingStages.Name;

            await botClient.SendTextMessageAsync(chat, "Мы рады, что вы решили оставить отзыв о нас. " +
              "Отменить отправку можно c помощью команды \"Отмена\".");
            await botClient.SendTextMessageAsync(chat, "Введите своё имя.");
        }

        private async Task FillName(Chat chat, string name)
        {
            actualComment.Name = name;
            Stage = FillingStages.Surname;

            await botClient.SendTextMessageAsync(chat, "Теперь введите фамилию.");
        }

        private async Task FillSurname(Chat chat, string surname)
        {
            actualComment.Surname = surname;
            Stage = FillingStages.PhoneNumber;

            await botClient.SendTextMessageAsync(chat, "Для обратной связи нам" +
                " нужно знать ваш номер телефона. Пожалуйста введите его.");
        }

        private async Task FillPhoneNumber(Chat chat, string phoneNumber)
        {
            try
            {
                actualComment.PhoneNumber = phoneNumber;
                Stage = FillingStages.CommentText;

                await botClient.SendTextMessageAsync(chat, "Напишите свой отзыв о нашем боулинг-центре.");

                return;
            }
            catch (ArgumentException e)
            {
                await botClient.SendTextMessageAsync(chat, "Вы ввели несуществующий номер телефона. " +
                    "Пожалуйста, введите ваш реальный.");
            }
        }

        private async Task FillCommentText(Chat chat, string commentText)
        {
            actualComment.CommentText = commentText;
            Stage = FillingStages.NotStart;

            actualComment = null;

            await botClient.SendTextMessageAsync(chat, "Отзыв успешно сохранён. Спасибо, что поделились своим мнением.");
        }

        public async Task Cansel(Chat chat)
        {
            actualComment = null;
            Stage = FillingStages.NotStart;

            await botClient.SendTextMessageAsync(chat, "Вы отменили отправку отзыва. " +
                "Если хотите начать заново, введите \"Написать отзыв\".");
        }
    }

    public enum FillingStages
    {
        NotStart,
        Name,
        Surname,
        PhoneNumber,
        CommentText
    }
}
