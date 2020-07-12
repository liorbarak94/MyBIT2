using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoins : MonoBehaviour
{
    private int rotateSpeed = 3;

    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }
}
