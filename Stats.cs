using System;

namespace botserver_standard
{
    public static class Stats
    {
        static DateTime startupTime;
        static DateTime shutdownTime;

        public static void StartupTimeFixator()
        {            
            startupTime = DateTime.Now;
        }

        public static void ShutdownTimeFixator()
        {
            shutdownTime = DateTime.Now;
        }

        public static void UpTimeWriter()
        {
            TimeSpan upTime = shutdownTime - startupTime;
            string query = $"INSERT INTO Session_duration(startup_time, shutdown_time, total_uptime)" +
                            $"VALUES('{startupTime}', '{shutdownTime}', '{upTime}');";
            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, query);
        }
    }
}
