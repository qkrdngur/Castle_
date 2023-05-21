using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class DrogAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static Vector2 DefaultPos, currentPos, mousePos;
    public static Vector3 instPos;

    [SerializeField]
    private EnemyManager manager;
    public EnemyManager EnemyManager { set { manager = value; } }

    private UiManager uiManager;
    private EnemySpawner spawner;

    void Awake()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = this.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = eventData.position;
       // currentPos = eventData.position;

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

        if (instPos.x! <= 52)
        {
            for (int i = 0; i < 5; i++)
            {
                if (manager.GClickObject.name == uiManager.button[i].name)
                {
                    Instantiate(spawner.enemyPrefabs[spawner.savearr[i]], instPos, Quaternion.identity);

                    uiManager.images[i].sprite = uiManager.sprites[manager.ButtonNum];
                    spawner.savearr[i] = manager.ButtonNum;
                    break;
                }
            }
        }
    }
}
