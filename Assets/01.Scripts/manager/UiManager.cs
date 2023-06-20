using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public Button[] button;
    public Image[] images;
    public Sprite[] sprites;
    public GameObject[] towerPos;
    public GameObject[] etowerPos;
    public GameObject panel;
    public GameObject limit;

    public void Return()
    {
        SceneManager.LoadScene("Main");
    }
}
