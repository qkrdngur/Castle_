using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Ally : MonoBehaviour
{
    #region Header

    [SerializeField]
    private AllyManager manager;

    private UiManager uiManager;

    Animator ani;
    NavMeshAgent agent;
    Collider[] findEnemy, atEnemy, castleEnemy;

    private LayerMask enemyLayer = 1 << 7;//��(enemy) ���̾�
    private LayerMask castleLayer = 1 << 8;//��(castle) ���̾�
    private Vector3 box, findBox, castleBox;

    private bool isFindEnemy, isHp = false;
    private bool isActive = false;

    public int allyHp = 50;

    [SerializeField] private int stopping;
    private GameObject saveObj;
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
        StartCoroutine(activeObj());
    }

    IEnumerator activeObj()
    {
        while(true)
        {
            if(isActive)
            {
                yield return new WaitForSeconds(1f);
                gameObject.SetActive(false);
                isActive = !isActive;
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
        Debug.Log(allyHp);
        Attack();
        Follow();
        Hp();
    }

    private void Hp()
    {
        if(allyHp <= 0)
        {
            isHp = false;
            ani.SetTrigger("die");
            isActive = true;
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
        //���ݹ��� ���� �ڽ�
        box = new Vector3(8, 8, 8);
        //���������� ���� �ڽ�
        findBox = new Vector3(20, 20, 20);
        //castle���� �ڽ�
        castleBox = new Vector3(8,8,8);

        //enemy
        findEnemy = Physics.OverlapBox(transform.position, findBox, transform.rotation, enemyLayer);
        atEnemy = Physics.OverlapBox(transform.position, box, transform.rotation, enemyLayer);
        //castle
        castleEnemy = Physics.OverlapBox(transform.position, castleBox, transform.rotation, castleLayer);


        if (castleEnemy.Length > 0)
        {
            isFindEnemy = false;
            ani.SetTrigger("attack");
            isHp = true;
        }

        if (findEnemy.Length > 0) // Ÿ�������� �̵��ϴٰ� ���� �νĹ��� �ȿ� ��������
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
