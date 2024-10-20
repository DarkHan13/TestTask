using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkChecker : MonoBehaviour
{
    public event Action OnObjectEnter, OnObjectExit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object")) OnObjectEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object")) OnObjectExit?.Invoke();
    }
}
