using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private AllySpawner spawner;

    [SerializeField]
    private GameObject spawnObj;

    private int[] saveIdx = new int[6] { 0,0,0,0,0,0};
    private int[] manaIdx = new int[6] { 3,2,4,4,3,3};

    private bool isrand = false;

    private float spawnCool = 3f;
    private int mana = 0;

    void Awake()
    {
        spawner = GameObject.Find("AllySpawner").GetComponent<AllySpawner>();
    }
    void Start()
    {
        StartCoroutine(IncreaseMana());
        //rand ����
        StartCoroutine(RandomIdx());

        isrand = true;

        spawnCool = PlayerPrefs.GetInt("Onspawner") == 1?   2.5f :  spawnCool;

        if (PlayerPrefs.GetInt("Onspawner") == 0)
        {
            spawnObj.SetActive(false);
        }
    }

    void Update()
    {
         //�ļ�
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
                spawnCool = Random.Range(spawnCool, spawnCool + 0.2f);
                yield return new WaitForSeconds(spawnCool);
                mana++;
            }
            yield return null;
        }
    }

    //�ε��� ����
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
