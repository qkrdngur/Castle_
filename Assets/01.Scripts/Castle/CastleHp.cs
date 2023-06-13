using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleHp : MonoBehaviour
{
    public float castleHp = 100;
    public int cnt = 0;
    public static CastleHp Instance;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(castleHp <= 0)
        {
            anim.SetTrigger("down");
            cnt = 1;
        }

        if(transform.position.y < -10)
            gameObject.SetActive(false);
    }
}
