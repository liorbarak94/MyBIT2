using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollider : MonoBehaviour
{
    public string colliderName;
    private ManagePartCreation managePartCreation;

    private void Start()
    {
        managePartCreation = FindObjectOfType<ManagePartCreation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == colliderName)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            managePartCreation.SetPartInPlace();
        }
    }
}
