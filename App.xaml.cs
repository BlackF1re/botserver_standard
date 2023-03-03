using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace botserver_standard
{
    public partial class App : Application
    {
        #pragma warning disable CA2211
        public static string? logPath = null; // путь к логу (добавить изменение пути лога в интерфейсе?)
        public static string? connString = null; //путь к бд
        public static string? botToken = null; //токен бота
        public static string? pwd = null;
        //private readonly string getSettingsQuery = ""; //извлечение настроек из бд на старте бота

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // OnStartup code next:
            SqliteConnection onStartupConnection = new(connString);

            DbWorker.DbQuerySilentSender(onStartupConnection, DbWorker.dbStructureRessurection);

            MessageBox.Show("running App.xaml.cs code. Variables setted. Press enter.");
            connString = "Data Source = appDB.db";

            logPath = "TGBot_server_log.txt";
            connString = "Data Source = appDB.db";
            botToken = "5969998133:AAF3nNDlYNfryOulNHKtsxlhuGo_roxXYXI";
        }

    }
}
