using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Item_Mgr : MonoBehaviour
{
    public  GameObject[] item = new GameObject[20];
    public Item_Scriptable Item_Scriptable;
    public void Initialize()
    {
        GroundItem[] fielditem = FindObjectsOfType<GroundItem>();
        for (int i = 0; i < fielditem.Length; i++)
        {
            item[i] = fielditem[i].gameObject;
        }

    }
    //아이디를 비교하여 아이템의 오브젝트를 리턴해주는 함수 Find_ID_RE_Gameobject
    public GameObject Find_ID_RE_Gameobject(int id)
    {
        for(int i=0; i < item.Length; i++)
        {
            if(id == item[i].gameObject.GetComponent<GroundItem>().Get_item_ID())
            {
                return item[i];
            }
        }
        return null;
    }
    //필드 아이템을 초기화하는 함수
    public void FieldItemClear()
    {
        for (int i = 0; i < item.Length; i++)
        {
            if (item[i] != null)
            {
                Destroy(item[i].gameObject);
                item[i] = null;
            }
        }
    }
    public void SortItemList()
    {
        for (int i = 0; i < item.Length-1; i++)
        {
            item[i] = null;
        }
        Initialize();
    }
    //아이템 프리펩을 찾아주는 함수
    public GameObject Find_ItemPrefab(int id)
    {
        for (int i = 0; i < Item_Scriptable.item_Prefab.Length; i++)
        {
            GameObject itemobject = Item_Scriptable.item_Prefab[i];
            if (id == itemobject.GetComponent<GroundItem>().Get_item_ID())
            {
                return itemobject;
            }
        }
        return null;
    }

    //필드에 아이템을 생성해주는 함수
    public GameObject Set_Field_item(Field_item_information Field_item)
    { 
        for(int i =0; i < Item_Scriptable.item_Prefab.Length; i++)
        {
            GameObject item = Find_ItemPrefab(Field_item.itemID);
            if (item != null)
            {
                GameObject Instantiatetitem = Instantiate(item, new Vector3(Field_item.x, Field_item.y, Field_item.z), Quaternion.Euler(Field_item.ro_x, Field_item.ro_y, Field_item.ro_z));
                return Instantiatetitem;
            }
        }
        return null;
    }

    public void Set_item_list(GameObject gameObject)
    {
        for(int i =0; i < item.Length; i++)
        {
            if (item[i] == null) 
            { 
                item[i] = gameObject;
                break;
            }
        }
           
    }
}
