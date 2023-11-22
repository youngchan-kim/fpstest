using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using static UnityEditor.Progress;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    
    public int InventoryID;
    public string savePath;
    //public List<ItemObject> Container = new List<ItemObject>(); 에서로 변경
    //private ItemDatabaseObject database;
    public ItemDatabaseObject database;
    //Inventory클래스의 명은 Container
    //Container를 사용하면 List Items를 사용하기위함
    public Inventory Container;



    //아이템 항목 추가 기능
    public bool AddItem(Item _item, InventoryType inventoryType, int itemamount)
    {
        if (EmptySlotCount <= 0)
            return false;

        InventorySlot slot = FindItemOnInventory(_item);
        if (inventoryType == InventoryType.Ground)
        {
            SetEmptySlot(_item, itemamount);

            return true;
        }
        else if (inventoryType == InventoryType.other)
        {            
            if (!database.Items[_item.Id].stackable || slot == null)
            {
                SetEmptySlot(_item, itemamount);

                return true;
            }
            slot.AddAmount(slot.item, itemamount);
        }
        return true;
    }

    //Ground
  public bool AddGroundItem(Item _item, int itemamount)
    {
        if (EmptySlotCount <= 0)
            return false;
        SetEmptySlot(_item, itemamount);
        return true;
    }


    //내부의 첫 번째 빈 슬롯을 찾는함수
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id == _item.Id)
            {
                return Container.Items[i];
            }
        }
        return null;
    }

    //빈슬롯에 설정
    public InventorySlot SetEmptySlot(Item _item, int _amount )
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id <= -1)
            {
                Container.Items[i].UpdateSlot(_item, _amount);
                return Container.Items[i];
            }
        }
        //set up functionality for full inventory
        return null;
    }


    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.Id == -1)
            {
                return Container.Items[i];
            }
        }
        //full inventory
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject)&& item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.totalamount);
            item2.UpdateSlot(item1.item, item1.totalamount);
            item1.UpdateSlot(temp.item, temp.totalamount);
        }
    }
    public void Sort()
    {      
        for (int i = 0; i < Container.Items.Length-1; i++)
        {
            //갯수 없으면 삭제
            if(Container.Items[i].item.Id != -1 && Container.Items[i].totalamount ==0)
            {
                Container.Items[i].RemoveItem();
            }
            if ((Container.Items[i].item.Id == -1) && (Container.Items[i + 1].item.Id >= -1))
            {
                Container.Items[i].slot_item_object = Container.Items[i + 1].slot_item_object;
                Container.Items[i].SortSlot(Container.Items[i + 1].item, Container.Items[i + 1].totalamount);
                Container.Items[i + 1].RemoveItem();
            }
            else if ((Container.Items[i].item.Id == -1) && (Container.Items[i + 1].item.Id == -1))
            {
                break;
            }
        }
       
    }
    public int ItemSlotNum(InventorySlot sellectslot)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i] == sellectslot)
            {
                return i;
            }
        }
        return -1;
    }



    //#
    public void ClearItem(int _Item_Id, int inven_num)
    {
        
        for (int i = 0; i < Container.Items.Length; i++)
        {
            //문제 //아이디 값이 같은데 걍 넘어감
            if (Container.Items[i].item.Id == _Item_Id)
            {
                if (inven_num != 0 && inven_num != -1)
                {
                    Destroy(Container.Items[i].slot_item_object); 
                }

                Container.Items[i].RemoveItem();
                break;
            }
        }
    }

    //인벤토리 인스펙터 창에서 저장하기위해 ContextMenu에 노출
    [ContextMenu("Save")]
    public void Save()
    {
        //설명
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //SerializationException: Type 'UnityEngine.GameObject' in Assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' is not marked as serializable.발생
        formatter.Serialize(stream, Container); 
        stream.Close();
        Debug.Log("인벤토리 저장");
    }
    //
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for(int i =0; i<Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].item, newContainer.Items[i].item.amount);
            }
            stream.Close();
            Debug.Log("인벤토리 로드");
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Debug.Log("clear");
        Container.Clear();
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];

    [System.NonSerialized]
    public UserInterface parent;
    public string name;
    public Item item = new Item();
    [HideInInspector]public GameObject slot_item_object;
    public int totalamount;
    public ItemObject ItemObject
    {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventory.database.Items[item.Id];
            }
            return null;
        }
    }

    public int GetInventoryID()
    {
        return parent.inventory.InventoryID;
    }

    //스왑할때 사용

    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
         totalamount = _item.amount;
        //totalamount = -1;
    }
    //저장할때 주로 사용
    public void UpdateSlot(Item _item, int _amount)
    {
        item = _item;
        totalamount = _amount;
        slot_item_object = _item.item_object;
    }
    public void SortSlot(Item _item, int _amount)
    {
        item = _item;
        totalamount = _amount;
    }

    public void RemoveItem()
    {
        item = new Item();
        totalamount = 0;
    }
    public void AddAmount(Item _item, int value)
    {
        totalamount += _item.amount;
    }

    //허용된 슬롯만 가능
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null ||_itemObject.data.Id < 0)
            return true;

        for(int i =0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}