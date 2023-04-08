using System.Collections.Generic;

namespace botserver_standard
{
    internal class UniversityEntryFreq
    {
        public static List<UniversityEntryFreq> universitiesFreqList = new();

        public string UniversityName { get; set; }
        public int Count { get; set; }

        public UniversityEntryFreq(string universityName, int count)
        {
            this.UniversityName = universityName;
            this.Count = count;
        }
    }
}
