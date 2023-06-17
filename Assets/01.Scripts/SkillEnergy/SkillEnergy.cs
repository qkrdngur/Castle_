using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEnergy : MonoBehaviour
{
    Slider m_Slider;

    public float mana = 0;

    void Awake()
    {
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
