using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManagerScript : MonoBehaviour
{
    private enum SceneStatus { None, MoveJoyStick, RotateJoyStick, PartsCreation }
    private SceneStatus sceneStatus;

    public ManagePartCreation managePartCreation;
    public PartsManager partsManager;

    public TextMeshProUGUI moveTXT_1;
    public Button moveBtn_1;
    public TextMeshProUGUI moveTXT_2;

    public TextMeshProUGUI rotateTXT_1;
    public Button rotateBtn_1;
    public TextMeshProUGUI rotateTXT_2;

    public TextMeshProUGUI panelAndImageTXT;

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
        sceneStatus = SceneStatus.None;
    }

    void Update()
    {
        switch (sceneStatus)
        {
            case SceneStatus.None:
                SceneStatus_Move1();
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

    public void SceneStatus_Move1()
    {
        moveTXT_1.gameObject.SetActive(true);
        moveBtn_1.gameObject.SetActive(true);
    }

    public void SceneStatus_Move2()
    {
        moveTXT_1.gameObject.SetActive(false);
        moveBtn_1.gameObject.SetActive(false);

        moveTXT_2.gameObject.SetActive(true);
        sceneStatus = SceneStatus.MoveJoyStick;
    }

    public void SceneStatusMoveJoyStick()
    {
        partsManager.moveJoyStick.SetActive(true);
        partsManager.moveJoyStickHand.gameObject.SetActive(true);

        if (maxMoveCoins < partsManager.coins_MoveJoyStick.Length)
        {
            partsManager.moveJoyStickHand.gameObject.SetActive(false);
        }

        if (maxMoveCoins == 0)
        {
            moveTXT_2.gameObject.SetActive(false);
            SceneStatus_Rotate1();
        }
    }

    public void SceneStatus_Rotate1()
    {
        rotateTXT_1.gameObject.SetActive(true);
        rotateBtn_1.gameObject.SetActive(true);
    }

    public void SceneStatus_Rotate2()
    {
        rotateTXT_1.gameObject.SetActive(false);
        rotateBtn_1.gameObject.SetActive(false);

        rotateTXT_2.gameObject.SetActive(true);

        partsManager.rotateJoyStickHand.gameObject.SetActive(true);

        sceneStatus = SceneStatus.RotateJoyStick;
    }

    public void SceneStatusRotateJoyStick()
    {
        partsManager.rotateJoyStick.SetActive(true);

        if (maxRotateCoins < partsManager.coins_RotateJoyStick.Length)
        {
            partsManager.rotateJoyStickHand.gameObject.SetActive(false);
        }

        if (maxRotateCoins == 0)
        {
            sceneStatus = SceneStatus.PartsCreation;
            rotateTXT_2.gameObject.SetActive(false);
        }
    }

    public void SceneStatusPartsCreation()
    {
        panelAndImageTXT.gameObject.SetActive(true);

        partsManager.planeForPartsPos.SetActive(true);
        partsManager.buttonCreatorParts.gameObject.SetActive(true);

        managePartCreation.gameObject.SetActive(true);
        managePartCreation.userDidTutorial = true;
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
