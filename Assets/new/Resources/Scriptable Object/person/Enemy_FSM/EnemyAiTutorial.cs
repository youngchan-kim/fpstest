using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    float health = 100;
    //해당 컴포넌트는 목표를 향해 움직일 때 서로 피해가는 캐릭터 생성에 유용합니다.
    //https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //순찰
    public Vector3 walkPoint;
    //walkPoint가 있는지에 대한 ?
    bool walkPointSet;
    //추격을 위한 보행지점 범위
    public float walkPointRange;


    //공격
    //공격딜레이타임
    public float timeBetweenAttacks;
    //이미 공격했는지 확인?
    bool alreadyAttacked;

    //발사체를 인스턴스화(총알에대한 인스턴스)
    //총기에서 총알에 대한 인스턴스를 다룰것
    public GameObject projectile;


    //범위
    //순찰범위와 공격범위
    public float sightRange, attackRange;
    //플레이어가 시야범위, 어택범위 내에 있는지 확인을 위한 값
    public bool playerInSightRange, playerInAttackRange;

    //씬이 시작될 때 한번 start전에 호출
    //게임 오브젝트가 활성화 되어있을 때 한번 호출
    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        //수정 : 적을 기준으로한 원이 아닌 시야각을 설정하여 확인
        //Physics.CheckSphere는 지정된 위치로부터 지정한 사이즈의 원안에 선택한 LayerMask가 있는지 확인후 체크한다. 
        //https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html
        //시야 범위에 있는지 확인
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //공격 범위에 있는지 확인
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }
    private void Patroling()
    {
        //걷기 지점이 성정되지 않은 경우 순찰 기능을 실행
        //걷기 지점을 검색
        if (!walkPointSet) SearchWalkPoint();

        //목표지점이 정해지면 움직여야함
        //SetDestination는 https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html에 설명이 나와있음
        //목적지 설정
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        //목적지에 도달했는지 체크를 위한 남은 거리
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //벡터의 길이 
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    //
    private void SearchWalkPoint()
    {
        //시야 범위안에 랜덤한 포인트를계산한다.
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //정찰 위치는 오브젝트 기준으로 randomx ,y는 그대로,randomz이다.
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //해당 지점이 땅인경우 walkPointSet을 true로 해준다.
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    //아이템을 쫒아가는 함수도 비슷한 코드가 나옴
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        //적이 움직이지 않고 공격을 시작
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        //수정: 가진 총기에 따른 공격
        //공격했는지 확인하여 공격했다면 딜레이를 준다.
        if (!alreadyAttacked)
        {
            ///공격에 대한 모든 공격코드
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //AddForce(힘의 방향*힘의 값, 힘의 종류);
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);



            ///


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    //재설정 공격
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);

        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    //공격과 시야 범위를 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
