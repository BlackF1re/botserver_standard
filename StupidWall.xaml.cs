﻿using System.Windows;

namespace botserver_standard
{
    public partial class StupidWall : Window
    {
        public StupidWall()
        {
            InitializeComponent();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {

            if (EnterPwdBox.Password == Settings.pwd) //если введенный пароль корректен
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
