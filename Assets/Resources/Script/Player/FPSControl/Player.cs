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
    private bool mode_chage;


    RaycastHit rayHit;
    Transform Gun;
    GunSystem gunSystem;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        Gun = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0)
            .GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5);
        //총의 시스템을 사용하는 것임으로 활성화 된 총기가 있는지
        //내가 현재 손에 들고 있는 총이 맞는지 확인
        gunSystem = Gun.GetChild(0).GetComponent<GunSystem>();

        damegeSystem = GetComponent<DamegeSystem>();
        //hpBar.SetMaxHealth(maxHealth);
    }

    public void Initialize()
    {
        life = true;
        hp.Initialize(maxHealth, maxHealvolume, life);
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    private void Update()
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playercam.gameObject.SetActive(mode_chage);
            //Inventory.SetActive(!mode_chage);
            mode_chage = !mode_chage;
        }

    

        //Debug.DrawRay(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward * 5f, Color.black, 0.2f);
        //플레이어가 총을 쥡기위한 범위 내에 있는지와 E키가 눌렸는지 확인
        if (Physics.Raycast(GameMgr.Instance.player.transform.localPosition, GameMgr.Instance.player.transform.GetChild(1).forward, out rayHit,  4f, Item_Mask))
        {
            /*Debug.Log("보고있는 아이템");
            Debug.Log(rayHit.transform.parent.GetComponent<GroundItem>());*/
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUp(rayHit);
            }
        }

        //itemDatabase save & load test code
        /*if (Input.GetKeyDown(KeyCode.End))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
        }*/
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1번눌림" );
            GunActive(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GunActive(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GunActive(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GunActive(3);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playercam.gameObject.SetActive(mode_chage);
            InventoryUI.SetActive(!mode_chage);
            mode_chage = !mode_chage;  
        }

        CuserControl(mode_chage);
        
    }
    void CuserControl(bool mode_chage)
    {
        //커서를 움직이지 않도록 하고 보이지 않도록 해야함.
        if (mode_chage == false) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = mode_chage;
    }



    public bool Getlife()
    {
        return hp.GetLife();
    }

    //데이터 관리 클래스
    public void OnApplicationQuit()
    {
        //@슬롯
/*        
        Groundinventory.Container.Clear();
        inventory.Container.Clear();
        Equipinventory.Container.Clear();
        Guninventory.Container.Clear();
*/
    }

}
