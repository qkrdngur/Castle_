using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private AllySpawner spawner;

    private int randNum = 0;
    private int saveIdx = 0;

    void Start()
    {
        spawner = transform.Find("AllySpawner").GetComponent<AllySpawner>();
    }

    void Update()
    {
        
    }

    void RandomIdx()
    {
        for(int i = 0; i < 6; i++)
        {
            randNum = Random.Range(0, 6);
            if(saveIdx != randNum)
            {
                int n;

            }
        }
    }
}
