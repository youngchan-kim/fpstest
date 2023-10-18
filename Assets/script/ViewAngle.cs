using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class ViewAngle : MonoBehaviour
{
    Vector3 TargetDir = new Vector3();
    float angle;
    public Transform target;
    Transform checktarget;

    //회전하기 전의 y축 값
    public float base_rotation_y;
    //회전하기 전의 y축 값
    public float var_rotation_y;

    float angleRange;
    float radius;
    float This_Angle;
    // 탐색 장소
    public Vector3 Spot;
    //탐색을 위한 y축 회전값
    public float y_rotation;
    //더할지 말지 관리
    bool y_Dir =true;
    public bool isDelay = true;
    public bool isTimeDelay = true;
    float second =0;
    //Laymask 레이에 인식되어야할
    //target은 Player임
    public LayerMask target_Mask;
    //Target과 플레이어 사이에 장애물이 있는 경우와 없는 경우 체크
    //장애물이 있으면 false 없으면 true
    //enemy1에서 Lerp에 사용하는 t값
    public float f_lerp_t;
    float Var_y;
    //선형보간 t값 예시
    float t = 0;
    //선형 보간인 경우 true이면 증가 아닌 경우 
    bool b_t;
    //Ray의 각도 값
    float Ray_angle_lerp = 0;
    public float Rayangle;

    RaycastHit rayHit;

    //부채꼴모양 색칠
    bool isCollision = false;
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    public void GetTarget(float viewAngle , float len)
    {
        angleRange = viewAngle;
        
        radius = len;
        //첫번째 벡터에서 두번째 벡터로의 회전 각도입니다.
        //첫번째 벡터로 두번째 벡터가 회전하기 위한 각도를 리턴한다.
        //position값이 아니라 방향값이 들어와야한다.
        //탐색자로 부터의 포인트가 있는 각도를 구해준다. 
        //함수화 할것
        
        //오버랩 스피어의 동작은 스피어콜라이더와 같다. 피봇은 그대로 이고 크기를 키우는 방향은 전방향으로 같은 값이 늘어난다.
        //
        //스피어 케스트의 동작은  캡슐 콜라이더와 비슷하다. 피봇은 그대로 지만 크기를 키우는 방향이 정해져있고
        //
        /*Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, target_Mask);
        //hitColliders의 값이 없지 않으면 리턴한다.
        if (hitColliders.Length != 0)
        {
            Debug.Log("원의 범위 안에 뭔가 있다.");
        }*/

        
        target = Find_Target(angleRange);
        isCollision = false;
    }

    //현재 하나의 목표만 찾는 경우로 만들었음
    //SphereCastAll로 바꾸어 모든 모든 개체에 대한 것으로 바꾸어 주어야함
    Transform Find_Target(float viewAngle)
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
            TargetDir = checktarget.position - transform.position;

            //탐색자를 향하는 타겟의 벡터
            //A가 타겟이고 B가 탐색자일때 B가 A로 가기 위한 방향은
            //A-B이다.
            //타겟에게 향하는 방향
            if (checktarget != null) angle = Vector3.Angle(TargetDir.normalized, transform.forward);

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

    //
 /*   Vector3 RayDir(float Rayangle)
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

    //오브젝트를 회전 시켜 타겟을 찾기
    public void Not_Find_Target()
    {
        if (isDelay == true) StartCoroutine(testCoroutin(5f));
    }
    
    IEnumerator testCoroutin(float time)
    {
        isDelay = false;
           
        //5초에 한번 true가 된다.
        //5초 뒤에 아래의 실행문이 한번실행된다.
        yield return new WaitForSeconds(time);
        second += time;
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
        Var_y = y_rotation;
        var_rotation_y = base_rotation_y + y_rotation;
        isDelay = true;
    }


    //언제 호출 되는가? 타겟을 놓쳤고 찾기 시작할때마다 호출
    public void Base_rotation_Y(Vector3 _Spot)
    {
        //가야하는 방향을 알면 TargetDir.normalized 한 값은 
        Vector3 SpotDir = _Spot - transform.forward;
        Debug.Log("타겟 방향의 벡터는 " + SpotDir.normalized);
        transform.forward = SpotDir;

    }

    /*//히트된 객체와 LayeMask를 비교하여 같으면 true를 반환 시켜주는 함수
    isRayCastHit(RaycastHit raycastHit, LayerMask target_Mask)
   {
       if (0 != ((1 << raycastHit.transform.gameObject.layer) & target_Mask))
       {
           return raycastHit.transform;
       }
       return false;
   }*/

    //시야각을 나눠서 레이를 쏴서 레이를 플레이어를 찾는다.
    /*public bool Find_Target_In_Lay()
    {

        //Find_target가 true 이면 히트된 transform을 리턴
        //방향으로 레이를 쏜다. 쏘는 방향은 발사하려는 영역을 알면 발사하는 위치를 알기 쉬운데
        //표적의 vector - 자신의 vector를 하면 방향이 나온다.

        Debug.DrawRay(transform.position, RayDir(Rayangle) * radius, Color.black);
        if(Physics.Raycast(transform.position, RayDir(Rayangle), out RaycastHit raycastHit, radius))
        {
            //예제 코드 좀비의 아이템 부분
            //target의 타입은 GameObject
            //target_Mask는 bit shift(2^index된 값을 가진다.)
            return isRayCastHit(raycastHit, target_Mask);
        }
        return false;
    }*/

    //시야각을 Scenes에서 보여주는 코드
    private void OnDrawGizmos()
    {
        /*
        RaycastHit hit;
        // Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 0)) { }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance, radius);*/

        Handles.color = isCollision ? _red : _blue;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange, radius);

    }


}


