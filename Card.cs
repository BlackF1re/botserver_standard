using System.Collections.Generic;

namespace botserver_standard
{

    public class Card
    {
        public static List<Card> cards = new(); // упорядоченный набор карточек (экземпляров классов). Нечитабельно при отладке(?)

        /*
        Всего строк: 6001
        Строк на одну карточку: 12
        Карточек: 500
        */
        public int Id { get; set; }
        public string UniversityName { get; set; }
        public string ProgramName { get; set; }
        public string Level { get; set; }
        public string ProgramCode { get; set; }
        public string StudyForm { get; set; }
        public string Duration { get; set; }
        public string StudyLang { get; set; }
        public string Curator { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Cost { get; set; }

        public Card(int id, string universityName, string programName, string level, string studyForm, string programCode, string duration, string studyLang, string curator, string phoneNumber, string email, string cost)
        {
            this.Id = id;
            this.UniversityName = universityName;
            this.ProgramName = programName;
            this.Level = level.ToLower();
            this.StudyForm = studyForm.ToLower();
            this.ProgramCode = programCode;
            this.Duration = duration.ToLower();
            this.StudyLang = studyLang.ToLower();
            this.Curator = curator;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.Cost = cost;

        }
    }
}
