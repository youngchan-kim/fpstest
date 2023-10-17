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
    public Transform point;

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
    bool target_bet_Object = false;
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

    //부채꼴모양 색칠
    bool isCollision = false;
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    public void GetSeeUnit(float viewAngle , float len)
    {

        //플레이어의 시야 값
        //Camera.main.transform.forward = Player.transform.forward;
        //viewAngle = 180 - Camera.main.fieldOfView;


        //탐색자를 향하는 타겟의 벡터
        //A가 타겟이고 B가 탐색자일때 B가 A로 가기 위한 방향은
        //A-B이다.
        //타겟에게 향하는 방향
        if (target == null)
            TargetDir = point.position - transform.position;
        else
            TargetDir = target.position - transform.position;
        angleRange = viewAngle;
        radius = len;
        //첫번째 벡터에서 두번째 벡터로의 회전 각도입니다.
        //첫번째 벡터로 두번째 벡터가 회전하기 위한 각도를 리턴한다.
        //position값이 아니라 방향값이 들어와야한다.
        //탐색자로 부터의 포인트가 있는 각도를 구해준다. 
        //함수화 할것
        angle = Vector3.Angle(TargetDir.normalized, transform.forward );

        //타겟과의 거리가 일정 거리보다 작은경우 실행
        if (TargetDir.magnitude <= len)
        {
            if (target == null)
            {
                //장애물 여부를 확인해야함
                target_bet_Object = Find_Target_In_Lay();
            }
            //angle은 탐색자를 기준으로한 각도값 
            //viewAngle은 가변하지 않음
            //탐색자를 기준으로
            if (angle < viewAngle)
            {

                //point가 target이 되는 순간
                //장애물 여부를 확인해야함
                /*target_bet_Object = Find_Target_In_Lay();*/
                if (target_bet_Object == true)
                {
                        //오브젝트가 타겟을 볼때 장애물이 계속해서 없는가?
                        if (Physics.Raycast(transform.position, (point.position-transform.position).normalized, out RaycastHit raycastHit, radius))
                    {
                        //왜? 이게 맞음? 버그 이게 맞는 이유가 확실하다면 버그가 아니지만
                        // 내가 의도 한데로 짠게 맞으면 버그의 해결
                        // 의도한데로 되진 않았지만 이게 좋은 방법이면 기획의 수정이 되는 것
                        //테스트이기 때문에 우선순위를 따질 필요없이 무조건 적으로 가게 만들어야한다.
                        //여기서 타겟이 바뀌게 되면 안된다.
                        //조건이 장애물이 생겨서 시야에서 없어졌을때의 조건을 체크해야한다.
                        //서치함수 안에서 모든 조건을 판별해야한다.
                        
                        //뭘하고 싶은지 어떤 작동이 필요한지
                        
                        //매프레임에 타겟이 바뀔 수 있다는 전제조건이 없음
                        //타겟을 찾았을 때 ray를 분산 시키는게 전제 조건에 부합하다.
                        if (isRayCastHit(raycastHit, target_Mask))
                        {
                            Debug.Log("오브젝트와 타겟 사이에 장애물이 없습니다.");
                            //장애물이 없는데 Point 를 타겟으로 바꿀 필요가 없다.
                            target = point;

                            Spot = target.position;
                        }
                        else
                            target_bet_Object = false;                       
                    }
                    //Debug.Log(gameObject.name + "의 시야에 있습니다.");
                }
                else
                target = null;
            }
            else
                target = null;
            //Debug.Log(gameObject.name + "의 시야에 없습니다.");
        }
        else
            target = null;
        // Debug.Log(gameObject.name + "의 시야에 없습니다.");
        isCollision = false;
    }

    //히트된 객체와 LayeMask를 비교하여 같으면 true를 반환 시켜주는 함수
    bool isRayCastHit(RaycastHit raycastHit, LayerMask target_Mask)
    {
        if (0 != ((1 << raycastHit.transform.gameObject.layer) & target_Mask))
        {
            return true;
        }
        return false;
    }

    Vector3 LayDir(float Rayangle)
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
    }

   /* 
    //실행되면 매초 마다 반복하는 코루틴 stopCoroutin으로만 멈출 수 있음
    //해당 함수로 바꿀 수 있는 것은 bool값만 바꿀 수 있다.
    IEnumerator TimerCoroutin(float time)
    {
        yield return new WaitForSeconds(time);
        
        StartCoroutine(TimerCoroutin(1));
        
    }
   */

    //시야각을 나눠서 레이를 쏴서 레이를 플레이어를 찾는다.
    public bool Find_Target_In_Lay()
    {

        //Find_target가 true 이면 히트된 transform을 리턴
        //방향으로 레이를 쏜다. 쏘는 방향은 발사하려는 영역을 알면 발사하는 위치를 알기 쉬운데
        //표적의 vector - 자신의 vector를 하면 방향이 나온다.

        Debug.DrawRay(transform.position, LayDir(Rayangle) * radius, Color.black);
        if(Physics.Raycast(transform.position, LayDir(Rayangle), out RaycastHit raycastHit, radius))
        {
            //예제 코드 좀비의 아이템 부분
            //target의 타입은 GameObject
            //target_Mask는 bit shift(2^index된 값을 가진다.)
            return isRayCastHit(raycastHit, target_Mask);
        }
        return false;
    }

    //오브젝트를 회전 시켜 타겟을 찾기
    public void Not_Find_Target()
    {
        if (isDelay == true) StartCoroutine("testCoroutin");
    }
    
    IEnumerator testCoroutin()
    {
        float time = 5f;

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
 

    //시야각을 Scenes에서 보여주는 코드
    private void OnDrawGizmos()
    {
        Handles.color = isCollision ? _red : _blue;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange *0.5f, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange * 0.5f, radius);
    }
}


