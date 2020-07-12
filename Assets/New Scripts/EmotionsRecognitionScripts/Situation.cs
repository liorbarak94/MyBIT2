
public class Situation
{
    public string title;
    private int id;
    private string[] partsOfTheStory = new string[FinalValues.STORY_SIZE];
    private Question[] questions = new Question[FinalValues.NUMBER_OF_QUESTIONS];

    public Situation()
    {
    }

    public Situation(int id, string title, string[] partsOfTheStory)
    {
        this.title = title;
        this.partsOfTheStory = partsOfTheStory;
        this.id = id;
    }

    public int GetID()
    {
        return this.id;
    }

    public Question[] GetQuestions()
    {
        return questions;
    }

    public void SetQuestions(Question[] questions)
    {
        this.questions = questions;
    }

    public int AddQuestion(Question question)
    {
        int i = 0;
        for (; i < questions.Length; i++)
            if (questions[i] == null)
            {
                questions[i] = question;
                return i;
            }
        return i;
    }

    public string GetTitle()
    {
        return this.title;
    }

    public string[] GetPartsOfTheStory()
    {
        return this.partsOfTheStory;
    }
    public override string ToString()
    {
        return "The Story: part 1 - \n"
            + partsOfTheStory[0] + " \n"
            + "part 2 - \n" + partsOfTheStory[1] + " \n"
            + "part 3 - \n" + partsOfTheStory[2];
    }

    public void SetTitle(string title)
    {
        this.title = title;
    }

    public void SetPartsOfTheStory(string[] partsOfStoryArr)
    {
        this.partsOfTheStory = partsOfStoryArr;
    }
}
