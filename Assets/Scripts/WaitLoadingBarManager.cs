using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitLoadingBarManager : MonoBehaviour
{
    public GameObject waitLoadingBar;

    public void WaitLoadingBar_Activation(bool isActive)
    {
        waitLoadingBar.SetActive(isActive);
    }
}
