using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartsManager : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas fingerIndicationCanvas;
    public Image finger1Indication;
    public Image finger2Indication;

    public GameObject planeForPartsPos;
    public GameObject arrowForPartsPos;
    public GameObject circleForPartsPos;

    public Image imageTutorialCanvas;

    [SerializeField]
    public GameObject[] coins_MoveJoyStick;

    [SerializeField]
    public GameObject[] coins_RotateJoyStick;

    public Button buttonCreatorParts;

    [SerializeField]
    public GameObject[] parts;

    [SerializeField]
    public GameObject[] colliders;

    [SerializeField]
    public GameObject[] fixed_parts;

    [SerializeField]
    public Sprite[] parts_sprites;

    public GameObject finalWellDone;

    public int currentPartIndex;


}
