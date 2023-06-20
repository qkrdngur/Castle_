using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class DrogAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    AudioSource audio;

    public static Vector2 DefaultPos, currentPos, mousePos;
    public static Vector3 instPos;

    public AllyManager manager;

    private SkillEnergy skillE;
    private UiManager uiManager;
    private AllySpawner spawner;

    void Awake()
    {
        audio = GameObject.Find("SpawnSound").GetComponent<AudioSource>();
        skillE = GameObject.Find("PlayerMana").GetComponent<SkillEnergy>();
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        spawner = GameObject.Find("AllySpawner").GetComponent<AllySpawner>();
    }

    void Update()
    {

    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        uiManager.limit.SetActive(true);
        DefaultPos = this.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        //마우스 포인터가 화면 밖에 못나가게
        Cursor.lockState = CursorLockMode.Confined;

        this.transform.position = DefaultPos;
        manager.ButtonNum = Random.Range(0, 6);
        manager.SaveRandom = manager.ButtonNum;
        manager.GClickObject = EventSystem.current.currentSelectedGameObject;

        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100.0f);

       instPos = Camera.main.ScreenToWorldPoint(screenPoint);

        //Debug.Log(CheckBreak());
        if (CheckBreak())
        {
            for (int i = 0; i < 6; i++)
            {
                if (manager.GClickObject.name == uiManager.button[i].name)
                {
                    if(skillE.mana >= manager.Mana)
                    {
                        audio.Play();
                        skillE.mana -= manager.Mana;
                        Instantiate(spawner.pPrefabs[spawner.savearr[i]], instPos, Quaternion.identity);

                        uiManager.limit.SetActive(false);

                        uiManager.images[i].sprite = uiManager.sprites[manager.ButtonNum];
                        spawner.savearr[i] = manager.ButtonNum;
                    }
                    break;
                }
            }
        }
        else
            uiManager.limit.SetActive(false); 
    }

    bool CheckBreak()
    {
            return (instPos.x >= 52);
    }
}
