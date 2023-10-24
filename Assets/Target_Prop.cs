using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Prop : MonoBehaviour
{ 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
