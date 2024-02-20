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
        private readonly ITelegramBotClient _botClient;

        private Comment _actualComment {  get; set; }

        public FillingStages Stage { get; private set; }

        public CommentCreater(ITelegramBotClient botClient)
        {
            Stage = FillingStages.NotStart;
            _botClient = botClient;
        }

        public async Task FillComment(Chat chat, string messageText)
        {
            if(Stage == FillingStages.NotStart)
            {
                _actualComment = new Comment();
                Stage = FillingStages.Name;

                await _botClient.SendTextMessageAsync(chat, "Мы рады, что вы решили оставить отзыв о нас. " +
                    "Отменить отправку можно С помощью команды \"Отмена\".");
                await _botClient.SendTextMessageAsync(chat, "Введите своё имя.");
            }

            else if(Stage == FillingStages.Name) 
            {
                _actualComment.Name = messageText;
                Stage = FillingStages.Surname;

                await _botClient.SendTextMessageAsync(chat, "Теперь введите фамилию.");
            }

            else if(Stage == FillingStages.Surname)
            {
                _actualComment.Surname = messageText;
                Stage = FillingStages.PhoneNumber;

                await _botClient.SendTextMessageAsync(chat, "Для обратной связи нам" +
                    " нужно знать ваш номер телефона. Пожалуйста введите его.");
            }

            else if(Stage == FillingStages.PhoneNumber) 
            {
                try
                {
                    _actualComment.PhoneNumber = messageText;
                    Stage = FillingStages.CommentText;

                    await _botClient.SendTextMessageAsync(chat, "Напишите свой отзыв о нашем боулинг-центре.");
                }
                catch (ArgumentException e)
                {
                    await _botClient.SendTextMessageAsync(chat, "Вы ввели несуществующий номер телефона. " +
                        "Пожалуйста, введите ваш реальный.");
                }
            }

            else if(Stage == FillingStages.CommentText)
            {
                _actualComment.CommentText = messageText;
                Stage = FillingStages.NotStart;

                _actualComment.SendComment();
                _actualComment = null;

                await _botClient.SendTextMessageAsync(chat, "Отзыв успешно сохранён. Спасибо, что поделились своим мнением.");
            }
        }

        public async Task Cansel(Chat chat)
        {
            _actualComment = null;
            Stage = FillingStages.NotStart;

            await _botClient.SendTextMessageAsync(chat, "Вы отменили отправку отзыва. " +
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
