using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Xml.Linq;

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

            //Console.Write("Enter the text: ");
            //string? text = Console.ReadLine();
            //if (text is not null)
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        //Thread.Sleep(30);
            //        Console.WriteLine(text + "  " + i);
            //    }
            //}
            //else { Console.WriteLine("Text is null!"); }
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
