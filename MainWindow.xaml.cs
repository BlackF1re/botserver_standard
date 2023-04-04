using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Exceptions;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        //импорт библиотек для запуска консоли
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public MainWindow()
        {
            InitializeComponent();

            //string tempPwd = "";

            if (Settings.pwd is "YtcPoTIZPA0WpUdc~SMCaTjL7Kvt#ne3k*{Tb|H2Kx4t227gXy") // setting new pwd if now setted default
            {
                pwdIsDefault = true;
            }
            //else
            //{
            //    Settings.pwd = tempPwd;
            //}

            if (pwdIsDefault is true)
            {
                ChangeDefaultPwd changeDefaultPwd = new();

                changeDefaultPwd.ShowDialog();
            }

            SetPwdBox.MaxLength = 50;
            SetRepeatedPwdBox.MaxLength = 50;
            //UseThisPwdCheckbox.IsChecked = Properties.Settings.Default.pwdIsSetted;

            Task.Factory.StartNew(() => CardParser()); //ok

        }
    }

}