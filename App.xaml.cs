using Microsoft.Data.Sqlite;
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
            }

        }

    }
}
