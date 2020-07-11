using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public PartsManager partsManager;
    public GameManagerBuildScript gameManagerBuildScript;

    private readonly float fillAmount = 0.5f;

    private Vector3 fingerIndicationCanvasPos;
    private Vector3 middle;

    private GameObject objectWasTouched;

    private readonly float moveSpeed = 10.0f;
    public float fingerIndicationCanvas_ShiftHeight;

    private enum TouchCounterStatus {ZERO, ONE, TWO};
    private TouchCounterStatus currentTouchCounterStatus;
    private TouchCounterStatus previousTouchCounterStatus;

    private bool isCountingTouches;

    public int allOneTouchesCounter;
    public int[] partsCounterTouches;

    private int currPartIndex;


    void Start()
    {
        partsCounterTouches = new int[partsManager.parts.Length];
        isCountingTouches = false;

        objectWasTouched = null;

        if (partsManager.mainCamera == null)
        {
            partsManager.mainCamera = Camera.main;
        }
    }

    void Update()
    {
        SetTouchCounterStatus();

        switch (currentTouchCounterStatus)
        {
            case TouchCounterStatus.ZERO:
                ZeroTouch();
                isCountingTouches = true;
                break;

            case TouchCounterStatus.ONE:
                OneTouch();
                isCountingTouches = false;
                break;

            case TouchCounterStatus.TWO:
                TwoTouch();
                isCountingTouches = false;
                break;
        }
    }

    public void SetTouchCounterStatus()
    {
        previousTouchCounterStatus = currentTouchCounterStatus;

        if (Input.touchCount == 0)
        {
            currentTouchCounterStatus = TouchCounterStatus.ZERO;
        }

        else if (Input.touchCount == 1)
        {
            currentTouchCounterStatus = TouchCounterStatus.ONE;
        }

        else if (Input.touchCount == 2)
        {
            currentTouchCounterStatus = TouchCounterStatus.TWO;
        }

        if (previousTouchCounterStatus == TouchCounterStatus.ZERO
            && currentTouchCounterStatus == TouchCounterStatus.TWO)
        {
            isCountingTouches = true;
        }
        else if (previousTouchCounterStatus == TouchCounterStatus.ONE
            && currentTouchCounterStatus == TouchCounterStatus.TWO)
        {
            isCountingTouches = true;
        }
    }

    public void ZeroTouch()
    {
        gameManagerBuildScript.waitForSecondTouchOnPart_Time = 0;
        gameManagerBuildScript.WaitForSecondTouchOfPart_Activation(false);

        partsManager.finger1Indication.GetComponent<Image>().fillAmount = 0;
        partsManager.finger2Indication.GetComponent<Image>().fillAmount = 0;
    }

    public void OneTouch()
    {
        Touch firstTouch = Input.GetTouch(0);
        Ray firstRayBefore = partsManager.mainCamera.ScreenPointToRay(firstTouch.position);

        if (Physics.Raycast(firstRayBefore, out RaycastHit hitFirst))
        {
            if (CheckIfRayCollideWithParts(hitFirst))
            {
                gameManagerBuildScript.waitForSecondTouchOnPart_Time += Time.deltaTime;
                
                if (gameManagerBuildScript.waitForSecondTouchOnPart_Time >=
                    FinalValues.WAIT_TIME_FOR_SECOND_TOUCH_OF_PART)
                {
                    gameManagerBuildScript.WaitForSecondTouchOfPart_Activation(true);
                }

                partsManager.finger1Indication.GetComponent<Image>().fillAmount = fillAmount;
                partsManager.finger2Indication.GetComponent<Image>().fillAmount = 0;
            }
            else
            {
                if (isCountingTouches)
                {
                    allOneTouchesCounter++;
                }
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
                    if (gameManagerBuildScript.waitForSecondTouchOnPart_Time >=
                        FinalValues.WAIT_TIME_FOR_SECOND_TOUCH_OF_PART)
                    {
                        gameManagerBuildScript.waitForSecondTouchOnPart_Time = 0;
                        gameManagerBuildScript.WaitForSecondTouchOfPart_Activation(false);
                    }

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

                    if (isCountingTouches)
                    {
                        currPartIndex = partsManager.currentPartIndex;
                        currPartIndex--;
                        partsCounterTouches[currPartIndex]++;
                    }
                }
            }
            else
            {
                if (isCountingTouches)
                {
                    allOneTouchesCounter++;
                }
            }
        }
    }

    private bool CheckIfRayCollideWithParts(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                currPartIndex = partsManager.currentPartIndex;
                currPartIndex--;

                if (hit.transform.gameObject.tag == 
                    partsManager.parts[currPartIndex].gameObject.tag)
                {
                    objectWasTouched = hit.transform.gameObject;
                    CanvasPos();
                    return true;
                }
            }
        }
        return false;
    }

    private void CanvasPos()
    {
        fingerIndicationCanvasPos = objectWasTouched.transform.position;
        fingerIndicationCanvasPos.y += fingerIndicationCanvas_ShiftHeight;
        partsManager.fingerIndicationCanvas.transform.position = fingerIndicationCanvasPos;
        partsManager.fingerIndicationCanvas.transform.LookAt(partsManager.mainCamera.transform);
    }
}
