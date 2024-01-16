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
    GunSystem gun;

    Field_item_List field_list;
    
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
   

/*    public void Clear()
    {
        for (int i = 0; i < grounds.Length; i++)
        {
            grounds[i].SetIsempty();
            grounds[i] = null;

        }
    }
    public Field_item_information emptyarray()
    {
        for (int i = 0; i < grounds.Length; i++)
        {
            if (grounds[i].Getisnotempty() == false)
                return grounds[i];
        }
        return null;
    }*/
    public void FieldItemSave()
    {
        GroundItem[] groundsitem = FindObjectsOfType<GroundItem>();
        
        field_list = new Field_item_List();
        field_list.grounds = new Field_item_information[groundsitem.Length];
        for (int i = 0; groundsitem.Length > i; i++)
        {
            field_list.grounds[i] = new(groundsitem[i].Get_item_Pos(), groundsitem[i].Get_item_ID());
        }

        string field_item_list = JsonUtility.ToJson(field_list);
        string path = Application.persistentDataPath+ "/FieldItem.json";//Path.Combine(, );
        File.WriteAllText(path, field_item_list);
        Debug.Log("맵 상의 모든 아이템의 저장");
    }
    public void FieldItemLoad()
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
}

//하이어라키 상의 모든 오브젝트중
//아이템들만 찾아서 위치와 아이템 정보만들 저장
[System.Serializable]
public class Field_item_information
{
    public float x,y,z;
    public int itemID;
    public Field_item_information(Vector3 pos, int _ItemID)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        itemID = _ItemID;  
    }
}

[System.Serializable]
public class Field_item_List
{
    public Field_item_information[] grounds;
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