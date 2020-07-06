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

    public TextMeshProUGUI startTXT;
    public TextMeshProUGUI moveTXT;
    public TextMeshProUGUI rotateTXT;
    public TextMeshProUGUI panelAndImageTXT;

    public Button nextBtn;

    private int maxMoveCoins;
    private int maxRotateCoins;


    private void Awake()
    {
        partsManager = GameObject.FindObjectOfType<PartsManager>();
    }

    void Start()
    {
        maxMoveCoins = partsManager.coins_MoveJoyStick.Length;
        maxRotateCoins = partsManager.coins_RotateJoyStick.Length;
        sceneStatus = SceneStatus.Start;
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
    }

    private void SceneStatusMoveJoyStick()
    {
        moveTXT.gameObject.SetActive(true);

        partsManager.moveJoyStick.SetActive(true);
        partsManager.moveJoyStickHand.gameObject.SetActive(true);

        if (maxMoveCoins < partsManager.coins_MoveJoyStick.Length)
        {
            partsManager.moveJoyStickHand.gameObject.SetActive(false);
        }

        if (maxMoveCoins == 0)
        {
            sceneStatus = SceneStatus.RotateJoyStick;
            moveTXT.gameObject.SetActive(false);
            partsManager.rotateJoyStickHand.gameObject.SetActive(true);
        }
    }

    private void SceneStatusRotateJoyStick()
    {
        partsManager.rotateJoyStick.SetActive(true);
        rotateTXT.gameObject.SetActive(true);

        if (maxRotateCoins < partsManager.coins_RotateJoyStick.Length)
        {
            partsManager.rotateJoyStickHand.gameObject.SetActive(false);
        }

        if (maxRotateCoins == 0)
        {
            sceneStatus = SceneStatus.PartsCreation;
            rotateTXT.gameObject.SetActive(false);
        }
    }

    private void SceneStatusPartsCreation()
    {
        panelAndImageTXT.gameObject.SetActive(true);

        partsManager.planeForPartsPos.SetActive(true);
        partsManager.buttonCreatorParts.gameObject.SetActive(true);
        managePartCreation.gameObject.SetActive(true);
    }

    public void RemoveCoinsFromArr()
    {
        switch (sceneStatus)
        {
            case SceneStatus.MoveJoyStick:
                maxMoveCoins--;
                break;

            case SceneStatus.RotateJoyStick:
                maxRotateCoins--;
                break;
        }
    }
}
