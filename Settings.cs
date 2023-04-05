using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botserver_standard
{
    internal class Settings //публичные поля настроек для последующей их записи в бд
    {
        public static string? logPath = null; // путь к логу (добавить изменение пути лога в интерфейсе?)
        public static string? connString = null; //путь к бд
        public static string? botToken = null; //токен бота
        public static string? pwd; //пароль на запуск. Может быть отключен, см. ниже
        //public static bool pwdIsSetted = false; //пароль установлен? 
        public static bool pwdIsUsing = false; //пароль используется(чекбокс)? 
        public static string? prsFilePath = null; //.prs = Parsing ReSult. Путь к файлу спарсенных данных
        public static string? parsingUrl = null; //URL страницы, подлежащей парсингу

    }
}
