using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Action : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Initalize()
    {
        if (anim)
        {
            anim.SetFloat("Move", 0.0f);
            anim.Play("Movement");
        }
    }
    public void Move(float value)
    {
        if (anim) anim.SetFloat("Move", value);
    }

    public void Die()
    {
        if (anim) anim.SetTrigger("IsDie");
    }
}
