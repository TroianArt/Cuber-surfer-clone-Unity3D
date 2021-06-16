using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCylinder : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1f, 0));
    }
}
