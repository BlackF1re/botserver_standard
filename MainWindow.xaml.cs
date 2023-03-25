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
            SetPwdBox.MaxLength = 50;
            SetRepeatedPwdBox.MaxLength = 50;
            UseThisPwdCheckbox.IsChecked = Properties.Settings.Default.pwdIsSetted;

            Task.Factory.StartNew(() => ParserGUI()); //ok


            //bool parsingIsDone = false;
            //ParserWindow parserWindow = new ParserWindow();
            //parserWindow.Show();
            //this.Hide();
            ////parserWindow.ParserGUI();
            //Task.Factory.StartNew(() => parsingIsDone = parserWindow.ParserGUI()); //ok
            //if (parsingIsDone is true) { parserWindow.Close(); }

        }

        private void OptionsBtn1_Click(object sender, RoutedEventArgs e)
        {

        }

    }

}