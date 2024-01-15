using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : PickUpController
{
    [Serialize]
    public HP_System hp;
    DamegeSystem damegeSystem;
    //maxHealth, MaxHealvolume을 스크립터블 오브젝트로 만들어 놓기
    float maxHealth = 100f;
    float maxHealvolume = 80f;
    bool life;
    public HPBar hpBar;

    public GameObject playercam;
    public GameObject InventoryUI;
    public bool Hold_contorl_check;
    private bool mode_chage;

    RaycastHit rayHit;
    GunSystem gunSystem;

    Animator anim;

    [SerializeField]
    public Transform head;

    Save_N_Load SL_System;
    void Start()
    {
        anim = GetComponent<Animator>();
        
        damegeSystem = GetComponent<DamegeSystem>();
        InventoryUI = GameMgr.Instance.GUI_Mgr.transform.GetChild(0).GetChild(6).gameObject;
        //hpBar.SetMaxHealth(maxHealth);
        Hold_contorl_check = false;
        SL_System = GetComponent<Save_N_Load>();
    }

    public void Initialize()
    {
        life = true;
        hp.Initialize(maxHealth, maxHealvolume, life);
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void GunSystem_chage()
    {
        //총의 시스템을 사용하는 것임으로 활성화 된 총기가 있는지
        //내가 현재 손에 들고 있는 총이 맞는지 확인

        for (int i = 0; i < Gun_Holder.childCount; i++)
        {
            if (Gun_Holder.GetChild(i).gameObject.activeSelf == true)
            {
                gunSystem = Gun_Holder.GetChild(i).GetComponent<GunSystem>();
                break;
            }
            else
                gunSystem = null;
        }
    }
    public GunSystem GetGunSystem()
    {
        return gunSystem;
    }
    // Update is called once per frame
    private void Update()
    {
        GunSystem_chage();
        if (Hold_contorl_check == false && gunSystem!=null)
        {
           
            if (gunSystem.HoldButtonUse())
            {
                gunSystem.ClickToShoot(Input.GetKey(KeyCode.Mouse0));
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    
                    if (gunSystem.BulletIsEmpty() && gunSystem.ReadyToShoot() && gunSystem.FiringIsPossible())
                    {
                        //anim.Shoot();
                        gunSystem.Firing();
                    }
                }
            }
            else
            {
                gunSystem.ClickToShoot(Input.GetKeyDown(KeyCode.Mouse0));
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (gunSystem.BulletIsEmpty() && gunSystem.ReadyToShoot() && gunSystem.FiringIsPossible())
                    {
                        //anim.Shoot();
                        gunSystem.Firing();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && gunSystem.ReloadIsPossible())
                gunSystem.Reload();
        }


        Debug.DrawRay(playercam.transform.position, transform.GetComponent<Cam_ScreenCenter>().Cam_Dir() * 4f, Color.black, 0.2f);
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        if (Physics.Raycast(playercam.transform.position, transform.GetComponent<Cam_ScreenCenter>().Cam_Dir(), out rayHit, 4f, Item_Mask))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUp(rayHit);
                transform.GetComponent<PlayerMovement>().Pickup_Item_Motion();
            }
            GunSystem_chage();
        }


        //itemDatabase save & load test code
        if (Input.GetKeyDown(KeyCode.End))
        {
            SL_System.FildItemSave();
            inventory.Save();
            Guninventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            Guninventory.Load();
            Gun_Load();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1번눌림" );
            GunActive(0);
            GunSystem_chage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GunActive(1);
            GunSystem_chage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GunActive(2);
            GunSystem_chage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GunActive(3);
            GunSystem_chage();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playercam.gameObject.SetActive(mode_chage);
            InventoryUI.SetActive(!mode_chage);
            mode_chage = !mode_chage;
            CuserControl(mode_chage);
        }
    }
    void CuserControl(bool mode_chage)
    {
        //커서를 움직이지 않도록 하고 보이지 않도록 해야함.
        if (mode_chage == false) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = mode_chage;
        Hold_contorl_check = !Hold_contorl_check;
    }



    public bool Getlife()
    {
        return hp.GetLife();
    }

    //데이터 관리 클래스
    public void OnApplicationQuit()
    {
        //@슬롯
        
        Groundinventory.Container.Clear();
        inventory.Container.Clear();
        Equipinventory.Container.Clear();
        Guninventory.Container.Clear();

    }

}
