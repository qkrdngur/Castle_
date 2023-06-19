using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class CastleHp : MonoBehaviour
{
    UiManager uiManager;

    public float castleHp = 100;
    public int cnt = 0;
    private string[] str = new string[2] { "YOU WIN", "YOU LOSE"};

    private TextMeshProUGUI text;

    Animator anim;
    void Awake()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        text = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(castleHp <= 0)
        {
            anim.SetTrigger("down");
            cnt = 1;
        }

        if(transform.position.y < -10)
            gameObject.SetActive(false);


        if (uiManager.towerPos[2].activeSelf == false)
        {
            text.text = str[0];
            showText();
        }
        
        if (uiManager.etowerPos[2].activeSelf == false)
        {
            text.text = str[1];
            showText();
        }
    }

    void showText()
    {
        uiManager.panel.SetActive(true);
        text.transform.DOLocalMove(Vector3.zero, 0.5f);

        if(Input.GetMouseButtonDown(0))
        {
            //신 넘기기
            SceneManager.LoadScene("Main");
        }
    }
}
