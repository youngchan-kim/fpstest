using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// 적이 해야될 것
// 각 상태들로 나누고 해당 상태가 되기 위한 조건이 필요
// 상태에서 해야하는 행동이 필요
public class Enemy1 : MonoBehaviour
{
    //player
    public Transform target;

    //enemy = agent;
    NavMeshAgent agent;

    //상태의 종류
    enum State
    {
        Idle,
        Run,
        Attack
    }
    //상태의 처리
    State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        agent = GetComponent<NavMeshAgent>();       
    }

    // Update is called once per frame
    void Update()
    {
        //각 상태에서의 처리를 해준다.
        //
        if(state == State.Idle)
        {
            //기본 상태일때 플레이어 찾기
            UpdateIdle();
        }
        else if(state == State.Run)
        {
            UpdateRun();
        }
        else if (state == State.Attack) 
        {
            UpdateAttack();
        }
    }

    private void UpdateIdle()
    {
        agent.speed = 0;
        //Player를 찾는다.
        target = GameObject.Find("Player").transform;
       

        if(target !=null)
        {
            state = State.Run;
            //애니메이션도 바꿔줘야함
        }

    }

    private void UpdateRun()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance <= 10)
        {
            state = State.Attack;
            //공격 애니메이션을 실행해줘야함
        }

        //속도 설정 : 기본 속도
        agent.speed = 7f;
        
        agent.destination = target.transform.position;

    }

    private void UpdateAttack()
    {
        agent.isStopped = true;
        //공격할때도 거리가 중요
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > 10)
        {
            agent.isStopped = false;
            state = State.Run;
            //이동 애니메이션을 실행해줘야함
        }
        //Damaged 코드 들어가야함


        //공격중 플레이어가 사라졌을때 플레이어를 찾아야함

    }


}
