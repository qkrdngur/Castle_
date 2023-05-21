using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleHp : MonoBehaviour
{
    public float castleHp = 100;
    public static CastleHp Instance;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(castleHp);
        if(castleHp <= 0)
        {
            anim.SetTrigger("down");
        }

        if(transform.position.y < -10)
            gameObject.SetActive(false);
    }
}
