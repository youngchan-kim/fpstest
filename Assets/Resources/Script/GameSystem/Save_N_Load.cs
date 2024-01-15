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

public class Save_N_Load : MonoBehaviour
{
    public Save_Path save_path;
    Player player;
    GunSystem gun;
    public Fielditems fielditems;
    SaveFieldItem savefielditem;
    // Start is called before the first frame update
    void Start()
    {
        player = GameMgr.Instance.player.GetComponent<Player>();
    }
    void inventorySave(string savePath)
    {
        IFormatter formatter = new BinaryFormatter();
        // C:\Users\A-01\AppData\LocalLow\DefaultCompany 경로에 파일이 생성됨
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);

    }
    public void FildItemSave()
    {
        fielditems.SaveList();
        savefielditem.Save("/FieldItem.Save");
    }
    void PlayerSave(string savePath)
    {

    }
    void Load()
    {
        player.inventory.Load();
        player.Guninventory.Load();
    }
}

//하이어라키 상의 모든 오브젝트중
//아이템들만 찾아서 위치와 아이템 정보만들 저장
[System.Serializable]
public class Field_item_information
{
    Vector3 pos;
    int itemID;
    public void Get_Save_Information(Vector3 _pos, int _ItemID)
    {
        pos = _pos;
        itemID = _ItemID;  
    }
}
[System.Serializable]
public class Field_item_List
{
    Field_item_information[] grounds = new Field_item_information[28];

    public void Clear()
    {
        for (int i = 0; i < grounds.Length; i++)
        {
            grounds[i] = null;
        }
    }
    public Field_item_information emptyarray()
    {
        for (int i = 0; i < grounds.Length; i++)
        {
            if(grounds[i] == null)
                return grounds[i];
        }
        return null;
    }
}
//하이어라키 상의 모든 오브젝트중
//아이템들만 찾아서 위치와 아이템 정보만들 저장
public class Fielditems : MonoBehaviour
{
    GroundItem[] groundsitem = FindObjectsOfType<GroundItem>();
    public Field_item_List field_list;
    //함수 실행시 모든 필드에 있는 아이템의 최소정보가 저장됨
    public void SaveList()
    {
        foreach (GroundItem item in groundsitem)
        {
            if (field_list.emptyarray() != null)
                field_list.emptyarray().Get_Save_Information(item.Get_item_Pos(), item.Get_item_ID());
            else
                Debug.Log("필드에 있는 아이템의 저장 슬롯이 부족함");
        }
    }
}

[System.Serializable]
public class SaveFieldItem
{
    Field_item_List field_list;
    Vector3 pos;
    Item item;

    public void Save(string savePath)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, field_list);
        stream.Close();
        Debug.Log("맵 상의 모든 아이템의 저장");
    }
}

[System.Serializable]
public class SaveinvetoryItem
{
    int InvenID;
    int slot_num;
    Item item;
    int totalcount;
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