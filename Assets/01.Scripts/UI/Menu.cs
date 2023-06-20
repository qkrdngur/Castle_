using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] canvas;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject StartPanel;

    private int cnt = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = cnt.ToString();

        if (cnt <= 1)
            SceneManager.LoadScene("Easy");
    }

    public void OnStart()
    {
        StartPanel.transform.DOLocalMove(Vector3.zero, 1);
    }

    void Count()
    {
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
        
        text.transform.DOShakePosition(3, 20, 20);
        StartCoroutine(Cnt());
    }

    IEnumerator Cnt()
    {
        while(cnt > 1)
        {
            yield return new WaitForSeconds(1f);
            cnt--;
        }
    }

    public void Easy()
    {
        PlayerPrefs.SetInt("Onspawner", 0);
        Count();
    }
    
    public void Normal()
    {
        PlayerPrefs.SetInt("Onspawner", 1);
        Count();
    }

    public void Quit()
    {
        Application.Quit();
    }    
}
