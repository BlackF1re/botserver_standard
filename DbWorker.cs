using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace botserver_standard
{
    internal class DbWorker
    {
        public static readonly string dbStructureRessurection = "CREATE TABLE IF NOT EXISTS Received_messages (username TEXT, is_bot INTEGER, first_name TEXT, last_name TEXT, language_code TEXT, chat_id INTEGER, message_id INTEGER, message_date TEXT, chat_type TEXT, message_content BLOB);" +
            " CREATE TABLE IF NOT EXISTS Settings (logPath TEXT, connString TEXT, botToken TEXT, pwd TEXT, pwdIsUsing TEXT);";

        //public static void DbStructureResurrector(SqliteConnection sqliteConn, string queryText)
        //{
        //    sqliteConn.Open();
        //    SqliteCommand command = new();
        //    {
        //        command.Connection = sqliteConn;
        //        command.CommandText = queryText;
        //    }
        //    command.ExecuteNonQuery();
        //}
        public static void DbQuerySilentSender(SqliteConnection sqliteConn, string queryText)
        {
            sqliteConn.Open(); //открытие соединения
            SqliteCommand command = new() //инициализация экземпляра SqliteCommand
            {
                Connection = sqliteConn, //соединение для выполнения запроса
                CommandText = queryText //текст запроса
            };
            command.ExecuteNonQuery(); //выполнение запроса и возврат количества измененных строк
            sqliteConn.Close(); //закрытие соединения
        }
    }
}
