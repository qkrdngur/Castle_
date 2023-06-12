using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "scriptable Object/Enemey Data", order = int.MaxValue)]
public class EnemyManager : ScriptableObject
{
    [SerializeField]
    private string enemyName;

    public string EnemyName { get { return enemyName; } }

    [SerializeField]
    private int enemyHp;

    public int EnemyHp { get { return enemyHp; } set {enemyHp = value; } }

    [SerializeField]
    private float enemyDamege;

    public float EnemyDamege { get { return enemyDamege; } }

    [SerializeField]
    private int buttonNum;

    public int ButtonNum { get; set; }

    [SerializeField]
    private GameObject GclickObject;

    public GameObject GClickObject { get; set; }

    [SerializeField]
    private int saveRandom;

    public int SaveRandom { get; set; }
}
