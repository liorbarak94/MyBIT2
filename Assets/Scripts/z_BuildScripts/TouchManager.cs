using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public enum TouchStatus {ZERO, ONE, TWO};
    public TouchStatus touchStatus;
    public Touching touching;

    void Update()
    {
        SetTouchStatus();

        switch (touchStatus)
        {
            case TouchStatus.ZERO:
                touching.ZeroTouch();
                break;

            case TouchStatus.ONE:
                touching.OneTouch();
                break;

            case TouchStatus.TWO:
                touching.TwoTouch();
                break;
        }
    }

    public void SetTouchStatus()
    {
        if (Input.touchCount == 0)
        {
            touchStatus = TouchStatus.ZERO;
        }

        if (Input.touchCount == 1)
        {
            touchStatus = TouchStatus.ONE;
        }

        if (Input.touchCount == 2)
        {
            touchStatus = TouchStatus.TWO;
        }
    }
}
