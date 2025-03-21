using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FallingObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float fallSpeed = 0f; // Speed at which the object falls
    public PlayerMovement playerScript;

    
    void Update()
    {
        // Move the object downwards over time
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border")){
            Debug.Log("Hit border");
            Destroy(this.gameObject);
        }
    }

}



