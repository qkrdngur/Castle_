using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private AllySpawner spawner;

    private int randNum = 0;
    private int[] saveIdx = new int[6];
    private bool isrand = false;

    void Awake()
    {
        spawner = transform.Find("AllySpawner").GetComponent<AllySpawner>();
    }
    void Start()
    {
        //rand ·£´ý
        StartCoroutine(RandomIdx());
    }

    void Update()
    {
        //¼Ä¼º
        isnt();
    }

    void isnt()
    {
        for (int i = 0; i < 6; i++)
        {
            Instantiate(spawner.enemyPrefabs[saveIdx[i]], transform.position, Quaternion.identity);
        }
    }

    IEnumerator RandomIdx()
    {
        while (true)
        {
            if (isrand)
            {
                for (int i = 0; i < 6; i++)
                {
                    saveIdx[i] = i + 1;
                }

                int temp, idx1, idx2;
                for (int i = 0; i < 20; i++)
                {
                    idx1 = Random.Range(0, 6);
                    idx2 = Random.Range(0, 6);
                    temp = saveIdx[idx1];
                    saveIdx[idx1] = saveIdx[idx2];
                    saveIdx[idx2] = temp;
                }

                isrand = !isrand;
            }
            yield return null;
        }
    }
}
