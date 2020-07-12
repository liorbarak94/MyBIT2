using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagePartCreation : MonoBehaviour
{
    public TutorialManagerScript tutorialManagerScript;
    public PartsManager partsManager;
    public GameManagerBuildScript gameManagerBuildScript;
    public CoinManager coinManager;

    [HideInInspector]
    public bool partInPlace;

    private Vector3 firstPos;
    private bool objectInPlace;
    private GameObject partToCreate;
    private GameObject fixed_PartToClone;

    private Vector3 tmpArrowPos;
    private Vector3 tmpCirclePos;

    public TextMeshProUGUI panelAndImageTXT;

    public TextMeshProUGUI partCreationTXT_1;
    public Button partCreationBtn_1;

    public TextMeshProUGUI partCreationTXT_2;
    public Button partCreationBtn_2;

    public TextMeshProUGUI partCreationTXT_3;

    public TextMeshProUGUI showSlowTouchmentTXT;

    public int arrowShiftHeight;

    public bool userDidTutorial;

    void Start()
    {
        objectInPlace = false;
        partInPlace = true;
        userDidTutorial = false;
        partsManager.currentPartIndex = 0;
    }

    void Update()
    {
        if (partInPlace && partsManager.currentPartIndex < partsManager.fixed_parts.Length)
        {
            if (partsManager.currentPartIndex > 0)
            {
                coinManager.AddCoin(partsManager.buttonCreatorParts.gameObject.transform.position);
                FindObjectOfType<AudioManager>().PlayAudio(FinalValues.COIN_AUDIO);
            }

            SetTheNextPart();

            partsManager.currentPartIndex++;
            partInPlace = false;
        }

        if (objectInPlace) 
        {
            objectInPlace = false;

            partsManager.finalWellDone.SetActive(true);

            partsManager.arrowForPartsPos.SetActive(false);
            partsManager.circleForPartsPos.SetActive(false);

            partsManager.buttonCreatorParts.gameObject.SetActive(false);

            StartCoroutine(FinisedTheGame());
        }
    }

    private IEnumerator FinisedTheGame()
    {
        yield return new WaitForSeconds(2f);
        gameManagerBuildScript.FinishedTheGame();
    }

    private void SetTheNextPart()
    {
        partsManager.colliders[partsManager.currentPartIndex].SetActive(true);
        partsManager.buttonCreatorParts.GetComponent<Image>().sprite =
            partsManager.parts_sprites[partsManager.currentPartIndex];
        partToCreate = partsManager.parts[partsManager.currentPartIndex];
        fixed_PartToClone = partsManager.fixed_parts[partsManager.currentPartIndex];
    }

    private void SetArropwAndCirclePos()
    {
        tmpArrowPos = new Vector3(
            fixed_PartToClone.transform.position.x,
            fixed_PartToClone.transform.position.y + arrowShiftHeight,
            fixed_PartToClone.transform.position.z);

        tmpCirclePos = new Vector3(
            fixed_PartToClone.transform.position.x,
            fixed_PartToClone.transform.position.y,
            fixed_PartToClone.transform.position.z);

        partsManager.arrowForPartsPos.SetActive(true);

        if (partsManager.arrowAnimBigOrSmall == 
            FinalValues.TypeOfArrowAnim.BIG)
        {
            partsManager.arrowForPartsPosAnim.SetBool(
                FinalValues.TypeOfArrowAnim.BIG.ToString(), true);
        }
        else if (partsManager.arrowAnimBigOrSmall ==
            FinalValues.TypeOfArrowAnim.SMALL)
        {
            partsManager.arrowForPartsPosAnim.SetBool(
                FinalValues.TypeOfArrowAnim.SMALL.ToString(), true);
        }

        partsManager.circleForPartsPos.SetActive(true);
        partsManager.arrowForPartsPos.transform.position = tmpArrowPos;
        partsManager.circleForPartsPos.transform.position = tmpCirclePos;
    }

    public void ButtonCreatorPartsWasPressed()
    {
        firstPos.x = partsManager.planeForPartsPos.transform.position.x;
        firstPos.y = fixed_PartToClone.transform.position.y;
        firstPos.z = partsManager.planeForPartsPos.transform.position.z;

        Instantiate(partToCreate);
        partToCreate.transform.position = firstPos;
        partToCreate.SetActive(true);

        partsManager.buttonCreatorParts.gameObject.SetActive(false);

        SetArropwAndCirclePos();

        if (userDidTutorial)
        {
            if (partsManager.currentPartIndex == 1)
            {
                panelAndImageTXT.gameObject.SetActive(false);
                partCreationTXT_1.gameObject.SetActive(true);
                partCreationBtn_1.gameObject.SetActive(true);
                gameManagerBuildScript.TimerActivation(true);

                tutorialManagerScript.gameObject.SetActive(false);
            }

            if (partsManager.currentPartIndex == 2)
            {
                partCreationTXT_3.gameObject.SetActive(false);
                showSlowTouchmentTXT.gameObject.SetActive(true);
            }

            if (partsManager.currentPartIndex >= 3)
            {
                partsManager.imageTutorialCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void PartCreationBtn_1_WasPressed()
    {
        partCreationTXT_1.gameObject.SetActive(false);
        partCreationBtn_1.gameObject.SetActive(false);

        partCreationTXT_2.gameObject.SetActive(true);
        partCreationBtn_2.gameObject.SetActive(true);
    }

    public void PartCreationBtn_2_WasPressed()
    {
        partCreationTXT_2.gameObject.SetActive(false);
        partCreationBtn_2.gameObject.SetActive(false);

        partCreationTXT_3.gameObject.SetActive(true);
    }

    public void SetPartInPlace()
    {
        partInPlace = true;
        fixed_PartToClone.SetActive(true);
        partsManager.buttonCreatorParts.gameObject.SetActive(true);
        partsManager.arrowForPartsPos.SetActive(false);
        partsManager.circleForPartsPos.SetActive(false);

        if (partsManager.currentPartIndex >= partsManager.fixed_parts.Length)
        {
            objectInPlace = true;
        }
    }    
}
