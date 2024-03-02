using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingConsultant
{
    public class Comment
    {
        public string Name { private get; set; } = string.Empty;

        public string Surname { private get; set; } = string.Empty;

        private string phoneNumber = string.Empty;

        public string PhoneNumber
        {
            set
            {
                if(value == null) 
                    throw new ArgumentNullException("PhonNumber is null");

                if (value.Length != 11 || value[0] != '8')
                    throw new ArgumentException("Incorrect number format");

                phoneNumber = value;
            }
        }

        public string CommentText { private get; set; }
    }
}
