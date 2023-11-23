using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Helmet,
    //방탄복
    Armor,
    Bag,
    Gun,
    Medical,    
    Bullet,
    Chest,
    Boots,
    //기타
    Default
}

//당장은 쓰지 않는 버프 종류
//도핑시스템에 사용됨
//스피드업
//도핑치유
public enum Attributes
{ 
    Dopping
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
public class ItemObject : ScriptableObject
{
    
    public Sprite uiDisplay;
    public GameObject characterDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item();
    /*public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }*/
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;
    public float addHeal;
    public int amount;
    [HideInInspector] public GameObject item_object;
    public GameObject item_object_Prefab;
    public bool inven_in_active;
    //public ItemType itemType;

    public Item()
    {
        Name = "";
        Id = -1;
    }

    /*public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }*/
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}