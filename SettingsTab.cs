using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;


namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        public static bool pwdIsDefault; 


        private void SetTokenBtn_Click(object sender, RoutedEventArgs e)
        {
            string updateTokenQuery = $"UPDATE Settings SET botToken = '{SettingsTokenInput.Text.Trim(' ')}';";
            int rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, updateTokenQuery);

            //IsChanged?
            if (rowsChanged is 1)
            {
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn); //from db to fields

                string tokenLeftPart = SettingsTokenInput.Text.Substring(0, SettingsTokenInput.Text.Length - 35); // :AAF3nNDlYNfryOulNHKtsxlhuGo_roxXYXI
                SettingsTokenInput.Text = $"Token has been changed to {tokenLeftPart}";
            }
            else { SettingsTokenInput.Text += "Unforseen error"; }


            //using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);
            //LiveLogOutput.Text += $"{Settings.logPath}, {Settings.connString}, {Settings.botToken}, {Settings.pwd}, {Settings.pwdIsSetted} \n";
        }

        private void SetLogPathBtn_Click(object sender, RoutedEventArgs e)
        {
            string updateTokenQuery = $"UPDATE Settings SET logPath = '{SettingsLogRootInput.Text}';";
            int rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, updateTokenQuery);

            //IsChanged?
            if (rowsChanged is 1)
            {
                using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);

                SettingsLogRootInput.Text = $"file path has been setted.";
            }
            else { SettingsTokenInput.Text += "Unforseen error"; }
        }

        private void SetDbPathBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented.");
        }

        private void SettingsFilePathSetBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImportSettingsBtn_Click(object sender, RoutedEventArgs e)
        {

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
                        Settings.pwd = SetRepeatedPwdBox.Password;

                        string updatePwdQuery = $"UPDATE Settings SET pwd = '{SetPwdBox.Password.Trim(' ')}';";

                        rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, updatePwdQuery);

                        //IsChanged?
                        if (rowsChanged is 1)
                        {
                            using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn);

                            MessageBox.Show($"your password has been changed at {DateTime.Now}.", "Notice");
                        }
                        else { SettingsTokenInput.Text += "Unforseen error"; }
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
            bool isChecked = true;

            string setCheckboxQuery = $"UPDATE Settings SET pwdIsUsing = '{isChecked}';";
            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, setCheckboxQuery);
        }

        private void UseThisPwdCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            StupidWall stupidWall = new();

            if (stupidWall.ShowDialog() == true)
            {
                if (stupidWall.EnterPwdBox.Password == Settings.pwd)
                {
                    MessageBox.Show("Авторизация пройдена");
                    UseThisPwdCheckbox.IsChecked = false;
                    string updatePwdIsUsingQuery = $"UPDATE Settings SET pwdIsUsing = '{UseThisPwdCheckbox.IsChecked}';";
                    DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, updatePwdIsUsingQuery);
                }

                if (stupidWall.EnterPwdBox.Password != Settings.pwd)
                {                    
                    MessageBox.Show("Неверный пароль");
                    UseThisPwdCheckbox.IsChecked = true;
                    string updatePwdIsUsingQuery = $"UPDATE Settings SET pwdIsUsing = '{UseThisPwdCheckbox.IsChecked}';";
                    DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, updatePwdIsUsingQuery);
                }
            }
            else
            {
                MessageBox.Show("Лох!");
            }
            

        }
    }
}
