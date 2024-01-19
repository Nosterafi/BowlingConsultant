using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingConsultant
{
    public static class Configurathion
    {
        public static TelegramSettings TelegramSettings { get; set; }

        public static void SetProperities(IConfiguration configurathion)
        {
            TelegramSettings = GetProperities<TelegramSettings>(configurathion, "TelegramSettings");
        }

        public static T GetProperities<T>(IConfiguration configurathion, string sectionName)
        {
            return configurathion.GetSection(sectionName).Get<T>()
                ?? throw new InvalidOperationException($"Not found section {nameof(T)} in configuration.");
        }
    }
}
