using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    public float waitTime = 0.25f;
    public GameObject fallingObject;
    // public float fallSpeed = 2f;

    // Start is called before the first frame update
    public void StartFall()
    {
            InvokeRepeating("Fall", waitTime, waitTime);
    }


    void Fall()
    {
        GameObject obj = Instantiate(fallingObject, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);
        }
    }


