using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : Player_recognizes_Item
{
    // public GunSystem gunScript;
    //public Scope scopeScript;
    //내려지는 레이어
    public LayerMask Item_Mask;

    public virtual void PickUp(RaycastHit rayHit)
    {
        //Debug.Log(rayHit.collider.transform.parent.GetComponent<GroundItem>().This_Item_info());
        //충돌한 아이템의 그라운드 아이템과 아이템을 매개변수로 사용
        Pickup_Swap_Item(rayHit.collider);
    }

    public bool GetEquipped()
    {
        //Debug.Log("장착 : " + equipped);
        return equipped;
    }


    ///인벤토리에서
    ///


  /*  public bool GetGunslotFull()
    {
        //Debug.Log("장착 : " + GunslotFull);
        return GunslotFull;
    }*/

}
