using System;
using System.IO;

namespace botserver_standard
{
    internal class ConsoleWorker
    {
        public static void CardOutputter()
        {
            MainWindow.AllocConsole();

            TextWriter stdOutWriter = new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding) { AutoFlush = true };
            TextWriter stdErrWriter = new StreamWriter(Console.OpenStandardError(), Console.OutputEncoding) { AutoFlush = true };
            TextReader strInReader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
            Console.SetOut(stdOutWriter);
            Console.SetError(stdErrWriter);
            Console.SetIn(strInReader);

            foreach (var item in Card.cards)
            {
                Console.WriteLine($"{item.Id} | {item.UniversityName} | {item.ProgramName} | {item.Level} | " +
                    $"{item.ProgramCode} | {item.StudyForm} | {item.Duration} | {item.StudyLang} | " +
                    $"{item.Curator} | {item.PhoneNumber} | {item.Email} | {item.Cost}");

            }

            Console.ReadKey();

            MainWindow.FreeConsole();
        }
    }
}
