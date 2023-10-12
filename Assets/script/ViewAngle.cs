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

    //회전하기 전의 y축 값
    public float base_rotation_y;
    //회전하기 전의 y축 값
    public float var_rotation_y;
    //부채꼴모양 색칠
    bool isCollision = false;
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);
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
    float second =0;
    //Laymask 레이에 인식되어야할
    //target은 Player임
    //LayerMask target_Mask;


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
                //Lay를 쏴서 정면을 기준
                target = point;
                Spot = target.position;
                //Debug.Log(gameObject.name + "의 시야에 있습니다.");
                isCollision = true;
            }
            else
            {
                //Debug.Log(gameObject.name + "의 시야에 없습니다.");
                
                isCollision = false;
            }
        }
        else
        {
            target = null;
           // Debug.Log(gameObject.name + "의 시야에 없습니다.");
        }
        isCollision = false;
    }

    /*    //시야각을 나눠서 레이를 쏴서 레이를 플레이어를 찾는다.
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

        }*/

    //오브젝트를 회전 시켜 타겟을 찾기
    public void Not_Find_Target()
    {


        if (isDelay == true)
            StartCoroutine("testCoroutin");


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

        base_rotation = Quaternion.Euler(SpotDir.normalized.x, SpotDir.normalized.y, SpotDir.normalized.y);
        //Quaternion의 y라는 float값을 
        //base_rotation_y = transform.rotation.y;

        base_rotation_x = SpotDir.normalized.y;

        //base_rotation_z = transform.rotation.z;

        //y_rotation = 0;
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


