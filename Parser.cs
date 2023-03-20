using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace botserver_standard
{
    public static class Parser
    {
        public static void ParserGUI()
        {

            //((MainWindow)System.Windows.Application.Current.MainWindow).Dispatcher.Invoke(() =>
            //{
                ((MainWindow)System.Windows.Application.Current.MainWindow).ParserLogOutput.Text += "-------------------------------------------------------------------------------------------------------------\n";
                ((MainWindow)System.Windows.Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Parsing in process...\n";
                ((MainWindow)System.Windows.Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Getting data from web...\n";
            //});

            var parsingUrl = "https://studyintomsk.ru/programs-main/?level=card-item&direction=card-item";
            var web = new HtmlWeb();
            HtmlDocument document;
            document = web.Load(parsingUrl);

            ((MainWindow)System.Windows.Application.Current.MainWindow).Dispatcher.Invoke(() =>
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Selecting nodes...\n";
            });
            var value = document.DocumentNode.SelectNodes("/html/body/div[3]/div[3]"); // /html/body/div[3]/div[3]/div/div/div/div/div

            ((MainWindow)System.Windows.Application.Current.MainWindow).Dispatcher.Invoke(() =>
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Write data...\n";
            });
            string prsFilePath = "parsing_result.prs"; //.prs = Parsing ReSult
            StreamWriter prsWriter = new(prsFilePath);
            foreach (var node in value)
            {
                prsWriter.WriteLine(node.InnerText);
            }
            prsWriter.Close();

            ((MainWindow)System.Windows.Application.Current.MainWindow).Dispatcher.Invoke(() =>
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Parsing is done.\n";
                ((MainWindow)System.Windows.Application.Current.MainWindow).ParserLogOutput.Text += "-------------------------------------------------------------------------------------------------------------\n";
            });
        }
    }
}
