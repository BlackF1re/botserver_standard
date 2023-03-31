using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
namespace botserver_standard
{
    internal class TelegramBotStaticKeypads
    {
        public static string? levelChoose;
        public static string? universityChoose;
        public static string? directionChoose;

        public static readonly InlineKeyboardMarkup mainMenuKeypad = new(
        new[]
        {
            // first row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Выбрать программу обучения", callbackData: "programChoose"),
                InlineKeyboardButton.WithUrl(text: "Проверить знание русского языка", url: "https://studyintomsk.2i.tusur.ru/"),
            },
            // second row
            new[]
            {
                InlineKeyboardButton.WithUrl(text: "Посетить веб-сайт проекта", url: "https://studyintomsk.ru/"),
                InlineKeyboardButton.WithCallbackData(text: "Сменить язык", callbackData: "langSwitch"),
            },

        });

        public static readonly InlineKeyboardMarkup levelChoosingKeypad = new(
        // keyboard
        new[]
        {
            // first row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Бакалавриат", callbackData: "firstLevel"),
                InlineKeyboardButton.WithCallbackData(text: "Магистратура", callbackData: "secondLevel"),
                InlineKeyboardButton.WithCallbackData(text: "Специалитет", callbackData: "thirdLevel"),
            },
            // second row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "На главную", callbackData: "toHome"),
            },

        });

        public static readonly InlineKeyboardMarkup universityChoosingKeypad = new( //MUST BE PARSED!!!
        // keyboard
        new[]
        {
            // first row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "ТГАСУ", callbackData: "firstUniversity"),
                InlineKeyboardButton.WithCallbackData(text: "ТГПУ", callbackData: "secondUniversity"),
                InlineKeyboardButton.WithCallbackData(text: "ТГУ", callbackData: "thirdUniversity"),
            },
            // second row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "ТПУ", callbackData: "fourthtUniversity"),
                InlineKeyboardButton.WithCallbackData(text: "ТУСУР", callbackData: "fifthUniversity"),
                InlineKeyboardButton.WithCallbackData(text: "ФГБОУ ВО СибГМУ", callbackData: "sixthUniversity"),
            },
            //
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "На главную", callbackData: "toHome"),
            },

        });
    }
}
