using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagePartCreation : MonoBehaviour
{
    public TutorialManagerScript tutorialManagerScript;
    public PartsManager partsManager;
    public GameManagerBuildScript gameManagerBuildScript;

    [HideInInspector]
    public bool partInPlace;

    private Vector3 firstPos;
    private bool objectInPlace;
    private GameObject partToCreate;
    private GameObject fixed_PartToClone;
    private int currentPartIndex;

    private Vector3 tmpArrowPos;
    private Vector3 tmpCirclePos;

    public TextMeshProUGUI panelAndImageTXT;
    public TextMeshProUGUI partsCreationTXT;
    public TextMeshProUGUI slowTouchImageTXT;
    public int shiftHeight;

    private void Awake()
    {
        partsManager = GameObject.FindObjectOfType<PartsManager>();
        gameManagerBuildScript = FindObjectOfType<GameManagerBuildScript>();
    }

    void Start()
    {
        if (tutorialManagerScript != null)
        {
            tutorialManagerScript.gameObject.SetActive(false);
        }
        objectInPlace = false;
        partInPlace = true;
        currentPartIndex = 0;
    }

    void Update()
    {
        if (partInPlace && currentPartIndex < partsManager.fixed_parts.Length)
        {
            SetTheNextPart();

            currentPartIndex++;
            partInPlace = false;
        }

        if (objectInPlace) 
        {
            partsManager.finalWellDone.SetActive(true);

            partsManager.arrowForPartsPos.SetActive(false);
            partsManager.circleForPartsPos.SetActive(false);

            partsManager.buttonCreatorParts.gameObject.SetActive(false);

            StartCoroutine(FinisedTheGame());
        }
    }

    public IEnumerator FinisedTheGame()
    {
        yield return new WaitForSeconds(2f);
        gameManagerBuildScript.FinishedTheGame();
    }

    void SetTheNextPart()
    {
        partsManager.colliders[currentPartIndex].SetActive(true);
        partsManager.buttonCreatorParts.GetComponent<Image>().sprite =
            partsManager.parts_sprites[currentPartIndex];
        partToCreate = partsManager.parts[currentPartIndex];
        fixed_PartToClone = partsManager.fixed_parts[currentPartIndex];
    }

    private void SetArropwAndCirclePos()
    {
        tmpArrowPos = new Vector3(
            fixed_PartToClone.transform.position.x,
            fixed_PartToClone.transform.position.y + shiftHeight,
            fixed_PartToClone.transform.position.z);

        tmpCirclePos = new Vector3(
            fixed_PartToClone.transform.position.x,
            fixed_PartToClone.transform.position.y,
            fixed_PartToClone.transform.position.z);

        partsManager.arrowForPartsPos.SetActive(true);
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

        if (currentPartIndex == 1)
        {
            panelAndImageTXT.gameObject.SetActive(false);
            partsCreationTXT.gameObject.SetActive(true);
        }
        if (currentPartIndex == 2)
        {
            partsCreationTXT.gameObject.SetActive(false);
            slowTouchImageTXT.gameObject.SetActive(true);
        }
        if (currentPartIndex >= 3)
        {
            slowTouchImageTXT.gameObject.SetActive(false);
            partsManager.imageTutorialCanvas.gameObject.SetActive(false);
        }

    }

    public void SetPartInPlace()
    {
        partInPlace = true;
        fixed_PartToClone.SetActive(true);
        partsManager.buttonCreatorParts.gameObject.SetActive(true);
        partsManager.arrowForPartsPos.SetActive(false);
        partsManager.circleForPartsPos.SetActive(false);

        if (currentPartIndex >= partsManager.fixed_parts.Length)
        {
            objectInPlace = true;
        }
    }
}
