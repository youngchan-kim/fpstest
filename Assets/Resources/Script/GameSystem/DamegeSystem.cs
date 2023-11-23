using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeSystem : MonoBehaviour
{

    HP_System hp;
    private void Start()
    {
        hp = GetComponent<HP_System>();
    }
    //오브젝트가 피해를 입으면 체력이 줄어듬
    //체력이 줄어들었을때 생사여부를 체크함
    public void TakeDamage(float damage_value)
    {
        if (hp.GetCurHp() > 0)
        {
            hp.SetCurHp(hp.GetCurHp() - damage_value);
        }
        hp.Lifecheck();
    }
}
