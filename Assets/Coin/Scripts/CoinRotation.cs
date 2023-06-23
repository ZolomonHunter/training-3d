using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float rotationSpeen = 30;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeen * Time.deltaTime);
    }
}
