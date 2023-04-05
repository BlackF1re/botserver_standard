using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace botserver_standard
{
    public partial class MainWindow : Window
    {
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
    }
}
