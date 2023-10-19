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

    //target은 Player임
    public LayerMask target_Mask;

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

    //오브젝트의 회전을 위한 변수들 여기부터
    public bool isDelay = true;
    //더할지 말지 관리
    bool y_Dir = true;
    //탐색을 위한 y축 회전값
    public float y_rotation;
    //회전하기 전의 y축 값
    public float var_rotation_y;
    //회전하기 전의 y축 값
    public float base_rotation_y;
    Quaternion VarRotation;
    //오브젝트의 회전을 위한 변수 여기까지

    //Object가 없어진경우 사용되는 변수들
    public GameObject DeadBox;



    //NPC의 기본 정보
    public float maxHealth = 100f;
    public float currentHealth;
    bool life;
    float atteck;
    bool isAtkDelay;


    //상태의 종류
    enum State
    {
        Idle,
        Run,
        Attack,
        Reconnaissance,
        Die
}
    //상태의 처리
    State state;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }



    // Update is called once per frame
    void Update()
    {

        UpdateTarget(Angle, nomal_site_len, target_Mask);
        Spot_mark.transform.position = Spot;
        if(life == false)
        {
            state = State.Die;
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
            case 4:
                UpdateDie();
                break;
        }
    }
    public void Initialize()
    {
        state = State.Idle;
        agent = GetComponent<NavMeshAgent>();
        Angle = 30f;
        Attectlen = 30f;
        nomal_site_len = Attectlen * 2;
        reconnaissance_site_len = nomal_site_len * 2;
        inv_len = reconnaissance_site_len * 0.01f;

        life = true;
        currentHealth = maxHealth;
        atteck = 30;
        isAtkDelay = true;
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
            agent.speed = 0f;
            agent.destination = Spot;

            //도착 장소에 도착하면 새로운 장소 지정
            if (distance < inv_len)
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
                    Rotate_Play(2f);

                    Debug.Log(base_rotation_y);
                    //목표지점을 바라보게 만듬
                    // 선형 보간 해서 시야각 돌리는 코드
                    VarRotation = Quaternion.Euler(0f, var_rotation_y, 0f);
                    Debug.Log(var_rotation_y);
                    //코르틴을 이용한 시간 값의 증가를 통해 t의 값을 0부터 1까지 증가시켜준다.
                    //t값은 시간에 따라 1이 되어야한다.
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, VarRotation, 0.01f);
            }
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

                //공격
                if (isAtkDelay == true)
                {
                    Atk_Play(1f);
                    target.GetComponent<Player>().Damage(atteck);
                }

            }
        }
    }
    private void UpdateDie()
    {
        DeadBox.SetActive(true);
        DeadBox.transform.position = gameObject.transform.position;
        gameObject.SetActive(false);

    }

    public void Damage(float damage_value)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage_value;
        }
        else
        {
            currentHealth = 0f;
            life = false;
        }
    }

    //오브젝트를 회전 시켜 타겟을 찾기
    public void Rotate_Play(float time)
    {

        if (isDelay == true) StartCoroutine(TimeCoroutin(time));
    }
    public void Atk_Play(float time)
    {

        if (isDelay == true) StartCoroutine(AtkTimeCoroutin(time));
    }
    IEnumerator TimeCoroutin(float time)
    {
        isDelay = false;

        //5초에 한번 true가 된다.
        //5초 뒤에 아래의 실행문이 한번실행된다.
        yield return new WaitForSeconds(time);
        //Debug.Log(second +"초");
        switch (y_Dir)
        {
            case true:
                y_rotation = -30;
                //Debug.Log("y값 현재 : " + y_rotation + "만큼 +되었습니다.");
                y_Dir = false;

                break;
            case false:

                y_rotation = 30;
                //Debug.Log("y값 현재 : " + y_rotation + "만큼 +되었습니다.");
                y_Dir = true;

                break;
        }

        var_rotation_y = base_rotation_y + y_rotation;
        isDelay = true;
    }
    /*
    //Ray를 움직였던 것처럼 만들려고 했음
        float view_lerp = 0;
        float view_lerp_t = 0;
        bool right = false;
       public void rotate_Objectview(float viewangle)
        {
            if (1 >= view_lerp_t && view_lerp_t >= 0)
            {
                if (right == false)
                {
                    //시야가 현재 보는 각이 view_lerp도이고 view_lerp로 부터 viewangle도 사이의 0.01f를 리턴
                    view_lerp = Mathf.Lerp(view_lerp, viewangle, 0.01f);
                    //현재 시야로 보는 각도+0.1f도는 최대 볼 수 있는 각도보다 크거나 같으면 
                    if (view_lerp + 0.1f >= viewangle)
                        right = true;
                }
                else
                {
                    view_lerp = Mathf.Lerp(view_lerp, -viewangle, 0.01f);
                    if (view_lerp - 0.1f <= -viewangle)
                        right = false;
                }
            }
            else
            {
                view_lerp = 0;
                Debug.Log("t 값이 비정상 적인 값이 들어와 있습니다.");
            }
            transform.rotation = Quaternion.Euler(Quaternion.AngleAxis(view_lerp, transform.up) * transform.forward);
        }*/
    IEnumerator AtkTimeCoroutin(float time)
    {
        isAtkDelay = false;

        //5초에 한번 true가 된다.
        //5초 뒤에 아래의 실행문이 한번실행된다.
        yield return new WaitForSeconds(time);

        isAtkDelay = true;
    }
}