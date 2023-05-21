using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTeddy3d : MonoBehaviour
{

    public GameObject Teddy;
    GameObject Teddy2;
    Transform trans;
    Transform trans2;
    public static bool dead;



    void Start ()
    {
        trans = Teddy.GetComponent<Transform>();
        Teddy2 = Teddy.transform.Find("ROOT/Teddy 1/Teddy Pelvis").gameObject;
        trans2 = Teddy2.GetComponent<Transform>();
        dead = false;
    }

    // Camera follows a different object when dead
    void Update ()
    {
        if (dead)
        {           
            transform.position = trans2.position + new Vector3(0f, 1.75f, 5f);            
            transform.LookAt(trans2);
        }
        else
        {
            transform.position = trans.position + new Vector3(0f, 2f, 5f);
            transform.LookAt(trans);
        }
    }
}
