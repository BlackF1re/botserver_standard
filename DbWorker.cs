using Microsoft.Data.Sqlite;
using System;

namespace botserver_standard
{
    internal class DbWorker
    {
        public static bool pwdSetResult;
        public static SqliteConnection sqliteConn = new(Settings.connString);

        //восстановление структуры БД, если файл не найден

        public static readonly string dbStructureRessurection =
            "CREATE TABLE IF NOT EXISTS Received_messages (username TEXT, is_bot INTEGER, first_name TEXT, last_name TEXT, language_code TEXT, chat_id INTEGER, message_id INTEGER, message_date TEXT, chat_type TEXT, message_content BLOB);" +
            "\r\nCREATE TABLE IF NOT EXISTS Settings (logPath TEXT, connString TEXT, botToken TEXT, pwd TEXT, pwdIsUsing TEXT, prsFilePath TEXT);" +
            "\r\nCREATE TABLE IF NOT EXISTS Cards (id INTEGER NOT NULL UNIQUE, universityName TEXT, programName TEXT, programCode TEXT, level TEXT, studyForm TEXT, duration TEXT, studyLang TEXT, curator TEXT, phoneNumber TEXT, email TEXT, cost TEXT, PRIMARY KEY(id)) WITHOUT ROWID" +
            "\r\nCREATE TABLE IF NOT EXISTS Session_duration (startup_time TEXT, shutdown_time TEXT, total_uptime TEXT);" +
            "\r\nCREATE TABLE IF NOT EXISTS Universities (id INTEGER NOT NULL, universityName TEXT);" +
            "\r\nCREATE TABLE IF NOT EXISTS Directions (id INTEGER NOT NULL, directionName TEXT);" +
            "\r\nCREATE TABLE IF NOT EXISTS Programs (id INTEGER NOT NULL, programName TEXT);" +
            "\r\nCREATE TABLE IF NOT EXISTS Parsing_history (timestamp TEXT, parsingStart TEXT, parsingEnd TEXT, parsingResult INTEGER);" +
            "\r\nCREATE TABLE IF NOT EXISTS Passwords_history (timestamp TEXT, oldPassword TEXT);";

        public static string readTokenFromDb = "SELECT botToken FROM Settings";


        public static readonly string received_messagesConsoleOutput = "SELECT * FROM Received_messages";

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

        public static SqliteDataReader SettingsReader(string queryText, SqliteConnection sqliteConn) //оновление настроек из бд
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
                    Settings.fileLoggerPath = Convert.ToString(reader["fileLoggerPath"]);
                    Settings.callbackLoggerPath = Convert.ToString(reader["callbackLoggerPath"]);
                    Settings.botToken = Convert.ToString(reader["botToken"]);
                    Settings.pwd = Convert.ToString(reader["pwd"]);
                    Settings.pwdIsUsing = Convert.ToBoolean(reader["pwdIsUsing"]);
                    Settings.datagridExportPath = Convert.ToString(reader["datagridExportPath"]);
                    Settings.parsingUrl = Convert.ToString(reader["parsingUrl"]);

                }
            }
            sqliteConn.Close();
            return reader;
        }

    }
}
