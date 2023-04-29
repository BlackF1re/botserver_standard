using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
            if (Settings.pwdIsUsing is true)
            {
                AskingPassword askPwd = new();
                if (askPwd.ShowDialog() == true)
                {
                    MessageBox.Show("Добро пожаловать!");
                    UseThisPwdCheckbox.IsChecked = false; //обновление состояния чекбокса в окне
                }
                else
                {
                    MessageBox.Show("Операция отменена.");
                    Environment.Exit(0);
                }
            }

            UseThisPwdCheckbox.IsChecked = Settings.pwdIsUsing;

            if (Settings.pwd is "YtcPoTIZPA0WpUdc~SMCaTjL7Kvt#ne3k*{Tb|H2Kx4t227gXy") // setting new pwd if now setted default
            {
                ChangeDefaultPwd changeDefaultPwd = new();
                changeDefaultPwd.ShowDialog();
                if (this.DialogResult is true)
                {
                    UseThisPwdCheckbox.IsChecked = true;
                }
            }

            SetPwdBox.MaxLength = 50;
            SetRepeatedPwdBox.MaxLength = 50;

            Task.Factory.StartNew(() => CardParser(DbWorker.sqliteConn)); //ok
            
        }

        private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (mainTab.IsSelected)
            {
                HomePic.Foreground = Brushes.RoyalBlue;
                ParserPic.Foreground = Brushes.LightGray;
                ParsedCardsPic.Foreground = Brushes.LightGray;
                ParsedUnivsPic.Foreground = Brushes.LightGray;
                SettingsPic.Foreground = Brushes.LightGray;
                AboutPic.Foreground = Brushes.LightGray;
            }

            if (parserTab.IsSelected)
            {
                HomePic.Foreground = Brushes.LightGray;
                ParserPic.Foreground = Brushes.RoyalBlue;
                ParsedCardsPic.Foreground = Brushes.LightGray;
                ParsedUnivsPic.Foreground = Brushes.LightGray;
                SettingsPic.Foreground = Brushes.LightGray;
                AboutPic.Foreground = Brushes.LightGray;
            }

            if (parsedCardsTab.IsSelected)
            {
                HomePic.Foreground = Brushes.LightGray;
                ParserPic.Foreground = Brushes.LightGray;
                ParsedCardsPic.Foreground = Brushes.RoyalBlue;
                ParsedUnivsPic.Foreground = Brushes.LightGray;
                SettingsPic.Foreground = Brushes.LightGray;
                AboutPic.Foreground = Brushes.LightGray;
            }

            if (parsedUniversitiesTab.IsSelected)
            {
                HomePic.Foreground = Brushes.LightGray;
                ParserPic.Foreground = Brushes.LightGray;
                ParsedCardsPic.Foreground = Brushes.LightGray;
                ParsedUnivsPic.Foreground = Brushes.RoyalBlue;
                SettingsPic.Foreground = Brushes.LightGray;
                AboutPic.Foreground = Brushes.LightGray;
            }

            if (settingsTab.IsSelected)
            {
                HomePic.Foreground = Brushes.LightGray;
                ParserPic.Foreground = Brushes.LightGray;
                ParsedCardsPic.Foreground = Brushes.LightGray;
                ParsedUnivsPic.Foreground = Brushes.LightGray;
                SettingsPic.Foreground = Brushes.RoyalBlue;
                AboutPic.Foreground = Brushes.LightGray;
            }
            
            if (aboutTab.IsSelected)
            {
                HomePic.Foreground = Brushes.LightGray;
                ParserPic.Foreground = Brushes.LightGray;
                ParsedCardsPic.Foreground = Brushes.LightGray;
                ParsedUnivsPic.Foreground = Brushes.LightGray;
                SettingsPic.Foreground = Brushes.LightGray;
                AboutPic.Foreground = Brushes.RoyalBlue;
            }

        }
    }

}