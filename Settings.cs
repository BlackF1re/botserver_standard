using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace botserver_standard
{
    internal class Settings //публичные поля настроек для последующей их записи в бд
    {
        public static string? fileLoggerPath = null; // путь к логу (добавить изменение пути лога в интерфейсе?)
        public static string? callbackLoggerPath = null;
        public static string? connString = null; //путь к бд. setted in app!
        public static string? botToken = null; //токен бота
        public static string? pwd; //пароль на запуск. Может быть отключен, см. ниже
        public static bool pwdIsUsing = false; //пароль используется(чекбокс)? 
        public static string? datagridExportPath = null;
        public static string? parsingUrl = null; //URL страницы, подлежащей парсингу

    }
}