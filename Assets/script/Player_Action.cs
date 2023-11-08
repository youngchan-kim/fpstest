using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;

    public void AnimStart()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    public void MoveAnim(float vertical, float horizon)
    {

        if(vertical!= 0 || horizon!=0)
        {
            Debug.Log("방향이 입력됨");
        }
        switch (vertical)
        {
            case 1:
                anim.SetFloat("B&F", 1);
                break;
            case 0:
                anim.SetFloat("B&F", 0);
                break;
            case -1:
                anim.SetFloat("B&F", -1);
                break;
        }
        switch (horizon)
        {
            case 1:
                anim.SetFloat("Dir", 1);
                break;
            case 0:
                anim.SetFloat("Dir", 0);
                break;
            case -1:
                anim.SetFloat("Dir", -1);
                break;
        }
    }
    public void Crouch()
    {
        switch (anim.GetBool("Crouch"))
        {
            case true:
                anim.SetBool("Crouch", false);
                break;

            case false:
                anim.SetBool("Crouch", true);
                break;
        }
    }
    public void IsJump(bool  isjump)
    {               
        anim.SetBool("Jump", isjump);
    }

    public void AimStart()
    {
        anim.SetBool("Aim", true);
    }
    public void AimEnd()
    {
        anim.SetBool("Aim", false);
    }
    public bool Aiming()
    {
        switch (anim.GetBool("Aim"))
        {
            case true:
                
                anim.SetBool("Aim", false);
                return false;
            case false:
                anim.SetBool("Aim", true);
                return true;
        }
    }
    public void Attacking()
    {
        anim.SetTrigger("Atteck");
    }
    public void Reloading(bool isReload)
    {
        anim.SetBool("Reload", isReload);
    }

    public void PickUpMotion(bool ispickup)
    {
        anim.SetBool("Pickup", ispickup);
    }
    public void Drink_A_Heal(bool isdrink)
    {
        anim.SetBool("Drink", isdrink);
    }
}
