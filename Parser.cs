using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        public void ParserGUI()
        {

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.AppendText("-----------------------------------------------------------------------------------------------------------\n");
                ParserLogOutput.Text += $"{DateTime.Now} | Parsing in process...\n";
            });

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Getting data from web...\n";

            });
            var parsingUrl = "https://studyintomsk.ru/programs-main/";
            var web = new HtmlWeb();
            HtmlDocument document;

            document = web.Load(parsingUrl); //loading html
            /*
            /html/body/div[2]/div/div[3]/div[5]/select - программы подготовки
            /html/body/div[3]/div[3] - карточки со сдвигами
            /html/body/div[2]/div/div[3]/div[3]/select - вузы
            /html/body/div[2]/div/div[3]/div[1]/select - уровни
            /html/body/div[2]/div/div[3]/div[4]/select - языки
            */

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Selecting nodes...\n";
            });
            if (document is null)
            {
                return;
            }
            var value = document.DocumentNode.SelectNodes("/html/body/div[3]/div[3]"); // /html/body/div[3]/div[3]/div/div/div/div/div
            string noTabsDoc = string.Empty; //первичная строка с сырыми данными

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Processing received data...\n";
            });
            foreach (var item in value)
            {
                noTabsDoc += item.InnerText; //single row?
            }
            noTabsDoc = noTabsDoc.Replace("\t", "\n"); //замена табуляций

            List<string> cardsList = new(); //лист под данные всех карточек

            cardsList = noTabsDoc.Split('\n').ToList(); //построчная запись данных (в том числе и мусора)

            CardsListCleaner(cardsList); //удаление мусора из листа

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Writing data...\n";
            });
            string prsFilePath = "parsing_result.prs"; //.prs = Parsing ReSult
            StreamWriter prsWriter = new(prsFilePath);

            foreach (string line in cardsList) //запись листа с карточками в файл
            {
                prsWriter.WriteLine(line);
            }
            prsWriter.Close();

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Parsing is done. {cardsList.Count} lines were written.\n";
                ParserLogOutput.Text += "-----------------------------------------------------------------------------------------------------------\n";
            });

        }

        private static void CardsListCleaner(List<string> cardsList)
        {
            string itemToRemove = "\n";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Уровень обучения";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Форма обучения";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Код программы ";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Продолжительность";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Степень или квалификация";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Язык обучения";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Куратор";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "за год обучения";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Поступить";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Нет подходящей программы?";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Напишите нам об этом и мы придумаем для вас индивидуальное  решение.";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "Получить решение";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = string.Empty;
            cardsList.RemoveAll(x => x == itemToRemove);
        }


        //public static void ParserGUI()
        //{

        //    ((MainWindow)Application.Current.MainWindow).Dispatcher.Invoke(() =>
        //    {
        //        ((MainWindow)Application.Current.MainWindow).ParserLogOutput.AppendText("-------------------------------------------------------------------------------------------------------------\n");
        //        ((MainWindow)Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Parsing in process...\n";
        //        ((MainWindow)Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Getting data from web...\n";
        //    });

        //    var parsingUrl = "https://studyintomsk.ru/programs-main/?level=card-item&direction=card-item";
        //    var web = new HtmlWeb();
        //    HtmlDocument document;
        //    document = web.Load(parsingUrl);

        //    ((MainWindow)Application.Current.MainWindow).Dispatcher.Invoke(() =>
        //    {
        //        ((MainWindow)Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Selecting nodes...\n";
        //    });
        //    var value = document.DocumentNode.SelectNodes("/html/body/div[3]/div[3]"); // /html/body/div[3]/div[3]/div/div/div/div/div

        //    ((MainWindow)Application.Current.MainWindow).Dispatcher.Invoke(() =>
        //    {
        //        ((MainWindow)Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Write data...\n";
        //    });
        //    string prsFilePath = "parsing_result.prs"; //.prs = Parsing ReSult
        //    StreamWriter prsWriter = new(prsFilePath);
        //    foreach (var node in value)
        //    {
        //        prsWriter.WriteLine(node.InnerText);
        //    }
        //    prsWriter.Close();

        //    ((MainWindow)Application.Current.MainWindow).Dispatcher.Invoke(() =>
        //    {
        //        ((MainWindow)Application.Current.MainWindow).ParserLogOutput.Text += $"{DateTime.Now} | Parsing is done.\n";
        //        ((MainWindow)Application.Current.MainWindow).ParserLogOutput.Text += "-------------------------------------------------------------------------------------------------------------\n";
        //    });

        //}
    }
}
