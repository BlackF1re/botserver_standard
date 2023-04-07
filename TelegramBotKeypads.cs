using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace botserver_standard
{
    internal class TelegramBotKeypads
    {
        public static string? choisedLevel;
        public static string? choisedUniversity;
        public static string? choisedProgram;

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
                //InlineKeyboardButton.WithCallbackData(text: "Сменить язык", callbackData: "langSwitch"),
            },

        });

        public static readonly InlineKeyboardMarkup levelChoosingKeypad = new(
        // keyboard
        new[]
        {
            // first row
            new[]
            {
                //InlineKeyboardButton.WithCallbackData(text: "Бакалавриат", callbackData: "first_level"),
                //InlineKeyboardButton.WithCallbackData(text: "Магистратура", callbackData: "second_level"),
                //InlineKeyboardButton.WithCallbackData(text: "Специалитет", callbackData: "third_level"),

                InlineKeyboardButton.WithCallbackData(text: "Бакалавриат", callbackData: "бакалавриат_level"),
                InlineKeyboardButton.WithCallbackData(text: "Магистратура", callbackData: "магистратура_level"),
                InlineKeyboardButton.WithCallbackData(text: "Специалитет", callbackData: "специалитет_level"),
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
                InlineKeyboardButton.WithCallbackData(text: "ТГАСУ", callbackData: "ТГАСУ_university"),
                InlineKeyboardButton.WithCallbackData(text: "ТГПУ", callbackData: "ТГПУ_university"),
                InlineKeyboardButton.WithCallbackData(text: "ТГУ", callbackData: "ТГУ_university"),
            },
            // second row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "ТПУ", callbackData: "ТПУ_university"),
                InlineKeyboardButton.WithCallbackData(text: "ТУСУР", callbackData: "ТУСУР_university"),
                //InlineKeyboardButton.WithCallbackData(text: "ФГБОУ ВО СибГМУ", callbackData: "sixth_university"),
            },
            // third row
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "На главную", callbackData: "toHome"),
            },

        });

        /// <summary>
        /// btnGeneration
        /// </summary>


    }
}
