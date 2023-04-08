using System.Threading;
using Telegram.Bot;

namespace botserver_standard
{
    internal class TgBot
    {
        public static TelegramBotClient botClient = new(Settings.botToken); //инициализация клиента
        public static CancellationTokenSource MainBotCts = new();
    }
}
