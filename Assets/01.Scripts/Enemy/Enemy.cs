using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    private UiManager uiManager;

    [SerializeField]
    private EnemyManager manager;
    //public EnemyManager EnemyManager { set { manager = value; } }

    Animator ani;
    NavMeshAgent agent;
    Collider[] findEnemy, atEnemy;

    [SerializeField]
    LayerMask lm;
    Vector3 box, findBox;

    private bool isFindEnemy, isHp = false;

    [SerializeField]
    private int enemyHp = 50;

    [SerializeField]
    private int stopping;

    private void Awake()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(attHp());
    }

    IEnumerator attHp()
    {
        while (true)
        {
            if (isHp)
            {
                if (lm ==  (1 << LayerMask.NameToLayer("Castle")) )
                {
                    if (atEnemy.Length > 0)
                    {
                        atEnemy[0].gameObject.GetComponent<CastleHp>().castleHp -= 5;
                    }
                    else
                    {
                        ani.SetTrigger("walk");
                        atEnemy = null;
                        findEnemy = null;
                    }
                }
                else
                {
                    if(atEnemy.Length > 0)
                    {
                       atEnemy[0].gameObject.GetComponent<Enemy>().enemyHp -= 5;
                    }
                    else
                    {
                        ani.SetTrigger("walk");
                        atEnemy=null;
                        findEnemy = null;
                    }
                }
              //  Debug.Log(atEnemy[0].gameObject.GetComponent<Enemy>().enemyHp);
                yield return new WaitForSeconds(1.2f);
            }
            yield return null;
        }
    }

    private void Update()
    {
        Attack();
        Follow();
        Hp();
        Debug.Log(atEnemy.Length);
    }

    private void Hp()
    {
        if(enemyHp <= 0)
        {
            isHp = false;
            atEnemy[0].gameObject.GetComponent<Enemy>().ani.SetTrigger("die");
        }
    }

    void Follow()
    {
        if (agent.enabled == true)
        {
            if (isFindEnemy)
            {
                lm = LayerMask.GetMask("Enemy");
                Vector3 dir = new Vector3(findEnemy[0].transform.position.x, transform.position.y, findEnemy[0].transform.position.z);
                agent.SetDestination(dir);
            }
            else
            {
                lm = LayerMask.GetMask("Castle");
                if (transform.position.z <= 0)
                {
                    if (uiManager.towerPos[0].activeSelf == true)
                        agent.SetDestination(uiManager.towerPos[0].transform.position);
                    else
                        agent.SetDestination(uiManager.towerPos[2].transform.position);
                }
                else
                {
                    if (uiManager.towerPos[1].activeSelf == true)
                        agent.SetDestination(uiManager.towerPos[1].transform.position);
                    else
                        agent.SetDestination(uiManager.towerPos[2].transform.position);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, box);
        Gizmos.DrawWireCube(transform.position, findBox);
    }

    void Attack()
    {
        //공격범위 감지 박스
        box = new Vector3(4, 4, 4);
        //적인지범위 감지 박스
        findBox = new Vector3(20, 20, 20);

        findEnemy = Physics.OverlapBox(transform.position, findBox, transform.rotation,lm);
        atEnemy = Physics.OverlapBox(transform.position, box, transform.rotation, lm);

        if (lm == LayerMask.GetMask("Castle"))
            agent.stoppingDistance = stopping + 2;
        else
            agent.stoppingDistance = stopping;
        
        if (findEnemy.Length > 0) // 타워쪽으로 이동하다가 적이 인식범위 안에 들어왔을때
        {
            //Castle에 다가가고 있을 때
            if (findEnemy.Length > 1 && findEnemy[1].gameObject.CompareTag("Castle"))
                isFindEnemy = false;
            else
                isFindEnemy = true;

            if (atEnemy.Length > 0)
            {
                Vector3 dir = new Vector3();
                dir.x = atEnemy[0].transform.position.y - transform.position.y;
                transform.eulerAngles += dir;
                ani.SetTrigger("attack");
                isHp = true;
            }
        }
        else // 적이 감지 안되있을때
        {
            ani.SetTrigger("walk");
            isFindEnemy=false;
        }
         agent.enabled = true;
    }
}
