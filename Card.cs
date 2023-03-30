using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botserver_standard
{
    public class Card
    {
        /*
        Всего строк: 6001
        Строк на одну карточку: 12
        Карточек: 500
        */
        public int Id;
        public string UniversityName;
        public string ProgramName;
        public string Level;
        public string ProgramCode;
        public string StudyForm;
        public string Duration;
        public string StudyLang;
        public string Curator;
        public string PhoneNumber;
        public string Email;
        public string Cost;

        public Card(int id, string universityName, string programName, string level, string studyForm, string programCode, string duration, string studyLang, string curator, string phoneNumber, string email, string cost)
        {
            this.Id = id;
            this.UniversityName = universityName;
            this.ProgramName = programName;
            this.Level = level;
            this.StudyForm = studyForm;
            this.ProgramCode = programCode;
            this.Duration = duration;
            this.StudyLang = studyLang;
            this.Curator = curator;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.Cost = cost;

        }
    }
}
