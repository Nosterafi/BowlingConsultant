using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingConsultant
{
    public class Comment
    {
        private DateTime _fillingDateTime;

        public string Name { private get; set; }

        public string Surname { private get; set; }

        private string _phoneNumber;

        public string PhoneNumber
        {
            set
            {
                if(value == null) 
                    throw new ArgumentNullException("PhonNumber is null");

                if (value.Length != 11 || value[0] != '8')
                    throw new ArgumentException("Incorrect number format");

                _phoneNumber = value;
            }
        }

        public string CommentText { private get; set; }

        public void SendComment()//Метод для отправки отзыва в БД
        {
            if (!IsFilled())
                throw new InvalidOperationException("Comment is not finished");
            _fillingDateTime = DateTime.Now;
            //...
        }

        private bool IsFilled()
        { return Name != null && Surname != null && _phoneNumber != null && CommentText != null; }
    }
}
