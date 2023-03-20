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
using static System.Net.Mime.MediaTypeNames;

namespace botserver_standard
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // OnStartup code next:

            Settings.connString = "Data Source = appDB.db";
            Settings.parsingUrl = "https://studyintomsk.ru/programs-main/?level=card-item&direction=card-item";

            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, DbWorker.dbStructureRessurection); //восстановление структуры бд при необходимости

            //чтение настроек
            using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);

            MessageBox.Show("running App.xaml.cs code. Variables setted. Press enter.");
        }

    }
}
