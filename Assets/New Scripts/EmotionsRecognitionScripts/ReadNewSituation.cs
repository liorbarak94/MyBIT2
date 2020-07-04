using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Firebase.Auth;

public class ReadNewSituation : MonoBehaviour
{
    private DatabaseReference reference;
    protected FirebaseUser currentUser;

    private string titleStr;
    private int situationsCounter, partOfStoryIndex = 0, currentSituationNumber = 0, currentQuestionNumber = 0, usersCounter;
    private string[] partsOfStoryArr = new string[FinalValues.STORY_SIZE];
    private Question question;
    private Situation situation = new Situation();
    private Question[] questionsArr = new Question[FinalValues.NUMBER_OF_QUESTIONS];
    private string[] answers;

    public TMP_Text storyText, titleText, rightAnswerExplainText, worngAnswerExplainText;
    public Image backButton, nextButton, restartButton, startButton, goBackToQuestButton;

    private bool isLoaded = false;

    public Animator animatorController;
    public GameObject imagesSwap, storyObjects, qusetionsObjects, startQuestionsObjects, startLevelCanvas,
        answersObjects, answerExplain;

    public Text questionText;
    public Text answer1Text, answer2Text, answer3Text, answer4Text;
    public Sprite answer1Sprite, answer2Sprite, answer3Sprite, answer4Sprite, answer5Sprite,
        answer6Sprite, answer7Sprite, answer8Sprite, answer9Sprite;
    public Image answer1Image, answer2Image, answer3Image, answer4Image;
    public Image previousQuestArrow, nextQuestionArrow;
    public Image returnToStory;
    public Image backToQuestions;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        animatorController = imagesSwap.GetComponent<Animator>();
        GetSituationCounter();
        GetUsersCounter();
    }

    void Update()
    {
        if (situationsCounter != 0 && !isLoaded)
        {
            Debug.Log("situationsCounter: " + situationsCounter);
            LoadSituationsFromFirebase();
            GetSituationTitle();
            isLoaded = true;
        }
        titleText.text = titleStr;
    }

    private void GetSituationCounter()
    {
        reference.Child(FinalValues.SITUATIONS_COUNTER).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                situationsCounter = int.Parse(snapshot.GetValue(true).ToString());
            }
        });
    }

    private void GetUsersCounter()
    {
        reference.Child(FinalValues.USERS_COUNTER_DB_NAME).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                usersCounter = int.Parse(snapshot.GetValue(true).ToString());
            }
        });
    }

    public void GetSituationTitle()
    {
        reference.Child(FinalValues.SITUATIONS_DB_NAME).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                titleStr = snapshot.Child(currentSituationNumber + "").Child(FinalValues.SITUATION_TITLE_TEXT)
                    .GetValue(true).ToString();
            }
        });
    }

    private void LoadSituationsFromFirebase()
    {
        reference.Child(FinalValues.SITUATIONS_DB_NAME).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                situation = new Situation();
                string str = snapshot.Child(currentSituationNumber + "").Child(FinalValues.SITUATION_TITLE_TEXT).GetValue(true).ToString();
                situation.SetTitle(str);

                // Save the story
                partsOfStoryArr[0] = snapshot.Child(currentSituationNumber + "").Child(FinalValues.SITUATION_START_TEXT)
                    .GetValue(true).ToString();
                partsOfStoryArr[1] = snapshot.Child(currentSituationNumber + "").Child(FinalValues.SITUATION_INFO1)
                   .GetValue(true).ToString();
                partsOfStoryArr[2] = snapshot.Child(currentSituationNumber + "").Child(FinalValues.SITUATION_INFO2)
                   .GetValue(true).ToString();
                Debug.Log("Situation " + currentSituationNumber + ": " + partsOfStoryArr[0] + " "
                   + partsOfStoryArr[1] + " " + partsOfStoryArr[2]);
                situation.SetPartsOfTheStory(partsOfStoryArr);

                for (int j = 0; j < FinalValues.NUMBER_OF_QUESTIONS; j++)
                {
                    question = new Question();
                    // Save the questions and answers
                    question.SetQuestText(snapshot.Child(currentSituationNumber + "").Child(FinalValues.QUESTIONS_DB_NAME)
                        .Child(j + "").Child(FinalValues.THE_QUESTION_DB_NAME).GetValue(true).ToString());
                    Debug.Log("here2");

                    question.SetRightAnswer(snapshot.Child(currentSituationNumber + "").Child(FinalValues.QUESTIONS_DB_NAME)
                        .Child(j + "").Child(FinalValues.THE_RIGHT_ANSWER_TEXT).GetValue(true).ToString());
                    Debug.Log("here3");

                    answers = new string[FinalValues.NUMBER_OF_ANSWERS];
                    for (int k = 0; k < FinalValues.NUMBER_OF_ANSWERS; k++)
                        answers[k] = snapshot.Child(currentSituationNumber + "").Child(FinalValues.QUESTIONS_DB_NAME)
                        .Child(j + "").Child(FinalValues.ANSWERS_DB_NAME).Child(k + "").GetValue(true).ToString();

                    question.SetAnswers(answers);
                    questionsArr[j] = question;
                    Debug.Log(question.ToString());
                }

                situation.SetQuestions(questionsArr);
                Debug.Log(situation.GetPartsOfTheStory()[0]);
            }
        });
    }

    public void SetFirstPartOfStory()
    {
        new WaitForSeconds(3);
        Debug.Log("currentSituationNumber: " + currentSituationNumber);
        nextButton.gameObject.SetActive(true);
        startLevelCanvas.SetActive(false);
        storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
    }

    public void OnRestartButtonClick()
    {
        partOfStoryIndex = 0;
        storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
        backButton.gameObject.SetActive(false);
        storyObjects.SetActive(true);
        qusetionsObjects.SetActive(false);
    }

    public void GoBackToQuestButtonClick()
    {
        answerExplain.SetActive(false);
    }

    public void OnNextButtonClick()
    {
        switch (partOfStoryIndex)
        {
            case 0:
                partOfStoryIndex++;
                storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
                backButton.gameObject.SetActive(true);
                animatorController.SetInteger("partOfStory", partOfStoryIndex);
                break;
            case 1:
                partOfStoryIndex++;
                storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
                animatorController.SetInteger("partOfStory", partOfStoryIndex);
                break;
            case 2:
                startQuestionsObjects.SetActive(true);
                break;
        }
    }

    public void OnBackButtonClick()
    {
        switch (partOfStoryIndex)
        {
            case 1:
                backButton.gameObject.SetActive(false);
                partOfStoryIndex--;
                animatorController.SetInteger("partOfStory", partOfStoryIndex);
                storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
                break;
            case 2:
                backButton.gameObject.SetActive(true);
                partOfStoryIndex--;
                animatorController.SetInteger("partOfStory", partOfStoryIndex);
                storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
                break;
        }
    }

    public void OnStartQuestionsClick()
    {
        Debug.Log("QusetionsManeger Starts");
        qusetionsObjects.SetActive(true);
        answerExplain.SetActive(false);
        storyObjects.SetActive(false);
        startQuestionsObjects.SetActive(false);
        previousQuestArrow.gameObject.SetActive(false);
        QusetionsManeger();
    }  
    
    public void QusetionsManeger()
    {
        if (currentQuestionNumber < FinalValues.NUMBER_OF_QUESTIONS)
        {
            questionText.text = situation.GetQuestions()[currentQuestionNumber].GetQuestText();
            UploadAnswers();
        }

        if (currentQuestionNumber >= FinalValues.NUMBER_OF_QUESTIONS)
        {
            // TODO: FINISH THE LEVEL
            UpdateDatabasePlayerInfo();
        }
    }

    private void UpdateDatabasePlayerInfo()
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync("dana@gmail.com", "דנדושה").ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.ToString());

            }
            else if (task.IsCompleted)
            {
                currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
                Debug.Log("currentUser: " + currentUser.UserId);
            }
        });

        for (int i = 0; i < usersCounter; i++)
            reference.Child(FinalValues.USERS_DB_NAME).Child(i + "").Child(FinalValues.USER_ID_DB_NAME).EqualTo(currentUser.UserId)
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        // Handle the error...
                        Debug.Log("user : " + currentUser.Email + " Doesnt found. ");
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        Debug.Log("user: " + snapshot.Child("userNickname").GetValue(true).ToString() + " is found. ");
                    }
                });
    }

    public void NextSituation()
    {
        currentQuestionNumber = 0;
        currentSituationNumber++;
        answersObjects.SetActive(false);
        SetActiveAnswersObjects(false);
        storyObjects.SetActive(true);
        LoadSituationsFromFirebase();
    }

    public void UploadAnswers()
    {
        SetActiveAnswersObjects(true);

        answer1Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[0];
        answer2Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[1];
        answer3Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[2];
        answer4Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[3];

        switch (currentQuestionNumber)
        {
            case 0:
                answer1Image.GetComponent<Image>().sprite = answer1Sprite;
                answer2Image.GetComponent<Image>().sprite = answer2Sprite;
                answer3Image.GetComponent<Image>().sprite = answer3Sprite;
                answer4Image.GetComponent<Image>().sprite = answer4Sprite;
                break;
            case 1:
                Debug.Log("UploadAnswers Starts");
                answer1Image.GetComponent<Image>().sprite = answer7Sprite;
                answer2Image.GetComponent<Image>().sprite = answer9Sprite;
                answer3Image.GetComponent<Image>().sprite = answer4Sprite;
                answer4Image.GetComponent<Image>().sprite = answer3Sprite;
                Debug.Log("UploadAnswers ends");
                break;
            case 2:
                Debug.Log("UploadAnswers Starts");
                answer1Image.GetComponent<Image>().sprite = null;
                answer2Image.GetComponent<Image>().sprite = null;
                answer3Image.GetComponent<Image>().sprite = null;
                answer4Image.GetComponent<Image>().sprite = null;
                Debug.Log("UploadAnswers ends");
                break;
        }
    }

    private void SetActiveAnswersObjects(bool b)
    {
        answer1Image.gameObject.SetActive(b);
        answer2Image.gameObject.SetActive(b);
        answer3Image.gameObject.SetActive(b);
        answer4Image.gameObject.SetActive(b);
        answer1Text.gameObject.SetActive(b);
        answer2Text.gameObject.SetActive(b);
        answer3Text.gameObject.SetActive(b);
        answer4Text.gameObject.SetActive(b);
    }

    public IEnumerator IsRightAnswerClicked(string ansClicked)
    {
        Debug.Log("answerClicked: " + ansClicked);

        if (ansClicked.Equals(situation.GetQuestions()[currentQuestionNumber].GetRightAnswer()))
        {
            Debug.Log("Right");
            yield return new WaitForSeconds(1);

            answerExplain.SetActive(true);
            rightAnswerExplainText.gameObject.SetActive(true);
            worngAnswerExplainText.gameObject.SetActive(false);
            goBackToQuestButton.gameObject.SetActive(false);
            nextQuestionArrow.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Worng Answer");
            yield return new WaitForSeconds(1);

            answerExplain.SetActive(true);
            rightAnswerExplainText.gameObject.SetActive(false);
            worngAnswerExplainText.gameObject.SetActive(true);
            goBackToQuestButton.gameObject.SetActive(true);
            nextQuestionArrow.gameObject.SetActive(false);

            switch (ansClicked)
            {
                case "0":
                    answer1Image.gameObject.SetActive(false);
                    answer1Text.gameObject.SetActive(false);
                    break;
                case "1":
                    answer2Image.gameObject.SetActive(false);
                    answer2Text.gameObject.SetActive(false);
                    break;
                case "2":
                    answer3Image.gameObject.SetActive(false);
                    answer3Text.gameObject.SetActive(false);
                    break;
                case "3":
                    answer4Image.gameObject.SetActive(false);
                    answer4Text.gameObject.SetActive(false);
                    break;
            }
        }
    }
    
    public void NextQuestion()
    {
        answerExplain.SetActive(false);
        currentQuestionNumber++;
        Debug.Log("questionNumber " + currentQuestionNumber);
        QusetionsManeger();
        nextQuestionArrow.gameObject.SetActive(false);
    }

    public void PreviousQuestion()
    {
        currentQuestionNumber--;
        QusetionsManeger();
    }
}