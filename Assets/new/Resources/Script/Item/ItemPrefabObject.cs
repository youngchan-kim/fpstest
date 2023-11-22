using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabObject : MonoBehaviour
{
    public GameObject Itemobject;
    
    public void SetObject(GameObject _gameobject)
    {
        Itemobject = _gameobject;
    }

    public void RemoveObject()
    {
        Itemobject = null;
    }
    public GameObject GetObject()
    {
        if (Itemobject == null)
            return null;
        return Itemobject;
    }
}
