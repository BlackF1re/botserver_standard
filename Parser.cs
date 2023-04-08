using HtmlAgilityPack;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        List<Card> cardsView = new();
        public async void CardParser(SqliteConnection sqliteConn)
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
            var cardsValue = document.DocumentNode.SelectNodes("/html/body/div[3]/div[3]/div/div/div/div/div");

            string noTabsDoc = string.Empty; //первичная строка с сырыми данными

            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Processing received data...\n";
            });
            foreach (var item in cardsValue)
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
                Card.cards.Add(new Card(Id, cardsList[row0], cardsList[row1], cardsList[row2],
                                    cardsList[row3], cardsList[row4], cardsList[row5],
                                    cardsList[row7], cardsList[row8], cardsList[row9],
                                    cardsList[row10], cardsList[row11]));
                Id++;

                //прыжок на строки следующей карточки
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

            string clearCardsDb = "DELETE FROM Cards;";
            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, clearCardsDb);

            //запись полученных карточек в бд
            foreach (var item in Card.cards)
            {
                string cardsToDb = $"INSERT INTO Cards(id, universityName, programName, programCode, level, studyForm, duration, studyLang, curator, phoneNumber, email, cost) " +
                $"VALUES('{item.Id}', '{item.UniversityName}', '{item.ProgramName}', '{item.ProgramCode}', '{item.Level}', '{item.StudyForm}', '{item.Duration}', '{item.StudyLang}', '{item.Curator}', '{item.PhoneNumber}', '{item.Email}', '{item.Cost}')";
                DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, cardsToDb);

            }

            //вывод данных из бд на вкладку карточек

            int id;
            string? universityName;
            string? programName;
            string? level;
            string? programCode;
            string? studyForm;
            string? duration;
            string? studyLang;
            string? curator;
            string? phoneNumber;
            string? email;
            string? cost;
            string queryText = "SELECT * FROM Cards";

            sqliteConn.Open(); //открытие соединения
            SqliteCommand command = new() //инициализация экземпляра SqliteCommand
            {
                Connection = sqliteConn, //соединение для выполнения запроса
                CommandText = queryText //текст запроса
            };
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть строки
            {
                while (reader.Read())   // построчное чтение данных
                {
                    //public Card(int id, string universityName, string programName, string level, string studyForm, string programCode, string duration, string studyLang, string curator, string phoneNumber, string email, string cost)

                    id = Convert.ToInt32(reader["Id"]);
                    universityName = Convert.ToString(reader["universityName"]);
                    programName = Convert.ToString(reader["programName"]);
                    programCode = Convert.ToString(reader["programCode"]);
                    level = Convert.ToString(reader["level"]);
                    studyForm = Convert.ToString(reader["studyForm"]);
                    duration = Convert.ToString(reader["duration"]);
                    studyLang = Convert.ToString(reader["studyLang"]);
                    curator = Convert.ToString(reader["curator"]);
                    phoneNumber = Convert.ToString(reader["phoneNumber"]);
                    email = Convert.ToString(reader["email"]);
                    cost = Convert.ToString(reader["cost"]);

                    cardsView.Add(new Card(id, universityName, programName, level, studyForm, programCode, duration, studyLang, curator, phoneNumber, email, cost));
                }
            }
            sqliteConn.Close();

            //выделение уникальных вузов
            List<string> universitiesList = new();
            int universityRow = 0;
            int rowCounter = 0;
            foreach (string line in cardsList)
            {

                universitiesList.Add(cardsList[universityRow]);
                universityRow += 12;
                rowCounter++;
                if (rowCounter >= cardsTotalRows)
                    break;
            }

            //ТУСУР - 78 раз
            //ТПУ - 203 раз
            //ТГПУ - 52 раз
            //ТГАСУ - 45 раз
            //ТГУ - 122 раз

            foreach (string item in universitiesList.Distinct())
            {
                UniversityEntryFreq.universitiesFreqList.Add(new UniversityEntryFreq(item, universitiesList.Where(x => x == item).Count()));
            }

            string clearUniversitiesFreqDb = "DELETE FROM Universities;";
            DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, clearUniversitiesFreqDb);
            id = 0;
            //запись полученных карточек в бд
            foreach (var item in UniversityEntryFreq.universitiesFreqList)
            {
                string universitiesToDb = $"INSERT INTO Universities(id, universityName, universityCount) " +
                $"VALUES('{id}', '{item.UniversityName}', '{item.Count}');";
                DbWorker.DbQuerySilentSender(DbWorker.sqliteConn, universitiesToDb);
                id++;
            }

            //вывод данных из бд на вкладку карточек
            
            string? freqUniversityName;
            int freqUniversityCount;

            queryText = "SELECT * FROM Universities";

            sqliteConn.Open(); //открытие соединения
            SqliteCommand freqCommand = new() //инициализация экземпляра SqliteCommand
            {
                Connection = sqliteConn, //соединение для выполнения запроса
                CommandText = queryText //текст запроса
            };
            SqliteDataReader freqReader = freqCommand.ExecuteReader();

            List < UniversityEntryFreq > universityFreqListView= new();
            if (freqReader.HasRows) // если есть строки
            {
                while (freqReader.Read())   // построчное чтение данных
                {
                    freqUniversityName = Convert.ToString(freqReader["universityName"]);
                    freqUniversityCount = Convert.ToInt32(freqReader["universityCount"]);

                    universityFreqListView.Add(new UniversityEntryFreq(freqUniversityName, freqUniversityCount));
                }
            }
            sqliteConn.Close();


            Dispatcher.Invoke(() =>
            {
                parsedCardsGrid.ItemsSource = cardsView;
            });

            Dispatcher.Invoke(() =>
            {
                parsedUniversitiesGrid.ItemsSource = universityFreqListView;
            });


            Dispatcher.Invoke(() =>
            {
                ParserLogOutput.Text += $"{DateTime.Now} | Parsing is done.\n {Card.cards.Count} cards have been added;\n {universityFreqListView.Count} universities have been added.\n";
                ParserLogOutput.Text += "-----------------------------------------------------------------------------------------------------------\n";
            });
        }

    }
}
