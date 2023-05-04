using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        private void ParserPeparseBtn_Click(object sender, RoutedEventArgs e)
        {
            universityFreqListView.Clear();
            Card.cards.Clear();
            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Reparsing...\n";

            });
            Task.Factory.StartNew(() => CardParser(DbWorker.sqliteConn)); //ok
        }

        private void ParserExportBtn_Click(object sender, RoutedEventArgs e)
        {
            //string parserOutPath = Settings.baseLogPath + $"parser_{DateTime.Now}.txt";
            string pathPart = $"parser_{DateTime.Now}.txt".Replace(":", "_");
            string parserOutPath = Settings.baseLogPath + pathPart;
            StreamWriter parserExport = new(parserOutPath);
            parserExport.WriteLine(ParserLogOutput.Text);
            parserExport.Close();
        }
    }
}