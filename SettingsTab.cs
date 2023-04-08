using Microsoft.Data.Sqlite;
using System;
using System.Windows;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {

        private void SetTokenBtn_Click(object sender, RoutedEventArgs e) //ok
        {
            string query = $"UPDATE Settings SET botToken = '{SettingsTokenInput.Text.Trim(' ')}';";
            int rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, query);

            //IsChanged?
            if (rowsChanged is 1)
            {
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn); //from db to fields

                string tokenLeftPart = SettingsTokenInput.Text[..^35]; // :AAF3nNDlYNfryOulNHKtsxlhuGo_roxXYXI
                SettingsTokenInput.Text = $"Token has been changed to {tokenLeftPart}";
            }
            else 
            {
                SettingsTokenInput.Text = "Unforseen error";
            }
        }

        private void SetMessageLogPathBtn_Click(object sender, RoutedEventArgs e) //ok
        {
            string query = $"UPDATE Settings SET logPath = '{MessagesLogRootInput.Text}';";
            int rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, query);

            //IsChanged?
            if (rowsChanged is 1)
            {
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);

                MessagesLogRootInput.Text = $"file path has been setted.";
            }
            else
            {
                MessagesLogRootInput.Text = "Unforseen error";
            }
        }

        private void SetDatagridExportPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string query = $"UPDATE Settings SET datagridExportPath = '{datagridExportPath.Text}';";
            int rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, query);

            //IsChanged?
            if (rowsChanged is 1)
            {
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);

                datagridExportPath.Text = $"file path has been setted.";
            }
            else
            {
                datagridExportPath.Clear();
                datagridExportPath.Text += "Unforseen error";
            }

        }

        private void ChoicesLogRootInputSetBtn_Click(object sender, RoutedEventArgs e)
        {
            string query = $"UPDATE Settings SET callbackLoggerPath = '{ChoicesLogRootInput.Text}';";
            int rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, query);

            //IsChanged?
            if (rowsChanged is 1)
            {
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);

                ChoicesLogRootInput.Text = $"Choices log path has been setted.";
            }
            else
            {
                ChoicesLogRootInput.Text = "Unforseen error";
            }
        }

        private void ParsingUrlSetBtn_Click(object sender, RoutedEventArgs e)
        {
            string query = $"UPDATE Settings SET parsingUrl = '{UrlSet.Text}';";
            int rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, query);

            //IsChanged?
            if (rowsChanged is 1)
            {
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);

                ChoicesLogRootInput.Text = $"Parsing URL has been setted.";
            }
            else
            {
                ChoicesLogRootInput.Text = "Unforseen error";
            }
        }

        private void SetPwdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SetPwdBox.Password.Length >3 && SetRepeatedPwdBox.Password.Length >3 && SetPwdBox.Password == SetRepeatedPwdBox.Password)
            {
                int rowsChanged;
                StupidWall stupidWall = new();

                if (stupidWall.ShowDialog() == true) // if checking is success
                {
                    if (stupidWall.EnterPwdBox.Password == Settings.pwd)
                    {
                        MessageBox.Show("Авторизация пройдена");

                        string updatePwdQuery = $"UPDATE Settings SET pwd = '{SetPwdBox.Password}';";

                        rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, updatePwdQuery); // обновление бд

                        //IsChanged?
                        if (rowsChanged is 1)
                        {
                            using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn); //обновление локальных настроек из бд

                            PwdChangeNotifier.Content = $"Your password has been changed at {DateTime.Now}.";
                        }
                        else { PwdChangeNotifier.Content = "Unforseen error. Use old password."; }
                    }

                    if (stupidWall.EnterPwdBox.Password != Settings.pwd)
                    {
                        MessageBox.Show("Неверный пароль"); //if checking is failed
                    }
                }

                else
                {
                    MessageBox.Show("Cancelled"); // if cancelled
                }
            }

            else
            {
                MessageBox.Show("Passwords are not the same, try again", "Error");
                SetRepeatedPwdBox.Clear();
            }
        }

        public void UseThisPwdCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            string setCheckboxQuery = $"UPDATE Settings SET pwdIsUsing = 'True';";
            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, setCheckboxQuery); // обновление бд
            using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn); //обновление локальных настроек из бд

        }

        private void UseThisPwdCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            StupidWall stupidWall = new();

            if (stupidWall.ShowDialog() == true)
            {

                MessageBox.Show("Запрос пароля отключен.");
                UseThisPwdCheckbox.IsChecked = false; //обновление состояния чекбокса в окне

                string updatePwdIsUsingQuery = $"UPDATE Settings SET pwdIsUsing = 'False';";
                DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, updatePwdIsUsingQuery); //обновление состояния чекбокса в бд
            }
            else
            {
                MessageBox.Show("Операция отменена.");
                UseThisPwdCheckbox.IsChecked = true;
            }


        }
    }
}
