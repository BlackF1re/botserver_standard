using Microsoft.Data.Sqlite;
using System;
using System.Windows;

namespace botserver_standard
{
    public partial class ChangeDefaultPwd : Window
    {
        public ChangeDefaultPwd()
        {
            InitializeComponent();
            EnterPwdBox.MaxLength = 50;
            EnterPwdBox_Repeated.MaxLength = 50;
        }

        int rowsChanged;
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            string changeDefaultPwdQuery = $"UPDATE Settings SET pwd = '{EnterPwdBox.Password}';";
            string setCheckboxQuery = $"UPDATE Settings SET pwdIsUsing = 'True';";
            DateTime updateMoment;
            string previousPwdWrite = $"INSERT INTO Passwords_history (timestamp, oldPassword) VALUES ('{updateMoment = DateTime.Now}', '{Settings.pwd}');"; //установка пароля по умолчанию и отключение его запроса при старте


            if (EnterPwdBox.Password == EnterPwdBox_Repeated.Password && EnterPwdBox_Repeated.Password != Settings.pwd) // если юзер не ошибся и пароль не равен предыдущему
            {
                rowsChanged = DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, changeDefaultPwdQuery); //смена дефолтного пароля
                //IsSetted?
                if (rowsChanged is 1) //если удачно
                {
                    DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, previousPwdWrite); //запись истории паролей

                    DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, setCheckboxQuery); //установка галки на использование пароля на старте

                    using SqliteDataReader reader = DbWorker.SettingsReader(DbWorker.readSettings, DbWorker.sqliteConn); //обновление настроек приложения из бд
                    
                    MessageBox.Show("Пароль успешно установлен. На вкладке \"Settings\" вы можете отключить его использование.", "Notice");
                    
                    this.DialogResult = true;
                    
                }

                else
                { MessageBox.Show("Непредвиденная ошибка", "Error"); }
            }
            else 
            {
                MessageBox.Show("Вы пытаетесь установить пароль по умолчанию, либо пароли не совпадают", "Notice");
            }

            
        }

    }
}
