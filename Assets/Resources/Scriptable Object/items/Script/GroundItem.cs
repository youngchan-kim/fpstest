using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;
    //데이터 바뀌도록할것
    public int amount;
    Transform PlayerObject;
    private void Start()
    {
        PlayerObject = GameMgr.Instance.player.transform;
    }
    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        //GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        //EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }

    // 오브젝트 사이의 접촉이 일어난 순간 호출
    public void OnTriggerEnter(Collider other)
    {
         
        if(amount == 0)
        {
            amount = item.data.amount;
        }
        
        Debug.Log("PlayerObject의 이름" + PlayerObject.name);
        Debug.Log("other의 이름" + other.name);
        if (PlayerObject.name == other.name)
        {
            item.data.item_object = gameObject;

                GameMgr.Instance.player.GetComponent<Player>().On_The_Ground_Item(item/*.data*/, amount);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (PlayerObject.name == other.name) 
        {

            GameMgr.Instance.player.GetComponent<Player>().On_The_Ground_Item_Removed(item);
           
        }
    }    
}
