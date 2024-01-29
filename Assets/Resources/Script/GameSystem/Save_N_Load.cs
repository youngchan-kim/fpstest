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
using System.Numerics;
using static UnityEngine.EventSystems.EventTrigger;

public class Save_N_Load : MonoBehaviour
{
    //public Save_Path save_path;
    Player player;
    Enemy enemy;
    Item_Mgr item_mgr;
    GunSystem gun;
    Field_item_List field_list;
    invetory_Item_List inventory_list;
    Player_Data player_info;
    Enemy_Data enemy_info;
    [SerializeField]
    string savepath_FieldItem_object, savepath_Field_item_list, savepath_Inven_item_list, savepath_Gun_item_list;
    // Start is called before the first frame update
    void Start()
    {
        player = GameMgr.Instance.player.GetComponent<Player>();
        enemy = GameMgr.Instance.enemy.GetComponent<Enemy>();
        item_mgr = GameMgr.Instance.Item_Mgr.GetComponent<Item_Mgr>();
        savepath_FieldItem_object = "/FieldItem_object.json";
        savepath_Field_item_list = "/Field_item_list.json";
        savepath_Inven_item_list = "/Inven_item_list.json";
        savepath_Gun_item_list = "/Gun_item_list.json";

    }
    public  void Update()
    {
        //itemDatabase save & load test code
        if (Input.GetKeyDown(KeyCode.End))
        {
            GameSave();

        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameLoad();
        }
    }

    public void GameSave()
    {
        FieldItemSave(savepath_FieldItem_object);
        inventoryItemSave(player.Groundinventory, savepath_Field_item_list);
        inventoryItemSave(player.inventory, savepath_Inven_item_list);
        inventoryItemSave(player.Guninventory, savepath_Gun_item_list);
        PlayerSave("/Player.json");
        EnemySave("/Enemy.json");
    }

    public void GameLoad()
    {
        FieldItemLoad(savepath_FieldItem_object);
        Itemlist_ReSet(player.Groundinventory);
        inventoryItemLoad(player.Groundinventory, InventoryType.Ground, savepath_Field_item_list);
        inventoryItemLoad(player.inventory, InventoryType.Inven, savepath_Inven_item_list);
        GunLoad(player.Guninventory, InventoryType.Gun, savepath_Gun_item_list);
        PlayerLoad("/Player.json");
        EnemyLoad("/Enemy.json");
    }
    void FieldItemSave(string SavePath)
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
        TO_JSON_Save(SavePath, field_list, "필드 아이템");
       /* string field_item_list = JsonUtility.ToJson(field_list);
        string path = Application.persistentDataPath+ SavePath;
        File.WriteAllText(path, field_item_list);
        Debug.Log("맵 상의 모든 아이템의 저장");*/
    }

    void FieldItemLoad(string SavePath)
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
    void inventoryItemSave(InventoryObject _inventoryObject, string SavePath)
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
        TO_JSON_Save(SavePath, inventory_list, "인벤토리");
      /*  string Inventory_item_list = JsonUtility.ToJson(inventory_list);
        string path = Application.persistentDataPath + SavePath;
        File.WriteAllText(path, Inventory_item_list);
        Debug.Log(_inventoryObject.name + "의 모든 아이템의 저장");*/
    }
    void Itemlist_ReSet(InventoryObject _inventoryObject)
    {
        int usingSlotCount = _inventoryObject.SlotCount;
        for (int i = 0; i < usingSlotCount; i++)
        {
            _inventoryObject.Container.Items[i].RemoveItem();
        }
    }
    void inventoryItemLoad(InventoryObject _inventoryObject, InventoryType inventoryType, string SavePath)
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
    void GunLoad(InventoryObject _inventoryObject, InventoryType inventoryType, string SavePath)
    {
        inventoryItemLoad(_inventoryObject, inventoryType, SavePath);
        player.Gun_Load();
    }

    void PlayerSave(string SavePath)
    {

        //인스턴스 생성
        player_info = new Player_Data();
        //grounds의 배열의 갯수만큼 인스턴스 생성
        player_info.playerdata = new Player_Information (player.transform,  player.hp.GetCurHp());

        TO_JSON_Save(SavePath, player_info, "플레이어");
/*        string player_information = JsonUtility.ToJson(player_info);
        string path = Application.persistentDataPath + SavePath;
        File.WriteAllText(path, player_information);
        Debug.Log("플레이어 정보 저장");*/
    }

    void PlayerLoad(string SavePath)
    {

        string path = Application.persistentDataPath + SavePath;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            player_info = JsonUtility.FromJson<Player_Data>(jsonData);
            Player_Information player_data = player_info.playerdata;
            player.transform.position = new UnityEngine.Vector3(player_data.x, player_data.y, player_data.z);
            player.hp.SetCurHp(player_data.hp);
            //list의 아이디와 비교하여 맞는 아이템을 스폰해줄것
            //field_list
            Debug.Log("필드아이템 오브젝트 로드");
        }
    }

    void EnemySave(string SavePath)
    {
        Enemy[] Alive_Enemy = FindObjectsOfType<Enemy>();

        //인스턴스 생성
        enemy_info = new Enemy_Data();
        //grounds의 배열의 갯수만큼 인스턴스 생성
        enemy_info.enemydata = new Enemy_Information[Alive_Enemy.Length];

        for (int i = 0; i < Alive_Enemy.Length; i++ )
        {
            enemy_info.enemydata[i] = new(Alive_Enemy[i].transform, Alive_Enemy[i].GetComponent<HP_System>().GetCurHp());
        }
        TO_JSON_Save(SavePath, enemy_info, "적");
/*        string enemy_information = JsonUtility.ToJson(enemy_info);
        string path = Application.persistentDataPath + SavePath;
        File.WriteAllText(path, enemy_information);
        Debug.Log("적 정보 저장");*/
    }

    void EnemyLoad(string SavePath)
    {

        string path = Application.persistentDataPath + SavePath;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            enemy_info = JsonUtility.FromJson<Enemy_Data>(jsonData);
            for(int i =0; i < enemy_info.enemydata.Length; i++)
            {
                Enemy_Information enemy_data = enemy_info.enemydata[i];
                enemy.transform.position = new UnityEngine.Vector3(enemy_data.x, enemy_data.y, enemy_data.z);
                enemy.hp.SetCurHp(enemy_data.hp);
            }
            Debug.Log("적 로드");
        }
    }

    void TO_JSON_Save<T>(string SavePath, T info, string savename)
    {
        string information = JsonUtility.ToJson(info);
        string path = Application.persistentDataPath + SavePath;
        File.WriteAllText(path, information);
        Debug.Log(savename + " 정보 저장");
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
    public float x, y, z;
    public float hp;

    public Player_Information(Transform _transfrom, float _hp)
    {
        x = _transfrom.position.x;
        y = _transfrom.position.y;
        z = _transfrom.position.z;
        hp = _hp;
    }
}
[System.Serializable]
public class Player_Data
{
    public Player_Information playerdata;
}
[System.Serializable]
public class Enemy_Information
{
    public float x, y, z;
    public float hp;

    public Enemy_Information(Transform _transfrom, float _hp)
    {
        x = _transfrom.position.x;
        y = _transfrom.position.y;
        z = _transfrom.position.z;
        hp = _hp;
    }
}

[System.Serializable]
public class Enemy_Data
{
    public Enemy_Information[] enemydata;
}

[CreateAssetMenu(fileName = "New SavePath", menuName = "Save System/SavePath")]
public class Save_Path : ScriptableObject
{
    public string SavePath_FieldItem;
    public string SavePath_InvenItem;
    public string SavePath_FieldPlayer;
    public string SavePath_FieldEnemy;
}