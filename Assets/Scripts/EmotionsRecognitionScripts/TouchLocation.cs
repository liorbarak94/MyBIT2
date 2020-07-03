using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLocation
{
    public int touchId;
    public GameObject touchImage;
    public int answer;

    public TouchLocation(int touchId, GameObject touchImage, int answer)
    {
        this.touchId = touchId;
        this.touchImage = touchImage;
        this.answer = answer;
    }

    public int GetAnswer()
    {
        return this.answer;
    }
}
