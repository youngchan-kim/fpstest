using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    NavMeshAgent agent;
    //Animator anim;

    Rigidbody target;
    EnemyData data;

    //적들의 업데이트시간을 0.04f로 쓴다.
    //업데이트 시간을 공용으로 사용하고 읽기 전용으로 사용한다.
    static readonly float updateTime = 0.04f;
    float attackTimer = 0.0f;
    //상태 데이터 (IdleState,ChaseState,AttackState,DamagedState, DieState)의 상태가 있다.

    //상태 데이터에 따라 행동이 달라짐
    StateData stateData = null;

    FSM stateMachine = null;

    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true; //길 찾기를 멈추기 위함
        //anim = GetComponent<Animator>();
    }
    void ErrorMessage(string msg)
    {
        Debug.LogError(msg);
        gameObject.SetActive(false);
    }

    public void Initialize(StateData data)
    {
        stateData = data;
    }

    //적의 데이터를 설정
    public bool SetData(EnemyData enemyData)
    {
        if (null == enemyData)
        {
            ErrorMessage("Enemy Data가 Null 입니다.");
            return false;
        }
        if (null == stateMachine) stateMachine = new FSM(this);
        if (!stateMachine.SetCurrState(stateData.IdleState))
        {
            ErrorMessage("Current State가 Null 입니다.");
            return false;
        }
        target = null;
        data = enemyData;
        gameObject.SetActive(true);
        //anim.Play("Movement");
        // GameObject가 Active(활성화) 상태여야만 Coroutine 사용이 가능.
        StartCoroutine(OnUpdate());
        return true;
    } // bool SetData()

    public void OnAttackState()
    {
        if (!stateMachine.ChangeState(stateData.AttackState))
        {
            ErrorMessage("Attack State가 Null 입니다.");
        }
    }
    public void OnDamagedState()
    {
        if (!stateMachine.ChangeState(stateData.DamagedState))
        {
            ErrorMessage("Damaged State가 Null 입니다.");
        }
    }
    public void OnIdleState()
    {
        if (!stateMachine.ChangeState(stateData.IdleState))
        {
            ErrorMessage("Idle State가 Null 입니다.");
        }
    }
    public void OnChaseState()
    {
        if (!stateMachine.ChangeState(stateData.ChaseState))
        {
            ErrorMessage("Chase State가 Null 입니다.");
        }
    }
    public void OnDieState()
    {
        if (!stateMachine.ChangeState(stateData.DieState))
        {
            ErrorMessage("Die State가 Null 입니다.");
        }
    }
    //멈추기 위한 함수
    public void MoveStop()
    {
        //anim.SetFloat("Magnitude", 0.0f);
        agent.isStopped = true; //길 찾기를 멈추기 위함
        agent.ResetPath();  //길을 찾았으니 길의 정보를 지운다.
    }

    //타겟을 놓쳤을때
    //타겟을 놓치면 타겟을 찾아야지
    void LostTarget()
    {
        target = null;
        MoveStop();
        //정찰 함수
    }

    //타겟 찾기
    public bool FindTarget()
    {
        //OverlapSphere는 중점과 반지름으로 가상의 원을 만들어 원 내부의 모든 콜라이더를 반환
        Collider[] targets = Physics.OverlapSphere(transform.position, data.SearchRange - 0.1f, data.TargetLayer);
        
        //타겟배열은 null이 아니고 타겟들의 배열의 길이가 0보다 커야함
        if(null != targets && 0 < targets.Length)
        {
            target = targets[0].GetComponent<Rigidbody>();
            return true;
        }
        LostTarget();
        return false;
    }

    //타겟과의 거리
    float DistanceToTarget()
    {
        //타겟이 없으면 -1을 리턴
        if (!target) return -1.0f;
        //타겟과의 거리를 리턴
        return Vector3.Distance(target.position, transform.position);
    }

    public bool TargetLostCheck()
    {
        float dist = DistanceToTarget();
        if ((0 > dist) || (dist > data.SearchRange))
        {
            LostTarget();
            return true;
        }
        return false;
    }

    //공격 범위
    public bool IsAttackBounds
    {
        get
        {
            float dist = DistanceToTarget();
            if(0>dist) return false;
            //타겟과의 거리보다 AI의 가 멈춘거리가 더 클거나 같을때
            return (dist <= agent.stoppingDistance);
        }
    }

    //타겟을 향해 이동
    public void MoveToTarget()
    {
        if(!target) return;

        agent.isStopped = false;
        //탐색할 대상의 지점
        //순찰 포인트 혹은 아이템 포인트, 적의 포인트
        agent.SetDestination(target.position);

        //AI가 움직이는 속도의 크기를 이용하여 움직이는 애니메이션 처리
        //anim.SetFloat("Magnitude", agent.velocity.magnitude);

    }

    //공격할때는 멈춘뒤 공격한다.
    public void ReadyAttack()
    {
        attackTimer = 0.0f;
        MoveStop();
        //엄폐물뒤로 간뒤 나와서 공격한다.
    }

    //공격 대기 시간.(공격하는 시간)
    public bool AttackDurationCheck()
    {
        if(0.0f < attackTimer)
        {
            attackTimer -= updateTime;
            return true;
        }
        return false;
    }

    //타겟을 바라보도록 만들기
    bool LookRotationToTarget()
    {
        if(!target) return false;

        Vector3 targetPos = target.position;
        Vector3 currPos = transform.position;
        currPos.y = targetPos.y;

        Vector3 dir = (targetPos - currPos).normalized;
        transform.rotation = Quaternion.LookRotation(dir);

        return true;
    }

    public void AttackToTarget()
    {
        //target을 바라본다.
        if (!LookRotationToTarget()) return;

        MoveStop();
        //anim.SetTrigger("Attack");
        attackTimer = data.AttackDuration;
        //플레이어 데미지 코드가 들어와야함
    }

    IEnumerator OnUpdate()
    {
        while(true)
        {
            stateMachine.Update();
            yield return new WaitForSeconds(updateTime);
        }
    }


}
