using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    #region Header

    AudioSource audio;

    public AllyManager manager;

    private UiManager uiManager;

    Animator ani;
    NavMeshAgent agent;
    Collider[] findEnemy, atEnemy, castleEnemy;

    private GameObject saveObj;

    private LayerMask enemyLayer = 1 << 3;//아군(enemy) 레이어
    private LayerMask castleLayer = 1 << 9;//성(castle) 레이어 
    private Vector3 box, findBox;

    [SerializeField]
    private Vector3 castleBox;

    private bool isFindEnemy, isHp = false, isDie = false;

    public int enemyHp = 50;

    [SerializeField] private int stopping;
    #endregion

    private void Awake()
    {
        audio = GameObject.Find("Attack").GetComponent<AudioSource>();
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(attHp());
        StartCoroutine(Die());
    }


    IEnumerator Die()
    {
        while(true)
        {
            if(isDie)
            {
               yield return new WaitForSeconds(1);
                Destroy(gameObject);
                isDie = !isDie;
            }
            yield return null;
        }
    }


    IEnumerator attHp()
    {
        while (true)
        {
            if (isHp)
            {
                audio.Play();
                if (!isFindEnemy)
                {
                    if (castleEnemy.Length > 0)
                    {
                        castleEnemy[0].gameObject.GetComponent<CastleHp>().castleHp -= 5;
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
                    if (atEnemy.Length > 0)
                    {
                            atEnemy[0].gameObject.GetComponent<Ally>().allyHp -= 5;
                    }
                    else
                    {
                        ani.SetTrigger("walk");
                        atEnemy = null;
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

        if (atEnemy.Length <= 0)
            audio.Pause();
    }

    private void Hp()
    {
        if (enemyHp <= 0)
        {
            isHp = false;
            ani.SetTrigger("die");
            isDie = true;
        }
    }

    void Follow()
    {
        if (agent.enabled == true)
        {
            if (isFindEnemy)
            {
                agent.stoppingDistance = stopping;

                Vector3 dir = new Vector3(saveObj.transform.position.x, transform.position.y, saveObj.transform.position.z);
                agent.SetDestination(dir);
            }
            else
            {
                agent.stoppingDistance = stopping + 2;

                if (transform.position.z <= 0)
                {
                    if (uiManager.etowerPos[0].activeSelf == true)
                        agent.SetDestination(uiManager.etowerPos[0].transform.position);
                    else
                        agent.SetDestination(uiManager.etowerPos[2].transform.position);
                }
                else
                {
                    if (uiManager.etowerPos[1].activeSelf == true)
                        agent.SetDestination(uiManager.etowerPos[1].transform.position);
                    else
                        agent.SetDestination(uiManager.etowerPos[2].transform.position);
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
        box = new Vector3(8, 8, 8);
        //적인지범위 감지 박스
        findBox = new Vector3(20, 20, 20);

        //enemy
        findEnemy = Physics.OverlapBox(transform.position, findBox, transform.rotation, enemyLayer);
        atEnemy = Physics.OverlapBox(transform.position, box, transform.rotation, enemyLayer);
        //castle
        castleEnemy = Physics.OverlapBox(transform.position, box, transform.rotation, castleLayer);


        if(castleEnemy.Length > 0)
        {
            isFindEnemy = false;
            ani.SetTrigger("attack");
            isHp = true;
        }

        #region 적감지
        if (findEnemy.Length > 0) // 타워쪽으로 이동하다가 적이 인식범위 안에 들어왔을때
        {
            saveObj = findEnemy[0].gameObject;
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
            isFindEnemy = false;
            findEnemy = null;
            atEnemy = null;
        }
        agent.enabled = true;
    }
    #endregion
}
