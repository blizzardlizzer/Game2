using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    float wait = 0.25f;
    public GameObject fallingObject;
    // public float fallSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fall", wait, wait);
    }


    void Fall()
    {
        GameObject obj = Instantiate(fallingObject, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);
        
        }
    }


