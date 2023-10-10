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
    float len;

    //공격 범위
    float Attectlen;

    Coroutine coroutine = null;

    //정찰하는 포인트
    Vector3 Reconnaissance = new Vector3();

    //상태의 종류
    enum State
    {
        Idle,
        Run,
        Attack,
        Reconnaissance,
        Research
    }
    //상태의 처리
    State state;
    // Start is called before the first frame update
    void Start()
    {
        
        state = State.Idle;
        agent = GetComponent<NavMeshAgent>();
        Angle = 30f;
        len = 15f;
        Attectlen = 15f;
        //search_spot = null;
    }

    // Update is called once per frame
    void Update()
    {
        GetSeeUnit(Angle, len);
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
                break;
            case 4:
                UpdateResearch();
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
            if (null != coroutine) StopCoroutine(coroutine);
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > len)
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
                state = State.Run;
                //이동 애니메이션을 실행해줘야함
            }
            else if (distance > len)
            {
                state = State.Idle;
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

            float distance = Vector3.Distance(transform.position, Spot);


            //속도 설정 : 기본 속도
            agent.speed = 4f;

            agent.destination = Spot;
            /*if (coroutine == null)
                TargetFind(coroutine);*/

            if(distance < len)
            {
                Spot = RandomPoint(len);
            }

        }
    }
    private void UpdateResearch()
    {
        target = point;
        state = State.Idle;
    }

}
