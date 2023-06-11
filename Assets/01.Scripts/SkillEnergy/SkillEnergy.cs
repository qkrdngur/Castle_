using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEnergy : MonoBehaviour
{
    Slider m_Slider;
    [SerializeField]
    [Range(0f, 10f)]
    private float mana = 10;

    void Start()
    {
        m_Slider = GetComponent<Slider>();
    }

    void Update()
    {
        m_Slider.value = mana;

        if(mana <= 10)
          mana = Time.fixedTime;
    }
}
