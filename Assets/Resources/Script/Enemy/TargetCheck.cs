using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TargetCheck : MonoBehaviour
{

    //Transfrom
    //target에 갱신하기전 체크 단계의 target
    Transform checktarget;
    //실제로 갱신되는 target
    //[SerializeField]
    public Transform target;

    //Vector3
    //target의 방향을 체크하기 위해 타겟의 방향을 담는 변수
    Vector3 TargetDir = new Vector3();
    // 탐색 장소
    public Vector3 Spot;

    //오브젝트가 체크할 수 있는 각도
    float angle;
    //반지름
    float radius;

    float DrawGizmosAngle;

    //오브젝트의 범위를 보기위한 변수들
    //부채꼴모양 색칠
    Color _blue = new Color(0f, 0f, 1f, 0.2f);

    //실제로 target을 갱신하기 위해 반복 실행되는 함수
    public void UpdateTarget(float viewAngle, float len, LayerMask target_Mask)
    {
        //첫번째 벡터에서 두번째 벡터로의 회전 각도입니다.
        //첫번째 벡터로 두번째 벡터가 회전하기 위한 각도를 리턴한다.
        //position값이 아니라 방향값이 들어와야한다.
        //탐색자로 부터의 포인트가 있는 각도를 구해준다. 
        //함수화 할것

        //오버랩 스피어의 동작은 스피어콜라이더와 같다. 피봇은 그대로 이고 크기를 키우는 방향은 전방향으로 같은 값이 늘어난다.
        //
        //스피어 케스트의 동작은  캡슐 콜라이더와 비슷하다. 피봇은 그대로 지만 크기를 키우는 방향이 정해져있고
        //
        DrawGizmosAngle = viewAngle;
        radius = len;
        target = Find_Target(viewAngle, target_Mask);
    }

    // Start is called before the first frame update
    //현재 하나의 목표만 찾는 경우로 만들었음
    //SphereCastAll로 바꾸어 모든 모든 개체에 대한 것으로 바꾸어 주어야함
    Transform Find_Target(float viewAngle, LayerMask target_Mask)
    {
        //
        //원이라는 범위 안에 찾고 있는 LayerMask가 있다면 if 문을 실행한다.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, target_Mask);
        if (hitColliders.Length != 0)
        {
            //우선 순위를 따지는 코드 필요

            foreach (Collider hit in hitColliders)
            {
                checktarget = hit.transform;
            }
            TargetDir = StaticViewAngle.static_Obj_to_Target_Dir(checktarget.position, transform.position);


            //탐색자를 향하는 타겟의 벡터
            //A가 타겟이고 B가 탐색자일때 B가 A로 가기 위한 방향은
            //A-B이다.
            //타겟에게 향하는 방향
            if (checktarget != null) angle = StaticViewAngle.static_Obj_ViewAngle_in_Target_Angle(TargetDir.normalized, transform.forward);

            else angle = viewAngle + 1;

            //원안에는 있지만 
            //오브젝트의 시야각 안에 있다면 if문 실행
            if (angle <= viewAngle)
            {

                /*
                예제 코드 좀비의 아이템 부분
                target의 타입은 GameObject
                target_Mask는 bit shift(2^index된 값을 가진다.)
                */
                //찾은 오브젝트가 찾으려는 LayerMask와 같은 지 확인하고 맞으면 target에 찾으려는 오브젝트를 넣어준다.
                if (0 != ((1 << checktarget.transform.gameObject.layer) & target_Mask))
                {
                    //Debug.Log("원의 범위 안, 시야각 내부에 타겟이 있다.");
                    //Target이 맞으면 오브젝트와 target사이에 장애물이 있는지 체크
                    if (Physics.Raycast(transform.position, (checktarget.position - transform.position).normalized, out RaycastHit rayHit, radius))
                    {
                        if (0 != ((1 << rayHit.transform.gameObject.layer) & target_Mask))
                        {
                            //Debug.Log("타겟과 오브젝트사이에 장애물이 없다.");
                            target = checktarget;
                            Target_Watching();
                        }
                        else TargetLost();
                    }
                }
            }
            //시야각 내부에서 벗어난 경우
            else TargetLost();
        }
        else TargetLost();
        return target;
    }
    //타겟을 바라보게함
    void Target_Watching()
    {
        transform.LookAt(target);
    }
    //타겟을 놓치면 
    // 타겟을 놓친 자리를 입력해줌
    void TargetLost()
    {
        if (target != null)
        {
            // Debug.Log("오브젝트 시야에서 벗어났다...");
            Spot = target.position;
        }
        else
        {
            // Debug.Log("타겟의 흔적도 발견하지 못했다.");
        }
        target = null;
        checktarget = null;
    }

    //시야각을 Scenes에서 보여주는 코드
    void OnDrawGizmos()
    {
        /*
        RaycastHit hit;
        // Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 0)) { }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance, radius);*/

        Handles.color = _blue;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, DrawGizmosAngle, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -DrawGizmosAngle, radius);

    }


}

/*
//Ray를 사용해야할 경우 Ray의 회전    
Vector3 RayDir(float Rayangle)
   {

       if (1 >= t && t >= 0)
       {
           if (b_t == false)
           {
               Ray_angle_lerp = Mathf.Lerp(Ray_angle_lerp, Rayangle, 0.01f);
               if (Ray_angle_lerp + 0.1f >= Rayangle)
                   b_t = true;
           }
           else
           {
               Ray_angle_lerp = Mathf.Lerp(Ray_angle_lerp, -Rayangle, 0.01f);
               if (Ray_angle_lerp - 0.1f <= -Rayangle)
                   b_t = false;
           }
       }
       else
       {
           Ray_angle_lerp = 0;
           Debug.Log("t 값이 비정상 적인 값이 들어와 있습니다.");
       }


       Vector3 RayDir = Quaternion.AngleAxis(Ray_angle_lerp, transform.up) * transform.forward;
       return RayDir;
   }*/