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
    private int situationsCounter, partOfStoryIndex, currentSituationLevel, currentQuestionNumber, numberOfMistekes,
        currentUserIndex;
    private string[] partsOfStoryArr = new string[FinalValues.STORY_SIZE];
    private Situation situation;
    private Question[] questionsArr;
    private string[] answers;

    public TMP_Text storyText, titleText, rightAnswerExplainText, worngAnswerExplainText,
        questionText, answer1Text, answer2Text, answer3Text, answer4Text;
    public Image backButton, nextButton, restartButton, startButton, goBackToQuestButton, menuButton, pauseButton,
        exitButton, pigyButton;

    private bool isLoaded = false, menuIsOpen = false;

    //public Animator animatorController;
    public GameObject imagesSwap, storyObjects, qusetionsObjects, startQuestionsObjects, startLevelCanvas,
        answersObjects, answerExplain, menuImageList;
    public Sprite[] story1Images = new Sprite[FinalValues.STORY_SIZE];
    public Sprite[] story2Images = new Sprite[FinalValues.STORY_SIZE];
    public Sprite[] answers1Images = new Sprite[FinalValues.NUMBER_OF_ANSWERS * FinalValues.NUMBER_OF_QUESTIONS];
    public Sprite[] answers2Images = new Sprite[FinalValues.NUMBER_OF_ANSWERS * FinalValues.NUMBER_OF_QUESTIONS];
    public Image answer1Image, answer2Image, answer3Image, answer4Image;
    public Image previousQuestArrow, nextQuestionArrow;
    public Image returnToStory;
    public Image backToQuestions;

    public TMP_Text timerTXT;
    public float timer, currentTimer;
    public bool timerIsRunning;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        numberOfMistekes = 0;
        //animatorController = imagesSwap.GetComponent<Animator>();
        currentUserIndex = PlayerPrefs.GetInt(FinalValues.MYBIT_GAME_USER_INDEX_PLAYER_PREFS_NAME, 0);
        Debug.Log("currentUserIndex: " + currentUserIndex);
        GetSituationCounter();
        GetTimerFromPlayerPrefs();
        GetCurrentSituationLevelFromPlayerPrefs();
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

        if (timerIsRunning)
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                timer += Time.deltaTime;

                timerTXT.text = DisplayTime(currentTimer);
                if (currentTimer < 30)
                {
                    timerTXT.color = Color.red;
                }
            }
            else
            {
                Debug.Log("Time has run out!");
                currentTimer = 0;
                TimerActivation(false);
                timerTXT.text = "";
                // TODO: SHOW THE RIGHT ANSWER AND EXPLAINE
            }
        }
    }

    public void TimerActivation(bool toRunTimer)
    {
        timerIsRunning = toRunTimer;
    }

    private string DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
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

    private void GetCurrentSituationLevelFromPlayerPrefs()
    {
        currentSituationLevel = 1;
        //currentSituationLevel = PlayerPrefs.GetInt(FinalValues.MYBIT_GAME_USER_CURRENT_LEVEL_INDEX_PLAYER_PREFS_NAME, 0);
        partOfStoryIndex = 0;
        currentQuestionNumber = 0;
        Debug.Log("currentSituationNumber: " + currentSituationLevel);
    }

    public void GetTimerFromPlayerPrefs()
    {
        currentTimer = PlayerPrefs.GetFloat(FinalValues.CURRENT_TIMER_LEVEL_PLAYER_PREFS_NAME, 3);
        currentTimer *= 60;
        timer = 0;
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
                titleStr = snapshot.Child(currentSituationLevel + "").Child(FinalValues.SITUATION_TITLE_TEXT)
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

                questionsArr = new Question[FinalValues.NUMBER_OF_QUESTIONS];
                situation = new Situation();

                string str = snapshot.Child(currentSituationLevel + "").Child(FinalValues.SITUATION_TITLE_TEXT).GetValue(true).ToString();
                situation.SetTitle(str);

                // Save the story
                partsOfStoryArr[0] = snapshot.Child(currentSituationLevel + "").Child(FinalValues.SITUATION_START_TEXT)
                    .GetValue(true).ToString();
                partsOfStoryArr[1] = snapshot.Child(currentSituationLevel + "").Child(FinalValues.SITUATION_INFO1)
                   .GetValue(true).ToString();
                partsOfStoryArr[2] = snapshot.Child(currentSituationLevel + "").Child(FinalValues.SITUATION_INFO2)
                   .GetValue(true).ToString();
                Debug.Log("Situation " + currentSituationLevel + ": " + partsOfStoryArr[0] + " "
                   + partsOfStoryArr[1] + " " + partsOfStoryArr[2]);
                situation.SetPartsOfTheStory(partsOfStoryArr);

                for (int j = 0; j < FinalValues.NUMBER_OF_QUESTIONS; j++)
                {
                    Question question = new Question();
                    // Save the questions and answers
                    question.SetQuestText(snapshot.Child(currentSituationLevel + "").Child(FinalValues.QUESTIONS_DB_NAME)
                        .Child(j + "").Child(FinalValues.THE_QUESTION_DB_NAME).GetValue(true).ToString());
                    Debug.Log("here2");

                    question.SetRightAnswer(snapshot.Child(currentSituationLevel + "").Child(FinalValues.QUESTIONS_DB_NAME)
                        .Child(j + "").Child(FinalValues.THE_RIGHT_ANSWER_TEXT).GetValue(true).ToString());
                    Debug.Log("here3");

                    answers = new string[FinalValues.NUMBER_OF_ANSWERS];
                    for (int k = 0; k < FinalValues.NUMBER_OF_ANSWERS; k++)
                        answers[k] = snapshot.Child(currentSituationLevel + "").Child(FinalValues.QUESTIONS_DB_NAME)
                        .Child(j + "").Child(FinalValues.ANSWERS_DB_NAME).Child(k + "").GetValue(true).ToString();

                    question.SetAnswers(answers);
                    questionsArr[j] = question;
                    Debug.Log(question.GetQuestText());
                }

                situation.SetQuestions(questionsArr);
                Debug.Log(situation.GetPartsOfTheStory()[0]);
            }
        });
    }

    public void SetFirstPartOfStory()
    {
        new WaitForSeconds(3);
        Debug.Log("currentSituationNumber: " + currentSituationLevel);
        nextButton.gameObject.SetActive(true);
        startLevelCanvas.SetActive(false);
        if (currentSituationLevel == 0)
            imagesSwap.GetComponent<Image>().sprite = story1Images[partOfStoryIndex];
        //animatorController.SetInteger("partOfStory", partOfStoryIndex);
        if (currentSituationLevel == 1)
            imagesSwap.GetComponent<Image>().sprite = story2Images[partOfStoryIndex];
        storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
        TimerActivation(true);
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
                if (currentSituationLevel == 0)
                    imagesSwap.GetComponent<Image>().sprite = story1Images[partOfStoryIndex];
                //animatorController.SetInteger("partOfStory", partOfStoryIndex);
                if (currentSituationLevel == 1)
                    imagesSwap.GetComponent<Image>().sprite = story2Images[partOfStoryIndex];
                break;
            case 1:
                partOfStoryIndex++;
                storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
                if (currentSituationLevel == 0)
                    imagesSwap.GetComponent<Image>().sprite = story1Images[partOfStoryIndex];
                //animatorController.SetInteger("partOfStory", partOfStoryIndex);
                if (currentSituationLevel == 1)
                    imagesSwap.GetComponent<Image>().sprite = story2Images[partOfStoryIndex];
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
                if (currentSituationLevel == 0)
                    imagesSwap.GetComponent<Image>().sprite = story1Images[partOfStoryIndex];
                //animatorController.SetInteger("partOfStory", partOfStoryIndex);
                if (currentSituationLevel == 1)
                    imagesSwap.GetComponent<Image>().sprite = story2Images[partOfStoryIndex];
                storyText.text = situation.GetPartsOfTheStory()[partOfStoryIndex];
                break;
            case 2:
                backButton.gameObject.SetActive(true);
                partOfStoryIndex--;
                if (currentSituationLevel == 0)
                    imagesSwap.GetComponent<Image>().sprite = story1Images[partOfStoryIndex];
                //animatorController.SetInteger("partOfStory", partOfStoryIndex);
                if (currentSituationLevel == 1)
                    imagesSwap.GetComponent<Image>().sprite = story2Images[partOfStoryIndex];
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
            // FINISH THE LEVEL
            UpdateDatabasePlayerInfo();
        }
    }

    private void UpdateDatabasePlayerInfo()
    {
        DatabaseReference databaseReferenceForUpdate = reference.Child(FinalValues.USERS_DB_NAME)
            .Child(currentUserIndex + "").Child(FinalValues.LEVELS_DB_NAME).Child(FinalValues.SITUATION_LEVELS_DB_NAME)
            .Child(currentSituationLevel + "");

        databaseReferenceForUpdate.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("UpdateDatabasePlayerInfo: IsFaulted");
            }

            if (task.IsCompleted)
            {
                databaseReferenceForUpdate.Child(FinalValues.LEVEL_TOTAL_TIME_DB_NAME).SetValueAsync(timer);

                databaseReferenceForUpdate.Child(FinalValues.LEVEL_NUMBER_OF_MISTAKES_OR_AVERAGE_NUMBER_OF_TOUCHES_DB_NAME)
                    .SetValueAsync(numberOfMistekes);

                databaseReferenceForUpdate.Child(FinalValues.LEVEL_IS_USER_DID_THE_LEVEL_DB_NAME).SetValueAsync(true);

                currentSituationLevel++;
                reference.Child(FinalValues.USERS_DB_NAME).Child(currentUserIndex + "")
                    .Child(FinalValues.USER_CURRENT_SITUATION_LEVEL_DB_NAME).SetValueAsync(currentSituationLevel);

                Debug.LogFormat("Saved User Details To DB After Finished Level Successfully");
            }
        });
    }

    public void NextSituation()
    {
        currentQuestionNumber = 0;
        currentSituationLevel++;
        answersObjects.SetActive(false);
        SetActiveAnswersObjects(false);
        storyObjects.SetActive(true);
        LoadSituationsFromFirebase();
    }

    public void UploadAnswers()
    {
        Debug.Log("UploadAnswers Starts");

        SetActiveAnswersObjects(true);

        answer1Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[0];
        answer2Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[1];
        answer3Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[2];
        answer4Text.text = situation.GetQuestions()[currentQuestionNumber].GetAnswers()[3];

        switch (currentQuestionNumber)
        {
            case 0:
                if (currentSituationLevel == 0)
                {
                    answer1Image.GetComponent<Image>().sprite = answers1Images[0];
                    answer2Image.GetComponent<Image>().sprite = answers1Images[1];
                    answer3Image.GetComponent<Image>().sprite = answers1Images[2];
                    answer4Image.GetComponent<Image>().sprite = answers1Images[3];
                }
                if (currentSituationLevel == 1)
                {
                    answer1Image.GetComponent<Image>().sprite = answers2Images[0];
                    answer2Image.GetComponent<Image>().sprite = answers2Images[1];
                    answer3Image.GetComponent<Image>().sprite = answers2Images[2];
                    answer4Image.GetComponent<Image>().sprite = answers2Images[3];
                }
                break;
            case 1:
                if (currentSituationLevel == 0)
                {
                    answer1Image.GetComponent<Image>().sprite = answers1Images[4];
                    answer2Image.GetComponent<Image>().sprite = answers1Images[5];
                    answer3Image.GetComponent<Image>().sprite = answers1Images[6];
                    answer4Image.GetComponent<Image>().sprite = answers1Images[7];
                }
                if (currentSituationLevel == 1)
                {
                    answer1Image.GetComponent<Image>().sprite = answers2Images[4];
                    answer2Image.GetComponent<Image>().sprite = answers2Images[5];
                    answer3Image.GetComponent<Image>().sprite = answers2Images[6];
                    answer4Image.GetComponent<Image>().sprite = answers2Images[7];
                }
                break;
            case 2:
                if (currentSituationLevel == 0)
                {
                    answer1Image.GetComponent<Image>().sprite = answers1Images[8];
                    answer2Image.GetComponent<Image>().sprite = answers1Images[9];
                    answer3Image.GetComponent<Image>().sprite = answers1Images[10];
                    answer4Image.GetComponent<Image>().sprite = answers1Images[11];
                }
                if (currentSituationLevel == 1)
                {
                    answer1Image.GetComponent<Image>().sprite = answers2Images[8];
                    answer2Image.GetComponent<Image>().sprite = answers2Images[9];
                    answer3Image.GetComponent<Image>().sprite = answers2Images[10];
                    answer4Image.GetComponent<Image>().sprite = answers2Images[11];
                }
                break;
        }
        Debug.Log("UploadAnswers ends");

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
            numberOfMistekes++;
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

    public void MenuBtnWasPressed()
    {
        if (menuIsOpen)
        {
            menuButton.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            menuImageList.gameObject.SetActive(false);
            menuIsOpen = false;
        }
        else
        {
            menuButton.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            menuImageList.gameObject.SetActive(true);
            menuIsOpen = true;
        }
    }
}