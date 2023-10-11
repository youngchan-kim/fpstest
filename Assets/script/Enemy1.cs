using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
        inv_len = reconnaissance_site_len / 4f;
        //search_spot = null;
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

       
        agent.speed = 0;

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
        agent.speed = 0;
        if (target == null)
        {
            state = State.Idle;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > Attectlen)
            {
                agent.isStopped = false;
                state = State.Idle;
                //이동 애니메이션을 실행해줘야함
            }
            else if (distance < Attectlen)
            {
                agent.speed = 0f;
                agent.isStopped = true;
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
            agent.speed = 3.5f;
            agent.destination = Spot;

            //도착 장소에 도착하면 새로운 장소 지정
            if(distance < inv_len)
            {
                Spot = RandomPoint(reconnaissance_site_len);
                Base_rotation_Y(Spot);
            }
            
            if(target != null)
            {

                state = State.Idle;
            }

            if (isDelay == true)
            {
                Not_Find_Target();
            }

            // 어떤 값을 넣어야하는지 Slerp를 어떻게 쓰는지 확인하고 다시 만들것
            transform.rotation = Quaternion.Slerp(transform.rotation,
    Quaternion.Euler(transform.rotation.x, base_rotation_y + y_rotation,
    transform.rotation.z), Time.deltaTime * 5f);
            Debug.Log("현재 y값은  : " + transform.rotation.y + "입니다.");


            /*            //도착장소가 없으면 할당
                        if(Spot==null)
                        {
                            Spot = RandomPoint(len);
                        }*/

        }
    }
 }
