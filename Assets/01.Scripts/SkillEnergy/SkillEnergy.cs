using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEnergy : MonoBehaviour
{
    public static SkillEnergy instance;

    Slider m_Slider;
    [SerializeField]
    [Range(0f, 10f)]
    public float mana = 10;

    void Awake()
    {
        if(instance == null)
        {
            instance = new SkillEnergy();
        }

        m_Slider = GetComponent<Slider>();
    }

    void Start()
    {
        StartCoroutine(IncreaseMana());
    }

    IEnumerator IncreaseMana()
    {
        while(true)
        {
            if(mana < 10)
            {
                yield return new WaitForSeconds(1f);
                mana++;
            }
            yield return null;
        }
    }

    void Update()
    {
        m_Slider.value = mana;
    }
}
