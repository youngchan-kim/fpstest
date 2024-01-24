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

    Player_Data player_info;
    Player_Information playerdata;

    public ItemDatabaseObject itemDatabase;
    string savepath_FieldItem_object, savepath_Field_item_list, savepath_Inven_item_list, savepath_Gun_item_list;
    // Start is called before the first frame update
    void Start()
    {
        player = GameMgr.Instance.player.GetComponent<Player>();
        item_mgr = GameMgr.Instance.Item_Mgr.GetComponent<Item_Mgr>();
        savepath_FieldItem_object = "/FieldItem_object.json";
        savepath_Field_item_list = "/Field_item_list.json";
        savepath_Inven_item_list = "/Inven_item_list.json";
        savepath_Gun_item_list = "/Gun_item_list.json";
    }
   

    public void GameSave(InventoryObject Field, InventoryObject Inven, InventoryObject Gun )
    {
        FieldItemSave(savepath_FieldItem_object);
        //inventoryItemSave(Field, savepath_Field_item_list);
        inventoryItemSave(Inven, savepath_Inven_item_list);
        inventoryItemSave(Gun, savepath_Gun_item_list);
        PlayerSave("/Player.json");
    }
    public void GameLoad(InventoryObject Field, InventoryObject Inven, InventoryObject Gun,
        InventoryType FieldType, InventoryType InvenType, InventoryType GunType
        )
    {
        FieldItemLoad(savepath_FieldItem_object);
        Itemlist_ReSet(Field);
        //inventoryItemLoad(Field, FieldType, savepath_Field_item_list);
        inventoryItemLoad(Inven, InvenType, savepath_Inven_item_list);
        GunLoad(Gun, GunType, savepath_Gun_item_list);
    }
    public void FieldItemSave(string SavePath)
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
        string path = Application.persistentDataPath+ SavePath;
        File.WriteAllText(path, field_item_list);
        Debug.Log("맵 상의 모든 아이템의 저장");
    }

    public void FieldItemLoad(string SavePath)
    {

        item_mgr.FieldItemClear();
        
        string path = Application.persistentDataPath + SavePath;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            field_list = JsonUtility.FromJson<Field_item_List>(jsonData);
            for(int i =0; i < field_list.grounds.Length; i++ )
            {
                item_mgr.Set_item_list(item_mgr.Set_Field_item(field_list.grounds[i]));
            }
            //list의 아이디와 비교하여 맞는 아이템을 스폰해줄것
            //field_list
            Debug.Log("필드아이템 오브젝트 로드");
        }
    }
    //C:\Users\A-01\AppData\LocalLow\DefaultCompany
    //각 인벤토리의 아이템 저장
    public void inventoryItemSave(InventoryObject _inventoryObject, string SavePath)
    {
        //
        int SlotCount = _inventoryObject.SlotCount;
        int UsingSlotCount = _inventoryObject.UsingSlotCount;
        inventory_list = new invetory_Item_List();
        inventory_list.invetory_Items = new invetory_Item_Information[UsingSlotCount];
        for (int i = 0; i < UsingSlotCount; i++)
        {
            InventorySlot itemslot= _inventoryObject.Container.Items[i];
            Item item = itemslot.item;
            inventory_list.invetory_Items[i] = new(_inventoryObject.InventoryID, i, item.Name, item.Id, 
                item.addHeal, item.amount, item.inven_in_active, itemslot.totalamount);
        }

        string Inventory_item_list = JsonUtility.ToJson(inventory_list);
        string path = Application.persistentDataPath + SavePath;
        File.WriteAllText(path, Inventory_item_list);
        Debug.Log(_inventoryObject.name + "의 모든 아이템의 저장");
    }
    public void Itemlist_ReSet(InventoryObject _inventoryObject)
    {
        int usingSlotCount = _inventoryObject.SlotCount;
        for (int i = 0; i < usingSlotCount; i++)
        {
            _inventoryObject.Container.Items[i].RemoveItem();
        }
    }
    public void inventoryItemLoad(InventoryObject _inventoryObject, InventoryType inventoryType, string SavePath)
    {
        Itemlist_ReSet(_inventoryObject);
        string path = Application.persistentDataPath + SavePath;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            inventory_list = JsonUtility.FromJson<invetory_Item_List>(jsonData);
            //list의 아이디와 비교하여 맞는 아이템을 스폰해줄것
            for (int i = 0; i < inventory_list.invetory_Items.Length; i++)
            {
                invetory_Item_Information inven_item = inventory_list.invetory_Items[i];
                Item item = new(inven_item.name, inven_item.id, inven_item.addHeal, inven_item.amount, inven_item.inven_in_active);
                _inventoryObject.AddItem(item, inventoryType, inven_item.totalcount, item_mgr.Find_ItemPrefab(inven_item.id));
                //stackable에 대한 정보가 없음
            }
            //field_list
            Debug.Log("인벤토리 로드");
        }
    }
    public void GunLoad(InventoryObject _inventoryObject, InventoryType inventoryType, string SavePath)
    {
        inventoryItemLoad(_inventoryObject, inventoryType, SavePath);
        player.Gun_Load();
    }

    public void PlayerSave(string SavePath)
    {

        //인스턴스 생성
        player_info = new Player_Data();
        //grounds의 배열의 갯수만큼 인스턴스 생성
        player_info.playerdata = new Player_Information (player.transform, player.GetComponent<PlayerMovement>().orientation.transform,  player.hp.GetCurHp());


        string player_information = JsonUtility.ToJson(player_info);
        string path = Application.persistentDataPath + SavePath;
        File.WriteAllText(path, player_information);
        Debug.Log("플레이어 정보 저장");
    }

    public void PlayerLoad(string SavePath)
    {

        item_mgr.FieldItemClear();

        string path = Application.persistentDataPath + SavePath;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            field_list = JsonUtility.FromJson<Field_item_List>(jsonData);
            for (int i = 0; i < field_list.grounds.Length; i++)
            {
                item_mgr.Set_item_list(item_mgr.Set_Field_item(field_list.grounds[i]));
            }
            //list의 아이디와 비교하여 맞는 아이템을 스폰해줄것
            //field_list
            Debug.Log("필드아이템 오브젝트 로드");
        }
    }
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
    public string name;
    public int id;
    public float addHeal;
    public int amount;
    public bool inven_in_active;
    public int totalcount;

    public invetory_Item_Information(int _InvenID, int _slot_num, string _name, int _Id, 
        float _addHeal, int _amount, bool _inven_in_active, int _totalcount)
    {
        InvenID = _InvenID;
        slot_num = _slot_num;
        name = _name;
        id = _Id;
        addHeal = _addHeal;
        amount = _amount;
        inven_in_active = _inven_in_active;
        totalcount = _totalcount;
    }
}
[System.Serializable]
public  class invetory_Item_List
{
    public invetory_Item_Information[] invetory_Items;
}
[System.Serializable]
public class Player_Information
{
    //플레이어의 위치, 체력,방향
    public float x,y,z;
    public float ro_x, ro_y, ro_z;
    public float hp;

    public Player_Information(Transform _transfrom, Transform Dir, float _hp)
    {
        x = _transfrom.position.x;
        y = _transfrom.position.y;
        z = _transfrom.position.z;
        ro_x = Dir.rotation.eulerAngles.x;
        ro_y = Dir.rotation.eulerAngles.y;
        ro_z = Dir.rotation.eulerAngles.z;
        hp = _hp;
    }
}
[System.Serializable]
public class Player_Data
{
    public Player_Information playerdata;
}
[System.Serializable]
public class SaveEnemys
{
    Vector3 pos;
}

[CreateAssetMenu(fileName = "New SavePath", menuName = "Save System/SavePath")]
public class Save_Path : ScriptableObject
{
    public string SavePath_FieldItem;
    public string SavePath_InvenItem;
    public string SavePath_FieldPlayer;
    public string SavePath_FieldEnemy;
}