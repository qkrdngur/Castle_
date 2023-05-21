using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEnergy : MonoBehaviour
{
    Slider m_Slider;
    [SerializeField]
    private float mana = 10;

    void Start()
    {
        m_Slider = GetComponent<Slider>();
    }

    void Update()
    {
        
    }
}
