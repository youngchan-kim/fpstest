using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;
using Unity.VisualScripting;
using static UnityEditor.Progress;
using System.ComponentModel;
using System.Xml.Linq;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;

public class Save_N_Load : MonoBehaviour
{
    //public Save_Path save_path;
    Player player;
    Item_Mgr item_mgr;
    GunSystem gun;

    Field_item_List field_list;

    invetory_Item_List inventory_list;
    // Start is called before the first frame update
    void Start()
    {
        player = GameMgr.Instance.player.GetComponent<Player>();
        item_mgr = GameMgr.Instance.Item_Mgr.GetComponent<Item_Mgr>();
    }
   
    public void FieldItemSave()
    {
        //하이어라키 상의 모든 grounditem컴포넌트를 가진 오브젝트를 배열로 반환
        GroundItem[] groundsitem = FindObjectsOfType<GroundItem>();
        
        //인스턴스 생성
        field_list = new Field_item_List();
        //grounds의 배열의 갯수만큼 인스턴스 생성
        field_list.grounds = new Field_item_information[groundsitem.Length];
        for (int i = 0; groundsitem.Length > i; i++)
        {
            field_list.grounds[i] = new(groundsitem[i].Get_item_Transform(), groundsitem[i].Get_item_ID());
        }

        string field_item_list = JsonUtility.ToJson(field_list);
        string path = Application.persistentDataPath+ "/FieldItem.json";
        File.WriteAllText(path, field_item_list);
        Debug.Log("맵 상의 모든 아이템의 저장");
    }

    public void FieldItemLoad()
    {
        item_mgr.FieldItemClear();
        
        string path = Application.persistentDataPath + "/FieldItem.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            field_list = JsonUtility.FromJson<Field_item_List>(jsonData);
            for(int i =0; i < field_list.grounds.Length; i++ )
            {
                item_mgr.Set_Field_item(field_list.grounds[i]);
            }
            //list의 아이디와 비교하여 맞는 아이템을 스폰해줄것
            //field_list
            Debug.Log("인벤토리 로드");
        }



    }
    //C:\Users\A-01\AppData\LocalLow\DefaultCompany
    public void inventoryItemSave(InventoryObject _inventoryObject, string SavePath)
    {
        /*int SlotCount = _inventoryObject.SlotCount;
        inventory_list = new invetory_Item_List();
        inventory_list.invetory_Items = new invetory_Item_Information[SlotCount];
        //public invetory_Item_Information(int _InvenID, int _slot_num, Item _item, int _totalcount)
        for (int i = 0; i < SlotCount; i++)
        {
            InventorySlot itemslot= _inventoryObject.Container.Items[i];
            inventory_list.invetory_Items[i] = new(_inventoryObject.InventoryID, i, itemslot.item, itemslot.totalamount);
        }

        string Inventory_item_list = JsonUtility.ToJson(inventory_list);
        string path = Application.persistentDataPath + SavePath;
        File.WriteAllText(path, Inventory_item_list);
        Debug.Log("맵 상의 모든 아이템의 저장");*/
    }
    /*
    public void inventoryItemLoad()
    {
        string path = Application.persistentDataPath + "/FieldItem.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            field_list = JsonUtility.FromJson<Field_item_List>(jsonData);
            //list의 아이디와 비교하여 맞는 아이템을 스폰해줄것
            //field_list
            Debug.Log("인벤토리 로드");
        }
    }
    */
}

//하이어라키 상의 모든 오브젝트중
//아이템들만 찾아서 위치와 아이템 정보만들 저장
[System.Serializable]
public class Field_item_information
{
    public float x,y,z;
    public float  ro_x, ro_y, ro_z;
    public int itemID;
    public Field_item_information(Transform _transfrom, int _ItemID)
    {
        x = _transfrom.position.x;
        y = _transfrom.position.y;
        z = _transfrom.position.z;
        ro_x = _transfrom.rotation.eulerAngles.x;
        ro_y = _transfrom.rotation.eulerAngles.y;
        ro_z = _transfrom.rotation.eulerAngles.z;
        itemID = _ItemID;  
    }
}

[System.Serializable]
public class Field_item_List
{
    public Field_item_information[] grounds;
}

[System.Serializable]
public class invetory_Item_Information
{
    //인벤토리 아이디가 필요한가?
    public int InvenID;
    public int slot_num;
    Item item;
    int totalcount;

    public invetory_Item_Information(int _InvenID, int _slot_num, Item _item, int _totalcount)
    {
        InvenID = _InvenID;
        slot_num = _slot_num;
        item = _item;
        totalcount = _totalcount;
    }
}
[System.Serializable]
public  class invetory_Item_List
{
    public invetory_Item_Information[] invetory_Items;
}
[System.Serializable]
public class SavePlayer
{
    Vector3 pos;
    Player Player;
}
[System.Serializable]
public class SaveEnemys
{
    Vector3 pos;
    Enemy enemy;
}

[CreateAssetMenu(fileName = "New SavePath", menuName = "Save System/SavePath")]
public class Save_Path : ScriptableObject
{
    public string SavePath_FieldItem;
    public string SavePath_InvenItem;
    public string SavePath_FieldPlayer;
    public string SavePath_FieldEnemy;
}