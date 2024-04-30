using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titanLocation : MonoBehaviour
{
    [Header("Stats")]
    public Vector3 location;
    public Quaternion rotation;

    // Update is called once per frame
    void Update()
    {
        GameManager.TitanLocation = transform.position;
        location = transform.position;
        rotation = transform.rotation;
        GameManager.TitanRotation = transform.rotation;
    }
}
