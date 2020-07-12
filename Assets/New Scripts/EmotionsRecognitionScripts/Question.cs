
public class Question
{
    private string questText;
    private string[] answers;
    private string rightAnswer;
    
    public Question()
    {

    }

    public Question(string questText, string[] answers, string rightAnswer)
    {
        this.questText = questText;
        this.answers = answers;
        this.rightAnswer = rightAnswer;
    }

    public string GetQuestText()
    {
        return questText;
    }

    public string[] GetAnswers()
    {
        return answers;
    }

    public string GetRightAnswer()
    {
        return rightAnswer;
    }

    public void SetRightAnswer(string rightAnswer)
    {
        this.rightAnswer = rightAnswer;
    }

    public void SetQuestText(string questText)
    {
        this.questText = questText;
    }

    public void SetAnswers(string[] answers)
    {
        this.answers = answers;
    }
}
