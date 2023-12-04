using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;


public class Enemy : TargetCheck
{

    //target은 Player임
    [Header("target")]
    public LayerMask target_Mask;
    [Header("ground")]
    public LayerMask ground;
    //NavMeshAgent을 이용한 map의 크기를 알아올 수 있는지
    NavMeshAgent agent;
    Transform Gun;
    GunSystem gunSystem;

    //인식 범위 : 길이의 다향성을 줄일 것
    //폭
    float Angle;
    //길이
    float nomal_site_len;


    //공격 범위는 인식 범위보다 짧아야한다.
    [SerializeField]
    public float Attectlen;
    //장소 도착 인정 범위
    public float in_spot_len;

    //탐색 길이
    float reconnaissance_site_len;

    public GameObject Spot_mark;
    Vector3 SpotDir;

    //public Findattackpoint findattackpoint;

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

    //적이면 머리를 조준 아니면 정면 조준
    Findattackpoint ATKpoint;

    //NPC의 기본 정보
    float maxHealth = 100f;
    float maxHealvolume = 80;
    bool life;

    [Serialize]
    public HP_System hp;
    DamegeSystem damegeSystem;

    bool isAtkDelay;
    //시체사라지는 딜레이 체크
    bool isDBDelay;

    float walk_speed;
    float Run_speed;


    //애니메이션
    public Enemy_Action anim;



    //상태의 종류
    enum State
    {
        Idle,
        Run,
        Attack,
        Reconnaissance,
        Escape,
        Die
    }
    //상태의 처리
    State state;
    // Start is called before the first frame update
    void Start()
    {
        ATKpoint = GetComponent<Findattackpoint>();
        damegeSystem = GetComponent<DamegeSystem>();
        Initialize();
    }

    public void Initialize()
    {
        state = State.Idle;

        Angle = 30f;
        nomal_site_len = Attectlen * 2;
        reconnaissance_site_len = nomal_site_len * 2;

        walk_speed = 1.5f;
        Run_speed = 3f;
        life = true;

        isAtkDelay = true;
        isDBDelay = true;

        //Debug.Log(transform.GetChild(0).GetChild(3).GetChild(2).GetChild(0).GetChild(0).name);
        Gun = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetChild(0);
        //GetComponent 
        //NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        //GunSystem
        gunSystem = Gun.GetComponent<GunSystem>();
        //Enemy_Action
        anim = transform.GetComponent<Enemy_Action>();
        //HP_System
        hp = transform.GetComponent<HP_System>();

        anim.Initalize();
        hp.Initialize(maxHealth, maxHealvolume, life);


    }
    public float maxlen()
    {
        return reconnaissance_site_len;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hp.GetLife());
        if (!hp.GetLife())
        {
            state = State.Die;
        }
        UpdateTarget(Angle, nomal_site_len, target_Mask);
        Spot_mark.transform.position = Spot;
        if (target == null)
            ATKpoint.LookTargetCheck(null);
        if (life == true)
        {
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
                    UpdateEscape();
                    break;
                case 5:
                    UpdateDie();
                    break;
            }
        }

    }

    private void UpdateEscape()
    {
        if (target != null)
        {

        }
    }

    private void UpdateReconnaissance()
    {
        anim.Move(1);
        if (target != null)
        {

            state = State.Run;
        }
        else
        {
            //  제자리에서 시야 회전 탐색이 완료되면 해당 코드 주석 없앨것
            float distance = Vector3.Distance(transform.position, Spot);


            //속도 설정 : 기본 속도
            agent.speed = walk_speed;
            agent.destination = Spot;

            //도착 장소에 도착하면 새로운 장소 지정
            if (distance < in_spot_len)
            {
                Spot = StaticRandomPosition.RandomPoint(transform.position, reconnaissance_site_len);
                //Debug.Log("현재 회전값  : " + transform.rotation.eulerAngles + "입니다.");

                SpotDir = Spot - transform.position;

                //오브젝트가 바라보는 정면은 스팟 방향이다.
                transform.forward = SpotDir.normalized;
                //이때 오브젝트가 회전값 y를 빼내어사용한다.
                base_rotation_y = transform.rotation.eulerAngles.y;
            }

            if (target != null) state = State.Idle;
            else
            {
                if (isDelay == true)
                {
                    //오브젝트의 시야
                    Rotate_Play(2f);

                    //Debug.Log(base_rotation_y);
                    //목표지점을 바라보게 만듬
                    // 선형 보간 해서 시야각 돌리는 코드
                    VarRotation = Quaternion.Euler(0f, var_rotation_y, 0f);
                    //Debug.Log(var_rotation_y);
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
        anim.Move(1);
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
            agent.speed = Run_speed;

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
            anim.Move(0);
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > Attectlen)
            {
                anim.Shoot(false);
                state = State.Idle;
                //이동 애니메이션을 실행해줘야함
            }
            else if (distance <= Attectlen)
            {
                agent.speed = 0f;
                ATKpoint.LookTargetCheck(target);
                //공격
                if (isAtkDelay == true)
                {
                    //Debug.Log(gunSystem.FiringIsPossible()); 
                    Atk_Play(1f);
                    if (gunSystem.BulletIsEmpty() && gunSystem.ReadyToShoot() && gunSystem.FiringIsPossible())
                    {
                        gunSystem.Firing();
                    }
                    else if (!gunSystem.ReadyToShoot())
                        gunSystem.Reload();
                }
            }
        }
    }

    private void UpdateDie()
    {
        if (isDBDelay)
        {
            anim.Die();
            DB_Play(3f);
        }
    }


    //오브젝트를 회전 시켜 타겟을 찾기
    public void Rotate_Play(float time)
    {
        if (isDelay == true) StartCoroutine(TimeCoroutin(time));
    }
    public void Atk_Play(float time)
    {
        gunSystem.ClickToShoot(false);
        if (isAtkDelay == true) StartCoroutine(AtkTimeCoroutin(time));
        anim.Shoot(true);
        gunSystem.ClickToShoot(true);
    }
    public void DB_Play(float time)
    {
        if (isDBDelay == true) StartCoroutine(Dead_Body_TimeCoroutin(time));
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
    IEnumerator AtkTimeCoroutin(float time)
    {
        isAtkDelay = false;

        yield return new WaitForSeconds(time);
        isAtkDelay = true;
    }
    IEnumerator Dead_Body_TimeCoroutin(float time)
    {
        isDBDelay = false;

        //5초에 한번 true가 된다.
        //5초 뒤에 아래의 실행문이 한번실행된다.
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        isDBDelay = true;
    }
}