using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManegerScript : MonoBehaviour
{
    public GameObject answerImage, answerObjectClicked;
    public List<TouchLocation> touches = new List<TouchLocation>();
    public Transform parent;
    public int answerClicked;
    public string ansClickedStr;
    private Boolean isPressedRight = false;


    public void OnPressedDown(int answerClicked)
    {
        Debug.Log("OnPressedDown Start, Answer: " + answerClicked);
        answerObjectClicked = GameObject.Find("Answer" + (answerClicked + 1) + "Image");
        this.answerClicked = answerClicked;
        isPressedRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressedRight)
        {
            int i = 0;
            while (i < Input.touchCount && i < 2)
            {
                Touch t = Input.GetTouch(i);
                if (t.phase == TouchPhase.Began)
                {
                    Debug.Log("touch began");
                    touches.Add(new TouchLocation(t.fingerId, CreateTouchImage(t), answerClicked));
                }
                else if (t.phase == TouchPhase.Ended)
                {
                    Debug.Log("touch ended");
                    TouchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                    Destroy(thisTouch.touchImage);
                    touches.RemoveAt(touches.IndexOf(thisTouch));
                    if (touches.Count == 0)
                        isPressedRight = false;
                }

                if (i == 1 && t.phase == TouchPhase.Began)
                {
                    Debug.Log("CheckAnswers() Starts");
                    CheckAnswers();
                }

                ++i;
            }
        }
    }

    private void CheckAnswers()
    {
        Debug.Log("CheckAnswers() Starts");
        ReadNewSituation readNewSituation = GameObject.Find("GameController").GetComponent<ReadNewSituation>();
        if (touches[0].GetAnswer() == touches[1].GetAnswer())
        {
            Debug.Log("enter here");
            ansClickedStr = touches[0].GetAnswer() + "";
            StartCoroutine(readNewSituation.CheckAnswers(ansClickedStr)); 
        }
    }

    public void StartIsRightAnswerClicked()
    {
        ReadNewSituation readNewSituation = GameObject.Find("GameController").GetComponent<ReadNewSituation>();
        StartCoroutine(readNewSituation.IsRightAnswerClicked(ansClickedStr));
    }

    GameObject CreateTouchImage(Touch t)
    {
        GameObject c = Instantiate(answerImage) as GameObject;
        c.name = "Touch" + t.fingerId;
        c.transform.SetParent(parent);
        c.transform.SetAsFirstSibling();
        SetColorAndPosiotion(c, t.fingerId);
        return c;
    }

    private void SetColorAndPosiotion(GameObject c, int fingerId)
    {
        if (fingerId == 0)
        {
            if (answerObjectClicked != null)
                c.transform.position =
                    new Vector2(answerObjectClicked.transform.position.x-50, answerObjectClicked.transform.position.y);
            c.GetComponent<Image>().color = Color.blue;
        }
        if (fingerId == 1)
        {
            if (answerObjectClicked != null)
                c.transform.position =
                    new Vector2(answerObjectClicked.transform.position.x+50, answerObjectClicked.transform.position.y);
            c.GetComponent<Image>().color = Color.green;
        }
    }
}
