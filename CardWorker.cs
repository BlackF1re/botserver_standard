using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace botserver_standard
{
    internal class CardWorker
    {
        struct Card
        {
            public int Id;
            /*
            Всего строк: 6501
            Строк на одну карточку: 13
            Карточек: 500
            */
            public string UniversityName;
            public string ProgramName;
            public string ProgramName2;//what?
            public string Level;
            public string ProgramCode;
            public string StudyForm;
            public string Duration;
            public string Qualification; //same as Level
            public string StudyLang;
            public string Curator;
            public string PhoneNumber;
            public string Email;
            public string Cost;
        }


        StreamReader streamReader = new("parsedData.prs");

    }
}
