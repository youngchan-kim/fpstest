using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Item_Mgr : MonoBehaviour
{
    [SerializeField]
    public GameObject[] item =new GameObject[5];
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

}
