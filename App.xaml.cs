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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // OnStartup code next:

            Settings.logPath = "TGBot_server_log.txt";
            Settings.connString = "Data Source = appDB.db";
            Settings.botToken = "5969998133:AAF3nNDlYNfryOulNHKtsxlhuGo_roxXYXI";
            //Settings.pwd = "1488";
            //Settings.pwdIsSetted = false;

            Settings.parsingUrl = "https://studyintomsk.ru/programs-main/?level=card-item&direction=card-item";
            Settings.prsFilePath = "parsing_result.prs";

            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, DbWorker.dbStructureRessurection); //восстановление структуры бд при необходимости

            MessageBox.Show("running App.xaml.cs code. Variables setted. Press enter.");

        }

    }
}
