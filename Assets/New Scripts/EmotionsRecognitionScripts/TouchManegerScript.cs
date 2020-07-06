using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManegerScript : MonoBehaviour
{
    public GameObject answerImage, answerObjectClicked;
    public List<TouchLocation> touches = new List<TouchLocation>();
    public Transform parent;
    public int answerClicked;
    private Boolean isPressedRight = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                    CheckAnswers();

                ++i;
            }
        }
    }

    private void CheckAnswers()
    {
        ReadNewSituation readNewSituation = GameObject.Find("GameController").GetComponent<ReadNewSituation>();
        if (touches[0].GetAnswer() == touches[1].GetAnswer())
            StartCoroutine(readNewSituation.IsRightAnswerClicked(touches[0].GetAnswer()+""));
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
                    new Vector2(answerObjectClicked.transform.position.x - 70, answerObjectClicked.transform.position.y);
            c.GetComponent<Image>().color = Color.blue;
        }
        if (fingerId == 1)
        {
            if (answerObjectClicked != null)
                c.transform.position =
                    new Vector2(answerObjectClicked.transform.position.x + 70, answerObjectClicked.transform.position.y);
            c.GetComponent<Image>().color = Color.green;
        }
    }
}
