using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardDEBUG : MonoBehaviour
{
    private void Update()
    {
        Debug.Log($"{transform.forward} | {transform.rotation * Vector3.forward}");
    }
}
