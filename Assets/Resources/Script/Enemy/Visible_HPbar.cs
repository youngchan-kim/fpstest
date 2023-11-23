using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Visible_HPbar : MonoBehaviour
{
    public GameObject HpBar;
    Vector3 TargetDir = new Vector3();
    public float ViewAngle;
    float angle;
    GameObject player;
    private void Start()
    {
        player = GameMgr.Instance.player;
    }
    void Update()
    {
        ViewAngle = Camera.main.fieldOfView;
        //플레이어로 향하는 target의 벡터
        TargetDir = StaticViewAngle.static_Obj_to_Target_Dir(player.transform.position, transform.position);

        //기준이 되는 
        angle = StaticViewAngle.static_Obj_ViewAngle_in_Target_Angle(TargetDir.normalized, player.transform.forward);

        if (angle > ViewAngle)
        {
            if (HpBar.activeSelf == false) HpBar.SetActive(true);
            HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
        }
        else
        {
            if (HpBar.activeSelf == true) HpBar.SetActive(false);
        }
    }
}
