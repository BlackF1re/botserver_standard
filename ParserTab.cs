using System;
using System.Threading.Tasks;
using System.Windows;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        private void ParserPeparseBtn_Click(object sender, RoutedEventArgs e)
        {
            Card.cards.Clear();
            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Reparsing...\n";

            });
            Task.Factory.StartNew(() => CardParser(DbWorker.sqliteConn)); //ok
        }
    }
}
