using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
        //parsedCards
        private void SearchByProgramName_KeyUp(object sender, KeyEventArgs e)
        {
            List<Card> searchResult = new();
            string programNameFrag = SearchByProgramName.Text;
            foreach (var item in cardsView)
            {
                if (item.ProgramName.ToLower().Contains(programNameFrag))
                {
                    searchResult.Add(item);
                }
                else { continue; }
            }
            parsedCardsGrid.ItemsSource = searchResult;
        }

        private void SearchByUniversity_KeyUp(object sender, KeyEventArgs e)
        {
            List<Card> searchResult = new();
            string universityNameFrag = SearchByUniversity.Text;
            foreach (var item in cardsView)
            {
                if (item.UniversityName.ToLower().Contains(universityNameFrag))
                {
                    searchResult.Add(item);
                }
                else { continue; }
            }
            parsedCardsGrid.ItemsSource = searchResult;
        }

        private void CardsExportBtn_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter parsedCardsExport = new(Settings.datagridExportPath);
            parsedCardsExport.WriteLine("Id Название университета\tНазвание программы\tКод программы\tУровень обучения\tФорма обучения\tДлительность обучения\tЯзык обучения\tКуратор\tНомер телефона\tПочта\tСтоимость");
            foreach (var item in cardsView)
            {

                parsedCardsExport.WriteLine($"{item.Id}\t{item.UniversityName}\t{item.ProgramName}\t{item.ProgramCode}\t{item.Level}\t{item.StudyForm}\t{item.Duration}\t{item.StudyLang}\t{item.Curator}\t{item.PhoneNumber}\t{item.Email}\t{item.Cost}");

            }
            parsedCardsExport.Close();
        }

        //parsedUniversities
        private void SearchByUniversityName_KeyUp(object sender, KeyEventArgs e)
        {
            List<UniversityEntryFreq> searchResult = new();
            string universityNameFrag = SearchByUniversityName.Text;
            foreach (var item in UniversityEntryFreq.universitiesFreqList)
            {
                if (item.UniversityName.ToLower().Contains(universityNameFrag))
                {
                    searchResult.Add(item);
                }
                else { continue; }
            }
            parsedUniversitiesGrid.ItemsSource = searchResult;
        }

        private void SearchByUniversityFreq_KeyUp(object sender, KeyEventArgs e)
        {
            List<UniversityEntryFreq> searchResult = new();
            string universityNameFrag = SearchByUniversityFreq.Text;
            foreach (var item in UniversityEntryFreq.universitiesFreqList)
            {
                if (item.Count.ToString().Contains(universityNameFrag))
                {
                    searchResult.Add(item);
                }
                else { continue; }
            }
            parsedUniversitiesGrid.ItemsSource = searchResult;
        }

        private void UniversitiesExportBtn_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter parsedUniversitiesExport = new("exportUniversities.txt");
            parsedUniversitiesExport.WriteLine("Id Название университета\tКоличество программ");
            foreach (var item in UniversityEntryFreq.universitiesFreqList)
            {
                parsedUniversitiesExport.WriteLine($"{item.UniversityName}\t{item.Count}");
            }
            parsedUniversitiesExport.Close();
        }
    }
}
