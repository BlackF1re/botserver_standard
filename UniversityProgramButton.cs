namespace botserver_standard
{
    public class UniversityProgramButton
    {

        public int Id { get; set; } //buttonID = cardId (callbackData)
        public string Text { get; set; } //programName, buttonText

        public UniversityProgramButton(string programName, int id)
        {
            this.Id = id;
            this.Text = programName;
        }
    }

}
