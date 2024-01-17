using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Item_Mgr : MonoBehaviour
{
    [SerializeField]
    public GameObject[] item =new GameObject[5];
    public Item_Scriptable Item_Scriptable;
    public void Initialize()
    {

    }
    public GameObject Find_Prefab(int id)
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
    public void FieldItemClear()
    {
        for (int i = 0; i < item.Length; i++)
        {
            Destroy(item[i].gameObject);
        }
    }
    public void Set_Field_item(Field_item_information Field_item)
    {
        for(int i =0; i < Item_Scriptable.item_Prefab.Length; i++)
        {
            if(Item_Scriptable.item_Prefab[i].GetComponent<GroundItem>().Get_item_ID() == Field_item.itemID)
            {
                Instantiate(Item_Scriptable.item_Prefab[i], new Vector3(Field_item.x, Field_item.y, Field_item.z)
                    , Quaternion.Euler(Field_item.ro_x, Field_item.ro_y, Field_item.ro_z));
            }
        }
    
    }
}
