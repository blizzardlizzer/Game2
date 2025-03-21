using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FallingObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float fallSpeed = 2f; // Speed at which the object falls

    
    void Update()
    {
        // Move the object downwards over time
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    // private void OnCollisionEnter2D(Collision2D collision)   game over code
    // {
    //     if(collision.gameObject.CompareTag("Object")){
    //         Destroy(this.gameObject);
    //     }
    // }

}



