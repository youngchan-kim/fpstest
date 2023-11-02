using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public MouseItem mouseItem = new MouseItem();
    public float maxHealth = 100f;
    public float currentHealth;
    //float healvolume = 30;
    float MaxHealvolume = 80;



    public GameObject playercam;

    //public GameObject InventoryUI;
    private bool mode_chage;

    private bool life;

    RaycastHit rayHit;
    // Start is called before the first frame update
    //Player body Object
    //[HideInInspector] 
    //public GameObject floor;

    void Start()
    {
    }

    public void Initialize()
    {
        life = true;
        currentHealth = maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playercam.gameObject.SetActive(mode_chage);
            //Inventory.SetActive(!mode_chage);
            mode_chage = !mode_chage;  
        }

        if (Getlife())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(15);
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                life = false;
            }
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

    //플레이어가 피해를 입으면 체력이 줄어듬
    void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    IEnumerator HpSliderAnimation(float _heal)
    {
        //최대 힐량
        float heal = _heal;

        float t = 0.0f;
        //최대로 채울 수 있는 값 분에 3.0을 채우는 시간
        float elipsed = 1.0f / MaxHealvolume;   
        //현재 채력이 최소 회복가능한 값보다 작을 때
        while (currentHealth < MaxHealvolume)
        {
            //heal이 남은 량이 0보다 크다면
            if(heal > 0)
            {
                //
                if (heal > elipsed)
                {
                    heal -= elipsed;
                    t += elipsed;
                }
                else
                { 
                    t += heal;
                    heal = 0;
                }

            }
            yield return new WaitForSeconds(elipsed);
        }
        Debug.Log("힐이 들어간 값 : "+ t);

    }

    public bool Getlife()
    {
        return life;
    }

    //데이터 관리 클래스

    public void Damage(float damage_value)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damage_value;
        }
    }
}
