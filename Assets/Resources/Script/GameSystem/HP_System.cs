using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_System : MonoBehaviour
{
    public float MaxHp;
    public float CurrentHp;
    float BasicHealvolume;
    bool HP_life;

    Coroutine coroutine = null;
    public void Initialize(float MaxHealth, float MaxHealvolume, bool life)
    {
        MaxHp = MaxHealth;
        CurrentHp = MaxHealth;
        BasicHealvolume = MaxHealvolume;
        HP_life = life;
    }

    public float GetCurHp()
    {
        return CurrentHp;
    }
    public void SetCurHp(float hp)
    {
        CurrentHp = hp;
    }
    public float GetBasicHealvolume()
    {
        return BasicHealvolume;
    }

    public void Lifecheck()
    {
        if (CurrentHp <= 0) HP_life = false;
    }
    public bool GetLife()
    {
        return HP_life;
    }

    public void Dead()
    {

    }
    public void OnHpAnimtion(InventorySlot Slot)
    {
        if (null != coroutine) { StopCoroutine(coroutine); Slot.totalamount++; }
        coroutine = StartCoroutine(HpSliderAnimation(Slot.item.addHeal));
        Slot.totalamount--;
    }

    //힐 이터멀레이터
    IEnumerator HpSliderAnimation(float _heal)
    {
        //최대 힐량
        float heal = _heal;

        float t = 0.0f;
        //최대로 채울 수 있는 값 분에 3.0을 채우는 시간
        float elipsed = 1.0f / GetBasicHealvolume();
        //현재 채력이 최소 회복가능한 값보다 작을 때
        while (GetCurHp() < GetBasicHealvolume())
        {
            //heal이 남은 량이 0보다 크다면
            if (heal > 0)
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
        Debug.Log("힐이 들어간 값 : " + t);
    }
}

