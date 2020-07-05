using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManagerScript : MonoBehaviour
{
    private enum SceneStatus { Start, MoveJoyStick, RotateJoyStick, PartsCreation }
    private SceneStatus sceneStatus;

    public ManagePartCreation managePartCreation;
    public PartsManager partsManager;

    public Animator handAnimMove;
    public Animator handAnimRotate;

    public TextMeshProUGUI startTXT;
    public TextMeshProUGUI moveTXT;
    public TextMeshProUGUI rotateTXT;
    public TextMeshProUGUI panelAndImageTXT;

    public Button nextBtn;

    private bool showPartsImage;

    private int currentCoinsMove;
    private bool showMoveCoins;

    private int currentCoinsRotate;
    private bool showRotateCoins;

    public GameObject moveJoyStick;
    public GameObject rotateJoyStick;


    private void Awake()
    {
        partsManager = GameObject.FindObjectOfType<PartsManager>();
    }

    void Start()
    {
        sceneStatus = SceneStatus.Start;

        HideCoins(partsManager.coins_MoveJoyStick);
        currentCoinsMove = 0;
        showMoveCoins = true;
        showPartsImage = true;

        HideCoins(partsManager.coins_RotateJoyStick);
        currentCoinsRotate = 0;
        showRotateCoins = true;
    }

    void Update()
    {
        switch (sceneStatus)
        {
            case SceneStatus.Start:
                SceneStatusStart();
                break;

            case SceneStatus.MoveJoyStick:
                SceneStatusMoveJoyStick();
                break;

            case SceneStatus.RotateJoyStick:
                SceneStatusRotateJoyStick();
                break;

            case SceneStatus.PartsCreation:
                SceneStatusPartsCreation();
                break;
        }
    }

    private void SceneStatusStart()
    {
        startTXT.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);
    }

    public void StartMoveCoins()
    {
        startTXT.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        sceneStatus = SceneStatus.MoveJoyStick;
        showMoveCoins = true;
    }

    private void SceneStatusMoveJoyStick()
    {
        moveTXT.gameObject.SetActive(true);

        handAnimMove.gameObject.SetActive(true);
        moveJoyStick.SetActive(true);

        if (showMoveCoins)
        {
            ShowCoins(partsManager.coins_MoveJoyStick);
            showMoveCoins = false;
        }

        if (currentCoinsMove >= 1)
        {
            handAnimMove.gameObject.SetActive(false);
        }

        if (currentCoinsMove == partsManager.coins_MoveJoyStick.Length)
        {
            sceneStatus = SceneStatus.RotateJoyStick;
            moveTXT.gameObject.SetActive(false);
            showRotateCoins = true;
        }
    }

    
    private void SceneStatusRotateJoyStick()
    {
        rotateJoyStick.SetActive(true);
        rotateTXT.gameObject.SetActive(true);

        if (showRotateCoins)
        {
            handAnimRotate.gameObject.SetActive(true);
            ShowCoins(partsManager.coins_RotateJoyStick);
            showRotateCoins = false;
        }

        if (currentCoinsRotate >= 1)
        {
            handAnimRotate.gameObject.SetActive(false);
        }

        if (currentCoinsRotate == partsManager.coins_RotateJoyStick.Length)
        {
            sceneStatus = SceneStatus.PartsCreation;
            showPartsImage = true;
            rotateTXT.gameObject.SetActive(false);
        }
    }

    private void SceneStatusPartsCreation()
    {
        panelAndImageTXT.gameObject.SetActive(true);

        if (showPartsImage)
        {
            partsManager.planeForPartsPos.SetActive(true);
            showPartsImage = false;
            partsManager.buttonCreatorParts.gameObject.SetActive(true);
            managePartCreation.gameObject.SetActive(true);
        }
    }

    private void HideCoins(GameObject[] coins)
    {
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].SetActive(false);
        }
    }

    private void ShowCoins(GameObject[] coins)
    {
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].SetActive(true);
        }
    }

    public void RemoveCoin()
    {
        if (sceneStatus == SceneStatus.MoveJoyStick)
        {
            currentCoinsMove++;
        }
        else if (sceneStatus == SceneStatus.RotateJoyStick)
        {
            currentCoinsRotate++;
        }
    }
}
