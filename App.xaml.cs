using Microsoft.Data.Sqlite;
using System.Data;
using System.Windows;

namespace botserver_standard
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // OnStartup code next:

            Stats.StartupTimeFixator();
            Settings.connString = "Data Source = appDB.db";
            Settings.parsingUrl = "https://studyintomsk.ru/programs-main/?level=card-item&direction=card-item";

            //восстановление структуры бд при необходимости
            try
            {
                DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, DbWorker.dbStructureRessurection);
            }

            catch
            {
                return;
            }

            finally
            {
                //чтение настроек
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);
                //MessageBox.Show("running App.xaml.cs code. Variables setted. Press enter.");

            }

        }

    }
}
