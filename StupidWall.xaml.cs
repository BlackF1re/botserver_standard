using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace botserver_standard
{
    /// <summary>
    /// Логика взаимодействия для StupidWall.xaml
    /// </summary>
    public partial class StupidWall : Window
    {
        public StupidWall()
        {
            InitializeComponent();
        }

        string? pwd; 
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM Settings";
            using (var connection = new SqliteConnection(Settings.connString)) //читаем актуальный пароль из бд
            {
                connection.Open();

                SqliteCommand command = new(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        pwd = (string)reader["pwd"];
                    }
                }
            }

            if (EnterPwdBox.Password == pwd) //если введенный пароль корректен
            {
                this.DialogResult = true;
            }
        }

        public string Password
        {
            get { return EnterPwdBox.Password; }
        }

    }
}
