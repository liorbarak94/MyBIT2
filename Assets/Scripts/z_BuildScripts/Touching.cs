using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Touching : MonoBehaviour
{
    /*
    [HideInInspector]
    public PartsManager partsManager;

    private readonly float fillAmount = 0.5f;

    private Vector3 fingerIndicationCanvasPos;
    private Vector3 middle;

    private GameObject objectWasTouched;

    private readonly float moveSpeed = 10.0f;
    public float shiftHeight;

    private void Awake()
    {
        partsManager = GameObject.FindObjectOfType<PartsManager>();
    }

    private void Start()
    {
        objectWasTouched = null;
        partsManager.fingerIndicationCanvas.gameObject.SetActive(true);

        if (partsManager.mainCamera == null)
        {
            partsManager.mainCamera = Camera.main;
        }
    }

    public void ZeroTouch()
    {
        partsManager.finger1Indication.GetComponent<Image>().fillAmount = 0;
        partsManager.finger2Indication.GetComponent<Image>().fillAmount = 0;
    }

    public void OneTouch(int allOneTouchesCounter)
    {
        Touch firstTouch = Input.GetTouch(0);
        Ray firstRayBefore = partsManager.mainCamera.ScreenPointToRay(firstTouch.position);

        if (Physics.Raycast(firstRayBefore, out RaycastHit hitFirst))
        {
            if (CheckIfRayCollideWithParts(hitFirst))
            {
                partsManager.finger1Indication.GetComponent<Image>().fillAmount = fillAmount;
                partsManager.finger2Indication.GetComponent<Image>().fillAmount = 0;
                allOneTouchesCounter++;
            }
        }
    }

    public void TwoTouch()
    {
        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        Ray firstRayAfter = partsManager.mainCamera.ScreenPointToRay(firstTouch.position);
        Ray secoondRayAfter = partsManager.mainCamera.ScreenPointToRay(secondTouch.position);

        Ray firstRayBefore = partsManager.mainCamera.ScreenPointToRay(firstTouch.position - firstTouch.deltaPosition);
        Ray secondRayBefore = partsManager.mainCamera.ScreenPointToRay(secondTouch.position - secondTouch.deltaPosition);

        Plane firstPlane = new Plane(Vector3.up, transform.position);
        Plane secondPlane = new Plane(Vector3.up, transform.position);

        if (Physics.Raycast(firstRayBefore, out RaycastHit hitFirst) &&
            Physics.Raycast(secondRayBefore, out RaycastHit hitSecond))
        {
            if (CheckIfRayCollideWithParts(hitFirst) &&
                CheckIfRayCollideWithParts(hitSecond))
            {
                if (firstPlane.Raycast(firstRayAfter, out float firstDistance) && 
                    secondPlane.Raycast(secoondRayAfter, out float secondDistance))
                {
                    partsManager.finger1Indication.GetComponent<Image>().fillAmount = fillAmount;
                    partsManager.finger2Indication.GetComponent<Image>().fillAmount = fillAmount;

                    objectWasTouched = hitFirst.transform.gameObject;
                    float yPos = objectWasTouched.transform.position.y;

                    Vector3 firstPos = firstRayBefore.GetPoint(firstDistance);
                    Vector3 secondPos = secondRayBefore.GetPoint(secondDistance);

                    middle = (firstPos + secondPos) / 2;
                    middle.y = yPos;

                    objectWasTouched.transform.position = Vector3.Lerp(objectWasTouched.transform.position, middle, moveSpeed);

                    CanvasPos();
                } 
            }
        }
    }

    private bool CheckIfRayCollideWithParts(RaycastHit hit)
    {
        bool ans = false;
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                for (int i = 0; i < partsManager.parts.Length; i++)
                {
                    if (hit.transform.gameObject.tag == partsManager.parts[i].gameObject.tag)
                    {
                        objectWasTouched = hit.transform.gameObject;
                        ans = true;
                        CanvasPos();
                        return ans;
                    }
                }
            }
        }
        return ans;
    }

    private void CanvasPos()
    {
        fingerIndicationCanvasPos = objectWasTouched.transform.position;
        fingerIndicationCanvasPos.y += shiftHeight;
        partsManager.fingerIndicationCanvas.transform.position = fingerIndicationCanvasPos;
        partsManager.fingerIndicationCanvas.transform.LookAt(partsManager.mainCamera.transform);
    }

    */
}



