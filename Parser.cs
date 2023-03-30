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
        public static List<Card> cards = new(); // упорядоченный набор карточек (классов). Нечитабельно при отладке(?)

        public void CardParser()
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
            var value = document.DocumentNode.SelectNodes("/html/body/div[3]/div[3]/div/div/div/div/div");

            string noTabsDoc = string.Empty; //первичная строка с сырыми данными

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Processing received data...\n";
            });
            foreach (var item in value)
            {
                noTabsDoc += item.InnerText; //node is single row?
            }
            noTabsDoc = noTabsDoc.Replace("\t", "\n"); //замена табуляций

            List<string> cardsList = new(); //лист с правильными данными, идущими подряд
            cardsList = noTabsDoc.Split('\n').ToList(); //построчная запись данных (в том числе и мусора)

            //удаление мусора из листа
            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Cleaning data...\n";
            });
            #region data cleaning

            string itemToRemove = "Уровень обучения";
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

            itemToRemove = "Основная программа О программе";
            cardsList.RemoveAll(x => x == itemToRemove);

            //symbols
            itemToRemove = "\n";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = "";
            cardsList.RemoveAll(x => x == itemToRemove);

            itemToRemove = string.Empty;
            cardsList.RemoveAll(x => x == itemToRemove);
            #endregion

            /*
            Всего строк: 6501
            Строк на одну карточку: 12
            Карточек: 500
            */

            //строки для передачи в атрибуты экземпляра класса 
            int Id = 0; // идентификатор экземпляра класса. Задать в бд?
            string UniversityName;
            string ProgramName;
            string Level;
            string ProgramCode;
            string StudyForm;
            string Duration;
            string StudyLang;
            string Curator;
            string PhoneNumber;
            string Email;
            string Cost;

            //для прыжков по строкам всех карточек в листе
            int row0 = 0;
            int row1 = 1;
            int row2 = 2;
            int row3 = 3;
            int row4 = 4;
            int row5 = 5;
            //int row6 = 6; //пропуск повторяющегося атрибута
            int row7 = 7;
            int row8 = 8;
            int row9 = 9;
            int row10 = 10;
            int row11 = 11;

            int cardCounter = 0;
            int cardsTotalRows = cardsList.Count / 12;

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Writing data...\n";
            });
            foreach (string line in cardsList)
            {
                cards.Add(new Card(Id, UniversityName = cardsList[row0], ProgramName = cardsList[row1], Level = cardsList[row2],
                                    StudyForm = cardsList[row3], ProgramCode = cardsList[row4], Duration = cardsList[row5],
                                    StudyLang = cardsList[row7], Curator = cardsList[row8], PhoneNumber = cardsList[row9],
                                    Email = cardsList[row10], Cost = cardsList[row11]));
                Id++;

                //переход на строки для следующей карточки
                row0 += 12;
                row1 += 12;
                row2 += 12;
                row3 += 12;
                row4 += 12;
                row5 += 12;
                //row6 += 12; //пропуск повторяющегося атрибута
                row7 += 12;
                row8 += 12;
                row9 += 12;
                row10 += 12;
                row11 += 12;

                cardCounter++;
                if (cardCounter == cardsTotalRows)
                    break;
            }

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Parsing is done. {cards.Count} cards have been added.\n";
                ParserLogOutput.Text += "-----------------------------------------------------------------------------------------------------------\n";
            });
        }

    }
}
