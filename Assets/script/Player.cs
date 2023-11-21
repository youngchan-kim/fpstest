using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    //public MouseItem mouseItem = new MouseItem();
    //maxHealth, MaxHealvolume을 스크립터블 오브젝트로 만들어 놓기
    float maxHealth = 100f;
    float maxHealvolume = 80f;
    bool life;
    //float healvolume = 30;
    [Serialize]
    public HP_System hp;
    /*public HP_System HP
    {
        get
        {
            if (hp == null) hp = GetComponent<HP_System>();

            return hp;
        }
    }*/
    DamegeSystem damegeSystem;

    public GameObject playercam;

    //public GameObject InventoryUI;
    private bool mode_chage;


    RaycastHit rayHit;
    // Start is called before the first frame update
    //Player body Object
    //[HideInInspector] 

    Transform Gun;
    GunSystem gunSystem;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        Gun = transform.GetChild(0).GetChild(4).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5);
        //총의 시스템을 사용하는 것임으로 활성화 된 총기가 있는지
        //내가 현재 손에 들고 있는 총이 맞는지 확인
        gunSystem = Gun.GetChild(0).GetComponent<GunSystem>();
        
        damegeSystem = GetComponent<DamegeSystem>();
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

        CuserControl(mode_chage);
    }

    void CuserControl(bool mode_chage)
    {
        //커서를 움직이지 않도록 하고 보이지 않도록 해야함.
        if (mode_chage == false)
        { 
            Cursor.lockState = CursorLockMode.Locked; 
        }

        else
        { 
            Cursor.lockState = CursorLockMode.Confined; 
        }
        Cursor.visible = mode_chage;
    }

    public bool Getlife()
    {
        return hp.GetLife();
    }
}
