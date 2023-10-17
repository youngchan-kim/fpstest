using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;


public class Enemy1 : RandomPosition
{
    //player
    //public Transform target;
    //enemy = agent;

    //NavMeshAgent을 이용한 map의 크기를 알아올 수 있는지
    NavMeshAgent agent;

    //인식 범위
    //폭
    float Angle;
    //길이
    float nomal_site_len;

    //장소 도착 인정 범위
    float inv_len;
    //공격 범위는 인식 범위보다 짧아야한다.
    float Attectlen;

    //탐색 길이
    float reconnaissance_site_len;

    public GameObject Spot_mark;

    Vector3 SpotDir;

    //상태의 종류
    enum State
    {
        Idle,
        Run,
        Attack,
        Reconnaissance
    }
    //상태의 처리
    State state;
    // Start is called before the first frame update
    void Start()
    {
        
        state = State.Idle;
        agent = GetComponent<NavMeshAgent>();
        Angle = 30f;
        Attectlen = 30f;
        nomal_site_len = Attectlen*2;
        reconnaissance_site_len = nomal_site_len * 2;
        inv_len = reconnaissance_site_len * 0.01f;
        Rayangle = Angle * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        GetSeeUnit(Angle, nomal_site_len);
        Spot_mark.transform.position = Spot;
        if (target != null)
        {
            transform.LookAt(target.position);
        }
            //각 상태에서의 처리를 해준다.
        switch ((int)state)
        {
            case 0:
                //기본 상태일때 플레이어 찾기
                UpdateIdle();
                break;
            case 1:
                UpdateRun();
                break;
            case 2:
                UpdateAttack();
                break;
            case 3:
                UpdateReconnaissance();
                //Debug.Log("탐색중");
                break;

        }
    }

    private void UpdateIdle()
    {
        //Player를 찾는다.
        //target = GameObject.Find("Player").transform;
        //부채꼴 범위 안에 들어온 Layermask를 모두 받는다.

        //본인에게 총이 있는가

        if (target != null)
        {
            state = State.Run;
            //애니메이션도 바꿔줘야함
        }
        else
        {
            //기본 상태에서 탐색 모드로 바뀔 때 처음 한번 실행되어야한다.
            SpotDir = Spot - transform.position;

            //오브젝트가 바라보는 정면은 스팟 방향이다.
            transform.forward = SpotDir.normalized;

            //이때 오브젝트가 회전값 y를 빼내어사용한다.
            base_rotation_y = transform.rotation.eulerAngles.y;

            state = State.Reconnaissance;
        }

    }

    private void UpdateRun()
    {
        if (target == null)
        {
            state = State.Idle;
        }
        else
        {
            
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > nomal_site_len)
            {
                state = State.Idle;
            }
            if (distance <= Attectlen)
            {

                state = State.Attack;
                //공격 애니메이션을 실행해줘야함
            }
            //속도 설정 : 기본 속도
            agent.speed = 7f;

            agent.destination = target.transform.position;
        }

    }

    private void UpdateAttack()
    {
        //agent.isStopped = true;
        //공격할때도 거리가 중요
        if (target == null)
        {
            state = State.Idle;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > Attectlen)
            {
                state = State.Idle;
                //이동 애니메이션을 실행해줘야함
            }
            else if (distance < Attectlen)
            {
                agent.speed = 0f;
                
                //총이 없는 지
                //if(have_gun)

                //총이 있는 지



            }
        }

        //Damaged 코드 들어가야함


        //공격중 플레이어가 사라졌을때 플레이어를 찾아야함

    }
    private void UpdateReconnaissance()
    {
        if (target != null)
        {
            state = State.Run;
        }
        else
        {

           //  제자리에서 시야 회전 탐색이 완료되면 해당 코드 주석 없앨것
            float distance = Vector3.Distance(transform.position, Spot);


            //속도 설정 : 기본 속도
            agent.speed = 7f;
            agent.destination = Spot;

            //도착 장소에 도착하면 새로운 장소 지정
            if(distance < inv_len)
            {
                Spot = RandomPoint(reconnaissance_site_len);
                //Debug.Log("현재 회전값  : " + transform.rotation.eulerAngles + "입니다.");

                SpotDir = Spot - transform.position;

                //오브젝트가 바라보는 정면은 스팟 방향이다.
                transform.forward = SpotDir.normalized;
                //이때 오브젝트가 회전값 y를 빼내어사용한다.
                base_rotation_y = transform.rotation.eulerAngles.y;

                //Base_rotation_Y(Spot);
            }

            if (target != null) state = State.Idle;
            else
            {
                if (isDelay == true)
                {
                    //오브젝트의 시야
                    Not_Find_Target();
                }
                //목표지점을 바라보게 만듬
                // 선형 보간 해서 시야각 돌리는 코드
                Quaternion VarRotation = Quaternion.Euler(0f, var_rotation_y, 0f);

                //코르틴을 이용한 시간 값의 증가를 통해 t의 값을 0부터 1까지 증가시켜준다.
                //t값은 시간에 따라 1이 되어야한다.
                transform.rotation = Quaternion.Lerp(transform.rotation, VarRotation, 0.01f);
            }

        }
    }
 }
