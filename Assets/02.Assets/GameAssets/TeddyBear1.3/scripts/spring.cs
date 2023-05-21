using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spring : MonoBehaviour
{   
    public float force;             

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player") 
        {
            GetComponent<Animator>().Play("spring");           
            other.gameObject.GetComponent<Rigidbody>().AddForce(0f, force, 0f, ForceMode.Impulse);
        }
    }
}
