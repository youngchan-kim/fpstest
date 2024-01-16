using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class fieldinfor : MonoBehaviour
{

    string[] testnum = new string[10]; 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.End))
        {
            Save("/test.save");
        }
    }

    public void Save(string savePath)
    {
        int i = 0;
        GroundItem[] groundsitem = FindObjectsOfType<GroundItem>();
        foreach (GroundItem item in groundsitem)
        {
            testnum[i] = i.ToString();
            i++;
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, testnum);
        stream.Close();
        Debug.Log("맵 상의 모든 아이템의 저장");
    }
}

/*[System.Serializable]
public class field
{
    GroundItem[] groundsitem = new GroundItem[0];
      //  FindObjectsOfType<GroundItem>();
    void find()
    {
        
    }

}*/