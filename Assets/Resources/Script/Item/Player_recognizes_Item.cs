using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;
/*using static UnityEngine.Rendering.VolumeComponent;*/


public enum InventoryType
{
    Ground,
    other
}

public class Player_recognizes_Item : MonoBehaviour
{
    [SerializeField]
    public InventoryObject Groundinventory, inventory, Equipinventory, Guninventory;
    //private bool player_recognizes = false;
    GroundItem groundItem;
    public bool pickup;

    string itemname;
    GroundItem haveitem;

    GameObject ItemList;

    GameObject ActiveGun;
    //모든 스크립트에서 변경할 수 있게 하기 위함
    public GameObject[] Gunslot =  new GameObject[4];
    //아이템의 실제정보를 가지고 있음
    public static GameObject[] Itemslot = new GameObject[4];
    public bool equipped;
    //아이템을 두기 위한 위치
    //월드 포지션을 가지기 위함
    private Vector3 hitPos;

    int Ojbectslotnum;
    public LayerMask Dropzon_layermask;
    [SerializeField]
    public Transform Gun_Holder;
    public Item_Mgr item_mgr;
    Transform Item_Prefab;

    //키보드로 먹을때
    public void Pickup_Swap_Item(Collider Coll)
    {
        Transform coll_parent = Coll.transform.parent;
        GameObject ground_Item =  coll_parent.gameObject;
        GroundItem ground_stript = coll_parent.GetComponent<GroundItem>();
        Item _item = ground_stript.item.data;   

        if (Groundinventory.FindItemOnInventory(_item) != null)
        {
            switch (ground_stript.item.type)
            {
                case ItemType.Gun:
                    Debug.Log("아이템 픽업");
                    Add_Item(Guninventory.InventoryID, _item, ground_stript.amount, ground_Item);
                    Item_Removed(Groundinventory, Guninventory.InventoryID, _item);
                    break;
                case ItemType.Bullet:
                    Add_Item(inventory.InventoryID, _item, ground_stript.amount, ground_Item);
                    Item_Removed(Groundinventory, inventory.InventoryID, _item);
                    //Groundinventory.SwapItems(Groundinventory.FindItemOnInventory(_item), inventory.GetEmptySlot());
                    break;
                case ItemType.Medical:
                    //아이템 슬롯에 아이템 넣어야함
                    Add_Item(inventory.InventoryID, _item, ground_stript.amount, ground_Item);
                    Item_Removed(Groundinventory, inventory.InventoryID, _item);
                    //힐템은 해당 아이템 상호작용전까지는 비활성화 상태이다.
                    break;
            }
        }
        //_item.groundobject = ground_Item.gameObject;
        ItemPickup_inventory_System(ground_stript.item, ground_Item);
        Groundinventory.Sort();
    }

    //드래그엔드일때 사용
    public void ItemPickup_inventory_System(ItemObject _itemObject, GameObject ground_Item)
        {
        Item _item = _itemObject.data;
        switch(_itemObject.type)
        {
            case ItemType.Gun:
                 //총의 빈 슬롯 체크
                if (GetGunSlotEmpty())
                {

                    ActiveGun = Gun_Item_ID_Check(_item.Id);
                    
                    AdditemObject(Gunslot, ActiveGun);
                    GunActive(0);
                    ground_Item.SetActive(false);

                }
                break;
            case ItemType.Bullet:
                Add_Item(inventory.InventoryID, _item, _itemObject.data.amount, ground_Item);
                Item_Removed(Groundinventory, inventory.InventoryID, _item);
                //총알은 플레이어에게서 버려질때를 제외하면 부모 오브젝트가 없다.
                //오브젝트 비활성화가 불가능하다.
                break;
            case ItemType.Medical:
                //아이템 슬롯에 아이템 넣어야함
                Add_Item(inventory.InventoryID, _item, _itemObject.data.amount, ground_Item);
                //아이템이 원래 있던 슬롯에서 제거해야함
                Item_Removed(Groundinventory, inventory.InventoryID, _item);
                Destroy(ground_Item);
                //힐템은 해당 아이템 상호작용전까지는 비활성화 상태이다.

                break;
        }
    }


    public void ItemDropSystem(InventorySlot obj/*, InventorySlot slot*/)
    {
        GameObject grountitem;
        /*if (crruntitem.groundobject.name == "Gun")
        {
            //플레이어가 총을 장착 여부와 슬롯이 차있는지 여부 확인
            if (equipped)
            {
                equipped = false;
                ItemActiveCheck().SetActive(false);
            }
        }
        else
        {
            //ItemActiveCheck().SetActive(false);
        }*/
        //Debug.Log(obj.slot_item_object);
        Debug.Log(transform.name);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit Hit, 10f))
        {
            Item_Prefab = GameMgr.Instance.Item_Mgr.transform;
            Debug.Log("땅 인식1");
            hitPos = Hit.point;
            hitPos.y += 0.01f;
            if (obj.totalamount != 0)
            {
                grountitem = Instantiate(Item_Prefab.GetComponent<Item_Mgr>().Find_ID_RE_Gameobject(obj.item.Id), hitPos, Quaternion.LookRotation(GameMgr.Instance.player.transform.forward));
                grountitem.GetComponent<GroundItem>().amount = obj.totalamount;
            }
        }
    }


    //추가 코드
    // 오브젝트 사이의 접촉이 일어난 순간 호출
    public void On_The_Ground_Item(ItemObject item, int itemamount, GameObject item_object)
    {
        Groundinventory.AddGroundItem(item.data, itemamount, item_object);
    }

    public void On_The_Ground_Item_Removed(ItemObject item)
    {
        Item_Removed(Groundinventory, -1, item.data);
        Groundinventory.Sort();
    }

    public void Add_Item(int invenID, Item item, int itemamount, GameObject Item_object)
    {
        switch (invenID)
        {
            case 1:
                if (inventory.AddItem(item, InventoryType.other, itemamount, Item_object))
                {
                    pickup = true;
                }
                break;
            case 2:
                if (Equipinventory.AddItem(item, InventoryType.other, itemamount, Item_object))
                {
                    pickup = true;
                }
                break;
            case 3:
                if (Guninventory.AddItem(item, InventoryType.other, itemamount, Item_object))
                {
                    pickup = true;
                }
                break;
        }
    }

    public void Gun_Load()
    {
        AdditemObject(Gunslot, Gun_Item_ID_Check(Guninventory.inventory_inside_item_find_Id()));
        GunActive(0);
    }    

    public void Item_Removed(InventoryObject item_out_inven, int inven_num, Item item)
    {
        //Debug.Log("반응 확인");
        item_out_inven.ClearItem(item.Id, inven_num);
        pickup = false;
    }


    /*    //목록체크
    GameObject ListActiveCheck(string name)
    {
        int ListListcount = transform.GetChild(1).GetChild(0).childCount;
        //Debug.Log(ListListcount);

        for (int i = 0; i < ListListcount; i++)
        {
            if (transform.GetChild(1).GetChild(0).GetChild(i).gameObject.name == name)
            {
                return transform.GetChild(1).GetChild(0).GetChild(i).gameObject;
            }
        }
        return null;
    }*/


    /*    //활성화된 오브젝트 리턴
        GameObject ItemActiveCheck()
        {
            int ItemListcount = transform.GetChild(1).GetChild(0).GetChild(0).childCount;
            //Debug.Log(ItemListcount);

            for (int i = 0; i < ItemListcount; i++)
            {
                if (transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.activeSelf == true)
                {

                    return transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject;
                }
            }
            return null;
        }*/

    public int GetInven_Find_Item(ItemObject _item)
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        { 
            if(inventory.Container.Items[i].item.Id == _item.data.Id)
            {
                //int item_amount_test = inventory.Container.Items[i].itemobject_data.amount;
                return inventory.Container.Items[i].totalamount;
            }  
        }
        return 0;
    }

    //인벤토이 안의 아이템을 찾아서 초기화
    public void SetInven_Find_Item(ItemObject _item, int num)
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            if (inventory.Container.Items[i].item.Id == _item.data.Id)
            {
                //int item_amount_test = inventory.Container.Items[i].itemobject_data.amount;
                inventory.Container.Items[i].totalamount = num;
            }
        }
    }
    //인벤토리 새로고침
    public void InventRefresh()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            if (inventory.Container.Items[i].totalamount == 0)
            {
                inventory.Container.Items[i].RemoveItem();
            }
        }
    }
    //인벤토리 정렬
    public void Inventsort(int invenID)
    {
        switch (invenID)
        {
            case 0:
                Groundinventory.Sort();
                break;
            case 1:
               inventory.Sort();
                break;
        }
        
    }

    //총의 활성화
    public void GunActive(int sellect)
    {
        if (Gunslot[sellect] != null)
        {
            Gunslot[sellect].SetActive(true);
        }
        GunUnActive(sellect);
    }
    //총의 슬롯이 비었는지 체크
    public bool GetGunSlotEmpty()
    {
        for (int i = 0; i < Gunslot.Length; i++)
        {
            if (Gunslot[i] == null)
                return true;
        }
        return false;
    }

    //아이템슬롯에 해당 아이템을 추가
    void AdditemObject(GameObject[] Itemslot, GameObject item)
    {
        //슬롯번호가 -1이면
        if (Ojbectslotnum == -1)
        {
            //슬롯의 크기만큼 반복
            for (int i = 0; i < Itemslot.Length; i++)
            {
                
                if (Itemslot[i] != null)
                    Debug.Log(i + "번째 슬롯에 " + Itemslot[i] + " 있다.");
                else
                    Debug.Log(i + "번째 슬롯에 없다.");
                //슬롯이 비어있으면  해당슬롯을 아이템오브젝트로 초기화
                if (Itemslot[i] == null)
                {
                    Itemslot[i] = item;
                    return;
                }
            }
        }
        else
        {
            Itemslot[Ojbectslotnum] = item;
        }
        Ojbectslotnum = -1;
    }

    //아이템 슬롯의 번호 설정
    public void ItemOjbectslotnum(int num)
    {
        Ojbectslotnum = num;
    }
    //총 비활성화
    public void RemoveGun(int num)
    {
        for (int i = 0; i < Gunslot.Length; i++)
        {
            if (i == num)
            {
                Gunslot[i].gameObject.SetActive(false);
                Gunslot[i] = null;
                return;
            }
        }
    }
    //총 비활성화
    public void GunUnActive(int sellect)
    {
        for (int i = 0; i < Gunslot.Length; i++)
        {
            if (sellect != i && Gunslot[i] != null)
                Gunslot[i].gameObject.SetActive(false);
        }
    }

    public void GunSlot_swap(int Ojbectslotnum1, int Ojbectslotnum2)
    {
        GameObject temp = Gunslot[Ojbectslotnum2];
        Gunslot[Ojbectslotnum2] = Gunslot[Ojbectslotnum1];
        Gunslot[Ojbectslotnum1] = temp;
        GunActive(Ojbectslotnum2);
    }


    //총의 id 찾기
    GameObject Gun_Item_ID_Check(int Id)
    {
        for (int i = 0; i < Gun_Holder.childCount; i++)
        {
            if (Id == Gun_Holder.GetChild(i).GetComponent<GunSystem>().Gun_property.Id)
            {
                return Gun_Holder.GetChild(i).gameObject;
            }
        }
        return null;
    }

}
