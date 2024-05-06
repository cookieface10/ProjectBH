using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbEnemy : MonoBehaviour
{
    float speed = 2;
    int count = 0;
    bool left;
    float distance;
    Vector3 startingCords;
    public float distanceAllowed;

    private void Start()
    {
        startingCords = transform.position;
    }
    void Update()
    {
        //Basic script to move the enemy left and right for tracking purposes.
        if (left)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if(distance > distanceAllowed)
        {
            left = !left;
            startingCords = transform.position;
        }


        distance = Vector3.Distance(startingCords, transform.position);

        

    }
}
