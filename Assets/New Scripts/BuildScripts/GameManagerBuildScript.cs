using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;

public class GameManagerBuildScript : MonoBehaviour
{
    private DatabaseReference reference;

    public PartsManager partsManager;
    public TouchManager touchManager;

    public TutorialManagerScript tutorialManagerScript;
    public ManagePartCreation managePartCreation;

    public Image largeImageIntroCanvas;
    public Image smallImageCanvas;

    public TextMeshProUGUI introTXTMeshPro;
    public Button nextBtn;

    public TextMeshProUGUI tutorialTXT;
    public Button v;
    public Button x;

    public TextMeshProUGUI missionTXT;
    public Button missionNextBtn;

    public Image pigMoneyImage;
    public Animator pigAnimator;

    public TextMeshProUGUI finishedTheGameTXT;
    public Button finishedTheGameBtn;

    public TextMeshProUGUI timerTXT;
    public float timer;
    public float currentTimer;
    public bool timerIsRunning;

    public TextMeshProUGUI timeRunOutTXT;
    public Button timeRunOut_Exit;
    public Button timeRunOut_Refresh;

    public TextMeshProUGUI waitForSecondTouchOnPartTXT;
    public float waitForSecondTouchOnPart_Time;

    public bool menuIsOpen;
    public Button menuBtn;
    public Button[] allBtnInMenu;

    public int averageNumberOfTouches;
    public int userIndex;
    public int levelIndex;

    private void Awake()
    {
        partsManager = GameObject.FindObjectOfType<PartsManager>();
    }

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FinalValues.FIREBASE_URL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        userIndex = PlayerPrefs.GetInt(
            FinalValues.MYBIT_GAME_USER_INDEX_PLAYER_PREFS_NAME,
            0);

        levelIndex = PlayerPrefs.GetInt(
            FinalValues.MYBIT_GAME_USER_CURRENT_LEVEL_INDEX_PLAYER_PREFS_NAME,
            0);

        DeActiveAllGameObjectsAtStart();
        HideAllObjectPartsAndItsClliders();
        GetTimerFromPlayerPrefs();

        largeImageIntroCanvas.gameObject.SetActive(true);
        introTXTMeshPro.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);

        menuIsOpen = false;
        TimerActivation(false);
        timer = 0;
        waitForSecondTouchOnPart_Time = 0;
    }

    private void DeActiveAllGameObjectsAtStart()
    {
        tutorialManagerScript.gameObject.SetActive(false);
        managePartCreation.gameObject.SetActive(false);

        partsManager.imageTutorialCanvas.gameObject.SetActive(false);

        partsManager.moveJoyStick.gameObject.SetActive(false);
        partsManager.moveJoyStickHand.gameObject.SetActive(false);

        partsManager.rotateJoyStick.gameObject.SetActive(false);
        partsManager.rotateJoyStickHand.gameObject.SetActive(false);

        partsManager.buttonCreatorParts.gameObject.SetActive(false);
        partsManager.fingerIndicationCanvas.gameObject.SetActive(false);

        partsManager.planeForPartsPos.gameObject.SetActive(false);
        partsManager.arrowForPartsPos.gameObject.SetActive(false);
        partsManager.circleForPartsPos.gameObject.SetActive(false);
        partsManager.finalWellDone.gameObject.SetActive(false);

        tutorialTXT.gameObject.SetActive(false);
        v.gameObject.SetActive(false);
        x.gameObject.SetActive(false);

        missionTXT.gameObject.SetActive(false);
        missionNextBtn.gameObject.SetActive(false);
    }

    private void HideAllObjectPartsAndItsClliders()
    {
        for (int i = 0; i < partsManager.parts.Length; i++)
        {
            partsManager.fixed_parts[i].SetActive(false);
        }
        for (int i = 0; i < partsManager.colliders.Length; i++)
        {
            partsManager.colliders[i].SetActive(false);
        }
    }

    public void GetTimerFromPlayerPrefs()
    {
        currentTimer = PlayerPrefs.GetFloat(
            FinalValues.CURRENT_TIMER_LEVEL_PLAYER_PREFS_NAME, 3);        
        currentTimer *= 60;
        timer = 0;
    }

    void Update()
    {
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
                TimeRunOut_Activation();
            }
        }
    }

    public void TimerActivation(bool toRunTimer)
    {
        timerIsRunning = toRunTimer;
    }

    public string DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PressedIntroNextBtn()
    {
        introTXTMeshPro.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);

        tutorialTXT.gameObject.SetActive(true);

        v.gameObject.SetActive(true);
        x.gameObject.SetActive(true);
    }

    public void Pressed_V()
    {
        tutorialManagerScript.gameObject.SetActive(true);
        partsManager.imageTutorialCanvas.gameObject.SetActive(true);
        partsManager.fingerIndicationCanvas.gameObject.SetActive(true);

        largeImageIntroCanvas.gameObject.SetActive(false);

        tutorialTXT.gameObject.SetActive(false);
        v.gameObject.SetActive(false);
        x.gameObject.SetActive(false);
    }

    public void Pressed_X()
    {
        tutorialTXT.gameObject.SetActive(false);
        v.gameObject.SetActive(false);
        x.gameObject.SetActive(false);

        missionTXT.gameObject.SetActive(true);
        missionNextBtn.gameObject.SetActive(true);
    }

    public void StartTheGame()
    {
        missionTXT.gameObject.SetActive(false);
        missionNextBtn.gameObject.SetActive(false);

        partsManager.moveJoyStick.gameObject.SetActive(true);
        partsManager.rotateJoyStick.gameObject.SetActive(true);

        partsManager.buttonCreatorParts.gameObject.SetActive(true);
        partsManager.planeForPartsPos.gameObject.SetActive(true);
        partsManager.fingerIndicationCanvas.gameObject.SetActive(true);

        tutorialTXT.gameObject.SetActive(false);
        v.gameObject.SetActive(false);
        x.gameObject.SetActive(false);

        largeImageIntroCanvas.gameObject.SetActive(false);

        managePartCreation.gameObject.SetActive(true);
    }

    public void MenuBtnWasPressed()
    {
        if (menuIsOpen)
        {
            menuBtn.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            HideAllIconsInMenu();
        }
        else
        {
            menuBtn.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            ShowAllIconsInMenu();
        }
    }

    public void ShowAllIconsInMenu()
    {
        float maxTime = 4;

        foreach (Button btn in allBtnInMenu)
        {
            float time = 0;

            while (time < maxTime)
            {
                time += Time.deltaTime;
            }
            btn.gameObject.SetActive(true);
        }
        menuIsOpen = true;
    }

    public void HideAllIconsInMenu()
    {
        float maxTime = 4;

        for (int i = allBtnInMenu.Length - 1; i >= 0; i--)
        {
            float time = 0;

            while (time < maxTime)
            {
                time += Time.deltaTime;
            }
            allBtnInMenu[i].gameObject.SetActive(false);
        }
        menuIsOpen = false;
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(FinalValues.MAIN_MENU_SCENE_INDEX);
    }

    public void RefreshThatScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FinishedTheGame()
    {
        FindObjectOfType<AudioManager>().PlayAudio(FinalValues.VICTORY_AUDIO);

        TimerActivation(false);

        largeImageIntroCanvas.gameObject.SetActive(true);
        finishedTheGameTXT.gameObject.SetActive(true);
        finishedTheGameBtn.gameObject.SetActive(true);

        string tmpTimerString = DisplayTime(timer);
        string tmp = "";
        for (int i = tmpTimerString.Length - 1; i >= 0; i--)
        {
            tmp += tmpTimerString[i];
        }

        for (int i = 0; i < touchManager.partsCounterTouches.Length; i++)
        {
            if (touchManager.partsCounterTouches[i] == 0)
            {
                touchManager.partsCounterTouches[i] = 1;
            }
            averageNumberOfTouches += touchManager.partsCounterTouches[i];
        }

        averageNumberOfTouches = (int)(averageNumberOfTouches / touchManager.partsCounterTouches.Length);

        finishedTheGameTXT.text = "כל הכבוד !!! \n\n";
        finishedTheGameTXT.text += "סיימתם את השלב תוך: " + tmp + "\n\n";
        finishedTheGameTXT.text += "עם ממוצע נגיעות בחלק: " + averageNumberOfTouches + "\n\n";

        SaveUserDetailsToDBAfterBuildLevel();
    }

    public void TimeRunOut_Activation()
    {
        largeImageIntroCanvas.gameObject.SetActive(true);
        timeRunOutTXT.gameObject.SetActive(true);
        timeRunOut_Exit.gameObject.SetActive(true);
        timeRunOut_Refresh.gameObject.SetActive(true);     
    }

    public void WaitForSecondTouchOfPart_Activation(bool toShowTXT)
    {
        smallImageCanvas.gameObject.SetActive(toShowTXT);
        waitForSecondTouchOnPartTXT.gameObject.SetActive(toShowTXT);
    }

    public void SaveUserDetailsToDBAfterBuildLevel()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(FinalValues.USERS_DB_NAME)
            .GetValueAsync()
            .ContinueWith(task => {

                if (task.IsFaulted)
                {
                    Debug.Log("SaveUserDetailsToDBAfterLevel IsFaulted");
                }

                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    int timesTheLevelWasPlayed = int.Parse(snapshot.Child(FinalValues.TIMES_THE_LEVEL_WAS_PLAYED_DB_NAME)
                        .GetValue(true).ToString());

                    timesTheLevelWasPlayed++;
                    reference.Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.TIMES_THE_LEVEL_WAS_PLAYED_DB_NAME).SetValueAsync(timesTheLevelWasPlayed);

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_NUMBER_OF_MISTAKES_OR_AVERAGE_NUMBER_OF_TOUCHES_DB_NAME)
                        .SetValueAsync(averageNumberOfTouches);

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_IS_USER_DID_THE_LEVEL_DB_NAME)
                        .SetValueAsync(true);

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.LEVELS_DB_NAME)
                        .Child(FinalValues.BUILD_LEVELS_DB_NAME)
                        .Child(levelIndex + "")
                        .Child(FinalValues.LEVEL_TOTAL_TIME_DB_NAME)
                        .SetValueAsync(timer);

                    levelIndex++;
                    reference
                       .Child(FinalValues.USERS_DB_NAME)
                       .Child(userIndex + "")
                       .Child(FinalValues.USER_CURRENT_BUILD_LEVEL_DB_NAME)
                       .SetValueAsync(levelIndex);

                    reference
                        .Child(FinalValues.USERS_DB_NAME)
                        .Child(userIndex + "")
                        .Child(FinalValues.TOTAL_BUILD_LEVELS_PLAYED_DB_NAME)
                        .SetValueAsync(levelIndex);

                    Debug.LogFormat(
                        "Saved User Details To DB After Finished Level Successfully");
                }
            });
    }
}
