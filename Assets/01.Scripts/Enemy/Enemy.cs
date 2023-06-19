using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    #region Header

    public AllyManager manager;

    private UiManager uiManager;

    Animator ani;
    NavMeshAgent agent;
    Collider[] findEnemy, atEnemy;

    private GameObject saveObj;

    private LayerMask enemyLayer = 1 << 3;//�Ʊ�(enemy) ���̾�
    private LayerMask castleLayer = 1 << 8;//��(castle) ���̾� 
    private Vector3 box, findBox;

    private bool isFindEnemy, isHp = false;

    public int enemyHp = 50;

    [SerializeField] private int stopping;
    #endregion

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
                if (!isFindEnemy)
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
                    if (atEnemy.Length > 0)
                    {
                        if (atEnemy[0].gameObject.layer == enemyLayer)
                            atEnemy[0].gameObject.GetComponent<Ally>().allyHp -= 5;
                        //else
                            //atEnemy[1].gameObject.GetComponent<Ally>().allyHp -= 5;
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
    }

    private void Hp()
    {
        if (enemyHp <= 0)
        {
            isHp = false;
            ani.SetTrigger("die");
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
        //���ݹ��� ���� �ڽ�
        box = new Vector3(4, 4, 4);
        //���������� ���� �ڽ�
        findBox = new Vector3(20, 20, 20);

        findEnemy = Physics.OverlapBox(transform.position, findBox, transform.rotation, enemyLayer);
        atEnemy = Physics.OverlapBox(transform.position, box, transform.rotation, enemyLayer);


        if (findEnemy.Length > 0) // Ÿ�������� �̵��ϴٰ� ���� �νĹ��� �ȿ� ��������
        {
            saveObj = findEnemy[0].gameObject;
            isFindEnemy = true;

            for (int i = 0; i < atEnemy.Length; i++)
            {
                if (Vector3.Distance(atEnemy[i].transform.position, transform.position) == stopping)
                {
                    Vector3 dir = new Vector3();
                    dir.x = atEnemy[0].transform.position.y - transform.position.y;
                    transform.eulerAngles += dir;
                    ani.SetTrigger("attack");
                    isHp = true;
                }
            }
        }
        else // ���� ���� �ȵ�������
        {
            ani.SetTrigger("walk");
            isFindEnemy = false;
            findEnemy = null;
            atEnemy = null;
        }
        agent.enabled = true;
    }
}
