//using Microsoft.Data.Sqlite;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;

//namespace botserver_standard
//{
//    public partial class MainWindow : Window
//    {
//        public static List<Card> cardsView = new();

//        static int id;
//        static string? universityName;
//        static string? programName;
//        static string? level;
//        static string? programCode;
//        static string? studyForm;
//        static string? duration;
//        static string? studyLang;
//        static string? curator;
//        static string? phoneNumber;
//        static string? email;
//        static string? cost;

//        public static async Task<Task> CardsViewReader(SqliteConnection sqliteConn)
//        {

//            string queryText = "SELECT * FROM Cards";

//            sqliteConn.Open(); //открытие соединения
//            SqliteCommand command = new() //инициализация экземпляра SqliteCommand
//            {
//                Connection = sqliteConn, //соединение для выполнения запроса
//                CommandText = queryText //текст запроса
//            };
//            SqliteDataReader reader = command.ExecuteReader();

//            if (reader.HasRows) // если есть строки
//            {
//                while (reader.Read())   // построчное чтение данных
//                {
//                    //public Card(int id, string universityName, string programName, string level, string studyForm, string programCode, string duration, string studyLang, string curator, string phoneNumber, string email, string cost)

//                    id = Convert.ToInt32(reader["Id"]);
//                    universityName = Convert.ToString(reader["universityName"]);
//                    programName = Convert.ToString(reader["programName"]);
//                    programCode = Convert.ToString(reader["programCode"]);
//                    level = Convert.ToString(reader["level"]);
//                    studyForm = Convert.ToString(reader["studyForm"]);
//                    duration = Convert.ToString(reader["duration"]);
//                    studyLang = Convert.ToString(reader["studyLang"]);
//                    curator = Convert.ToString(reader["curator"]);
//                    phoneNumber = Convert.ToString(reader["phoneNumber"]);
//                    email = Convert.ToString(reader["email"]);
//                    cost = Convert.ToString(reader["cost"]);
                    
//                    cardsView.Add(new Card(id, universityName, programName, programCode, level, studyForm, duration, studyLang, curator, phoneNumber, email, cost));
//                }
//            }
//            sqliteConn.Close();
//            return Task.CompletedTask;
//        }
//    }
//}
