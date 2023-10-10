using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ViewAngle : MonoBehaviour
{
    Vector3 TargetDir = new Vector3();
    float angle;
    public Transform target;
    public Transform point;

    //인식 거리

    float base_rotation_y;
    //바라보는 주체가 누구인지

    //부채꼴모양 색칠
    bool isCollision = false;
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);
    float angleRange;
    float radius;
    float This_Angle;
    // 탐색 장소
    public Vector3 Spot;
    Transform search_spot;
    //탐색을 위한 y축 회전값
    float y_rotation;
    //더할지 말지 관리
    bool y_Dir;
    bool isDelay = true;

    //Laymask 레이에 인식되어야할
    //target은 Player임
    LayerMask target_Mask;

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
            //angle은 탐색자를 기준으로한 각도값 
            //viewAngle은 가변하지 않음
            //탐색자를 기준으로
            if (angle < viewAngle)
            {
                target = point;
                Spot = target.position;
                //search_spot = target;
                Debug.Log(gameObject.name + "의 시야에 있습니다.");
                StopCoroutine("SearchTime");
                Find_Target_In_Lay(viewAngle);
                isCollision = true;
            }
            else
            {
                target = Find_Target_In_Lay(viewAngle);
                Debug.Log(gameObject.name + "의 시야에 없습니다.");
                
                isCollision = false;
            }
        }
        else
        {
            target = null;
            Debug.Log(gameObject.name + "의 시야에 없습니다.");
            Find_Target_In_Lay(viewAngle);
        }
        isCollision = false;
    }

    //시야각을 나눠서 레이를 쏴서 레이를 플레이어를 찾는다.
    public Transform Find_Target_In_Lay(float viewAngle)
    {

        //Find_target가 true 이면 히트된 transform을 리턴
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit , radius, target_Mask))
        {
            return raycastHit.transform;
        }
        else
        {
            //최대 시야각에 관한 Ray회전값을을 갱신하는 코드 입력
            //시야각은 현재 중심을 기준으로 변해야함
            //base는 z
        }
        return null;
        
    }

    //타겟을 찾았을때
    public void TargetFind(Coroutine coroutine)
    {

       // base_rotation_y = transform.rotation.y;
        if (isDelay)
        { 
            coroutine = StartCoroutine(SearchTime());
            //정면
            y_rotation = transform.rotation.y;
            isDelay = false;
        }

       
    }

    //탐색시간 만큼 탐색을 반복해준다.
    public IEnumerator SearchTime()
    {
        while (true) 
        {
            yield return new WaitForSeconds(3f);
            
            switch (y_Dir)
            {
                case true:
                    if (y_rotation < -40)
                        y_rotation = -40;
                    else if (y_rotation == -40)
                        y_Dir = false;
                    else
                        y_rotation -= 10;
                    break;
                case false:
                    if (y_rotation > 40)
                        y_rotation = 40;
                    else if(y_rotation == 40)
                        y_Dir = true;
                    else
                        y_rotation += 10;
                    break;
            }
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + y_rotation, transform.rotation.z);
            isDelay=true;
        }

    }

    //시야각을 Scenes에서 보여주는 코드
    private void OnDrawGizmos()
    {
        Handles.color = isCollision ? _red : _blue;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
    }
}


