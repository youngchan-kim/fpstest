using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class Save_N_Load : MonoBehaviour
{
    Player player;
    GunSystem gun;
    string savePath;
    // Start is called before the first frame update
    void Start()
    {
        savePath = "/save";
        player = GameMgr.Instance.player.GetComponent<Player>();
    }
    void inventorySave(string savePath)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);

    }
    void FiedItemSave(string savePath)
    {

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

//하이어라키 상의 모든 오브젝트중 아이템들만 찾아서 위치와 아이템 정보만들 저장
public class GroundItems
{
    Vector3 pos;
    Item item;

}

[System.Serializable]
public class SaveGroundItem
{
    Vector3 pos;
    Item item;

    public void Save(string savePath)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
       /* formatter.Serialize(stream, Container);
        stream.Close();
        Debug.Log("맵 상의 모든 아이템의 저장");*/
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