using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
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
        public static SqliteConnection sqliteConn = new(Settings.connString);

        //восстановление структуры БД, если файл не найден
        public static readonly string dbStructureRessurection = 
            "CREATE TABLE IF NOT EXISTS Received_messages (username TEXT, is_bot INTEGER, first_name TEXT, last_name TEXT, language_code TEXT, chat_id INTEGER, message_id INTEGER, message_date TEXT, chat_type TEXT, message_content BLOB);" +
            "CREATE TABLE IF NOT EXISTS Settings (logPath TEXT, connString TEXT, botToken TEXT, pwd TEXT, pwdIsUsing TEXT, prsFilePath TEXT);" +
            "CREATE TABLE Cards (id INTEGER NOT NULL UNIQUE, universityName TEXT, programName TEXT, level TEXT, studyForm TEXT, duration TEXT, studyLang TEXT, curator TEXT, phoneNumber TEXT, email TEXT, cost TEXT, PRIMARY KEY(\"id\")) WITHOUT ROWID";

        public static string readTokenFromDb = "SELECT botToken FROM Settings";


        public static readonly string received_messagesConsoleOutput = "SELECT * FROM Received_messages";

        //public static readonly string parsedDataToDb = "INSERT INTO parsedData VALUES";

        public static readonly string readSettings = "SELECT * FROM Settings";
        public static int DbQuerySilentSender(SqliteConnection sqliteConn, string queryText) //no feedback
        {
            sqliteConn.Open();
            SqliteCommand command = new()
            {
                Connection = sqliteConn, //соединение для выполнения запроса
                CommandText = queryText //текст запроса
            };
            int rowsChanged = command.ExecuteNonQuery(); //выполнение запроса и возврат количества измененных строк
            sqliteConn.Close(); //закрытие соединения
            return rowsChanged;
        }

        public static SqliteDataReader SettingsReader(string queryText, SqliteConnection sqliteConn)
        {
            sqliteConn.Open(); //открытие соединения
            SqliteCommand command = new() //инициализация экземпляра SqliteCommand
            {
                Connection = sqliteConn, //соединение для выполнения запроса
                CommandText = queryText //текст запроса
            };
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows) // если есть строки
            {
                while (reader.Read())   // построчное чтение данных
                {
                    Settings.logPath = Convert.ToString(reader["logPath"]);
                    Settings.connString = Convert.ToString(reader["connString"]);
                    Settings.botToken = Convert.ToString(reader["botToken"]);

                    var checkDBNull = reader["pwd"];
                    if (checkDBNull == DBNull.Value) Settings.pwd = null; //System.InvalidCastException: "Object cannot be cast from DBNull to other types."
                    else Settings.pwdIsSetted = Convert.ToBoolean(reader["pwdIsUsing"]);

                    Settings.pwdIsUsing = Convert.ToBoolean(reader["pwdIsUsing"]);
                    Settings.prsFilePath = Convert.ToString(reader["prsFilePath"]);
                    Settings.parsingUrl = Convert.ToString(reader["parsingUrl"]);

                }
            }
            sqliteConn.Close();
            return reader;
        }

    }
}
