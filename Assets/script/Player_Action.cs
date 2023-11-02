using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour
{



    float cur_hori;
    float cur_vert;
    // Start is called before the first frame update

    Animator anim;
    float moveAction =0;

    public void AnimStart()
    {
        Debug.Log(transform.GetChild(0).name);
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
    public void IsJump()
    {               
        anim.SetTrigger("Jump");
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
    public void Reloading()
    {
        anim.SetTrigger("Reload");
    }

    public void PickUpMotion()
    {
        anim.SetTrigger("Pickup");
    }
}
