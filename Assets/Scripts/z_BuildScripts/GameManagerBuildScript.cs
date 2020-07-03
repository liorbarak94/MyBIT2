using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerBuildScript : MonoBehaviour
{
    public PartsManager partsManager;

    public GameObject moveJoyStick;
    public GameObject moveJoyStickHand;

    public GameObject rotateJoyStick;
    public GameObject rotateJoyStickHand;

    public TutorialManagerScript tutorialManagerScript;
    public ManagePartCreation managePartCreation;

    public TextMeshProUGUI timerTXT;
    public float timer;
    public bool timerIsRunning;

    public TextMeshProUGUI introTXTMeshPro;
    public Button nextBtn;

    public TextMeshProUGUI tutorialTXT;
    public Button v;
    public Button x;

    public Image imageIntroCanvas;

    public TextMeshProUGUI missionTXT;
    public Button missionNextBtn;

    public Image pigMoneyImage;
    public Animator pigAnimator;

    public TextMeshProUGUI finishedTheGameTXT;
    public Button finishedTheGameBtn;

    public bool menuIsOpen;
    public Button menuBtn;
    public Button[] allBtnInMenu;

    private void Awake()
    {
        partsManager = GameObject.FindObjectOfType<PartsManager>();
    }

    void Start()
    {
        DeActiveAllGameObjectsAtStart();
        HideAllObjectPartsAndItsClliders();
        GetTimerFromPlayerPrefs();

        imageIntroCanvas.gameObject.SetActive(true);
        introTXTMeshPro.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);
        menuIsOpen = false;
        timerIsRunning = false;
    }

    private void DeActiveAllGameObjectsAtStart()
    {
        partsManager.imageTutorialCanvas.gameObject.SetActive(false);

        moveJoyStick.gameObject.SetActive(false);
        moveJoyStickHand.gameObject.SetActive(false);

        rotateJoyStick.gameObject.SetActive(false);
        rotateJoyStickHand.gameObject.SetActive(false);

        partsManager.buttonCreatorParts.gameObject.SetActive(false);
        partsManager.fingerIndicationCanvas.gameObject.SetActive(false);

        partsManager.planeForPartsPos.gameObject.SetActive(false);
        partsManager.arrowForPartsPos.gameObject.SetActive(false);
        partsManager.circleForPartsPos.gameObject.SetActive(false);
        partsManager.finalWellDone.gameObject.SetActive(false);

        tutorialManagerScript.gameObject.SetActive(false);
        managePartCreation.gameObject.SetActive(false);

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
        timer = PlayerPrefs.GetFloat(
            FinalValues.CURRENT_TIMER_BUILD_LEVEL_PLAYER_PREFS_NAME, 5);
        timer *= 60;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                DisplayTime(timer);
                if (timer < 30)
                {
                    timerTXT.color = Color.red;
                }
            }
            else
            {
                Debug.Log("Time has run out!");
                timer = 0;
                timerIsRunning = false;
                timerTXT.text = "";
            }
        }
    }

    public void TimerActivation(bool toRunTimer)
    {
        timerIsRunning = toRunTimer;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerTXT.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        imageIntroCanvas.gameObject.SetActive(false);

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

        moveJoyStick.gameObject.SetActive(true);
        moveJoyStickHand.gameObject.SetActive(false);

        rotateJoyStick.gameObject.SetActive(true);
        rotateJoyStickHand.gameObject.SetActive(false);

        partsManager.buttonCreatorParts.gameObject.SetActive(true);
        partsManager.planeForPartsPos.gameObject.SetActive(true);

        tutorialTXT.gameObject.SetActive(false);
        v.gameObject.SetActive(false);
        x.gameObject.SetActive(false);

        imageIntroCanvas.gameObject.SetActive(false);

        managePartCreation.gameObject.SetActive(true);
    }

    public void FinishedTheGame()
    {
        imageIntroCanvas.gameObject.SetActive(true);
        finishedTheGameTXT.gameObject.SetActive(true);
        finishedTheGameBtn.gameObject.SetActive(true);
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

    public void GoBackToMainManue()
    {
        SceneManager.LoadScene(FinalValues.MAIN_MANU_SCENE_INDEX);
    }
}
