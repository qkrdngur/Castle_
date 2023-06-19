using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private AllySpawner spawner;
    public AllyManager manager;

    private int[] saveIdx = new int[6] { 0,0,0,0,0,0};
    private int[] manaIdx = new int[6] { 3,2,4,4,3,3};
    private bool isrand = false;
    private int mana;

    void Awake()
    {
        spawner = GameObject.Find("AllySpawner").GetComponent<AllySpawner>();
    }
    void Start()
    {
        StartCoroutine(IncreaseMana());
        //rand ·£´ý
        StartCoroutine(RandomIdx());

        isrand = true;
    }

    void Update()
    {
         //¼Ä¼º
         inst();
    }

    void inst()
    {
        if(mana - manaIdx[saveIdx[0]] >= 0)
        {
            Instantiate(spawner.ePrefabs[saveIdx[0]], transform.position, Quaternion.identity);
            mana -= manaIdx[saveIdx[0]];

            isrand = true;
        }
    }

    IEnumerator IncreaseMana()
    {
        while (true)
        {
            if (mana < 10)
            {
                yield return new WaitForSeconds(2f);
                mana++;
            }
            yield return null;
        }
    }

    //ÀÎµ¦½º ¼¯±â
    IEnumerator RandomIdx()
    {
        while (true)
        {
            if (isrand)
            {
                for (int i = 0; i < 6; i++)
                {
                    saveIdx[i] = i;
                }

                int temp, idx1, idx2;
                for (int i = 0; i < 10; i++)
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
