using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для ParserWindow.xaml
    /// </summary>
    public partial class ParserWindow : Window
    {
        public ParserWindow()
        {
            InitializeComponent();

            //Task.Factory.StartNew(() => parsingIsDone = ParserGUI()); //ok
            //parsingIsDone = ParserGUI(); //ok
            var t = Task.Run(() => prsr());
            t.Wait();
            Close();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            //bool isparsed;
            //if (isparsed == true)
            //{
            //    MainWindow mainWindow = new MainWindow();
            //    mainWindow.Show();
            //    this.Close();
            //}
        }

        public async void prsr()
        {
            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.AppendText("-------------------------------------------------------------------------------------------------------------\n");
                ParserLogOutput.Text += $"{DateTime.Now} | Parsing in process...\n";
                ParserLogOutput.Text += $"{DateTime.Now} | Getting data from web...\n";
            });

            var parsingUrl = "https://studyintomsk.ru/programs-main/?level=card-item&direction=card-item";
            var web = new HtmlWeb();
            HtmlDocument document;
            document = web.Load(parsingUrl);

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Selecting nodes...\n";
            });
            var value = document.DocumentNode.SelectNodes("/html/body/div[3]/div[3]"); // /html/body/div[3]/div[3]/div/div/div/div/div

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Write data...\n";
            });
            string prsFilePath = "parsing_result.prs"; //.prs = Parsing ReSult
            StreamWriter prsWriter = new(prsFilePath);
            foreach (var node in value)
            {
                prsWriter.WriteLine(node.InnerText);
            }
            prsWriter.Close();

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Parsing is done.\n";
                ParserLogOutput.Text += "-------------------------------------------------------------------------------------------------------------\n";
            });

            return;
        }


    }
}
