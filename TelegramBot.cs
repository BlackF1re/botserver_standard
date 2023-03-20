using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace botserver_standard
{
    internal class TgBot
    {
        public static TelegramBotClient botClient = new(Settings.botToken); //инициализация клиента
        public static CancellationTokenSource MainBotCts = new();
    }
}
