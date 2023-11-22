using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunPickUp : PickUpController
{
    //총기 시스템을 가지고 있는 object를 연결
    public GunSystem gunScript;
    //스코프 시스템을 가지고 있는 object를 연결
    public Scope scopeScript;

    //object의 장착 여부
    //public bool equipped;
    //이미 총을 들고 있는지 확인
    //모든 스크립트에서 변경하기 위함
    public static bool slotFull;

    public Image Equit_icon;
    public Sprite nomal_icon;
    Sprite after_object;


    /*public bool GetEquipped()
    {
        return equipped;
    }*/

    void Start()
    {
        //Setup
        switch (equipped)
        {
            case false:
                gunScript.enabled = false;
                scopeScript.enabled = false;

                break;

            case true:

                gunScript.enabled = true;
                scopeScript.enabled = true;
                slotFull = true;
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
/*        if (!equipped && Input.GetKeyDown(KeyCode.F) && !slotFull)
            PickUpItem();

        //플레이어가 아이템을 가지고 있는지 체크와 G키를 통해 내려놓음
        if (equipped && Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }*/
    }

    private void PickUpItem()
    {

        equipped = true;
        slotFull = true;

        //총의 스크립스 활성화
        gunScript.enabled = true;
        scopeScript.enabled = true;
        //Equit_icon.sprite = gunScript.GetSprite();
    }

    private void DropItem()
    {
        equipped = false;
        slotFull = false;

        //총의 스크립스 비활성화
        gunScript.enabled = false;
        scopeScript.OnUnScoped();
        scopeScript.enabled = false;

        Equit_icon.sprite = nomal_icon;
    }
}
