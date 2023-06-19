using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleHp : MonoBehaviour
{
    UiManager uiManager;
    public float castleHp = 100;
    public int cnt = 0;
    Animator anim;
    void Start()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
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

        if (uiManager.towerPos[2].activeSelf == false || uiManager.etowerPos[2].activeSelf == false)
        {

        }
    }
}
