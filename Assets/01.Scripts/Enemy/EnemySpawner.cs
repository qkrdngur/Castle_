using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyType
{
    Normal, Power, Sensor, Speed, Tank
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<EnemyManager> enemyManager;

    public GameObject[] enemyPrefabs;

    [SerializeField]
    private EnemyManager manager;
    public EnemyManager EnemyManager { set { manager = value; } }

    private UiManager uiManager;
    public int[] savearr = new int [6];
    private void Awake()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();

        for(int i=0; i<savearr.Length;i++)
        {
            savearr[i] = i;
        }
        for(int i = 0; i < 5; i++)
        {
            int temp = savearr[i];
            manager.SaveRandom = Random.Range(0, 6);
            savearr[i] = manager.SaveRandom;
            manager.SaveRandom = temp;
            uiManager.images[i].sprite = uiManager.sprites[manager.SaveRandom];
            savearr[i] = manager.SaveRandom;
        }
    }

    private void Update()
    {
        
    }
}
