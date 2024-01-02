using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_ScreenCenter : MonoBehaviour
{
    public RaycastHit Aimrayhit;
    public RaycastHit rayhitObject;

    private float range;
    private float maxrange = 100f;

    GameObject aim; //에임의 위치를 set하기 위해 사용


    private Vector3 ScreenMax;
    private Vector3 ScreenMin;

    public AimShaker aimshaker;

    public LayerMask layermask;
    Camera maincam;

    [SerializeField]
    [Header("RayStart_Position")]
    public Transform TPSCam_Holder;
    public Transform Dir;
    private void Start()
    {
        //Debug.Log(aimshaker.name);
        //Camera는 메인 카메라를 tag로 찾는다.
        maincam = Camera.main;
        range = maxrange;
        aim = transform.GetChild(1).gameObject;
        Dir = transform.GetChild(0).GetChild(1).GetChild(2);
    }
    private void Update()
    {
        //뷰포인트 상에서의 가로세로는 0~1로 정할 수 있는 UV 좌표계를 사용한다.
        //중간 값인 0.5f를 사용했고 월드좌표상으로 카메라와 떨어져있을 거리를 입력하면 
        //화면에서는 항상 중앙에 있지만 거리또한 항상 일정한 좌표가 완성된다.

        //플레이어 앞에 고정 시킬것
        ScreenMin = new Vector3(0.5f, 0.5f, 0);

        ScreenMax = new Vector3(0.5f, 0.5f, range);


        //문제는 카메라가 움직이면 뷰포인트가 바라보는 월드상의 좌표가 달라지는 데 계속 업데이트 해줘야
        //뷰포인트의 중간점을 갱신하게 되는데 업데이트 하지않고 한번만 호출한게 문제였다.
        //카메라가 바라보는 곳의 중앙

        Vector3 ScreenMinPoint = maincam.ViewportToWorldPoint(ScreenMin);
        Vector3 ScreenMaxPoint = maincam.ViewportToWorldPoint(ScreenMax);
        
        //Ray의 방향은 화면으로 부터의 중앙으로 최대최소거리의 빼기값
        Vector3 diraction = ScreenMaxPoint - ScreenMinPoint;

        //플레이어가 보는 곳에 물체가 있는 경우
        //에임에 걸리는 물체들은 layermask에 적용
        //Ray의 시작 지점은 Camhold가 있는 곳
        
        if (Physics.Raycast(TPSCam_Holder.position, diraction, out rayhitObject, maxrange, layermask))
        {
            aim.transform.position = rayhitObject.point;
        }
        else //ray가 없는 경우 에임의 위치를 변경
        {
            //에임의 위치를 확인하기 위한 코드 최대에임 위치
            aim.transform.position = ScreenMaxPoint;
            range = maxrange;
        }
        Dir.LookAt(aim.transform.position); 
        //Debug.Log("출력");
        Debug.DrawLine(Camera.main.transform.position, ScreenMaxPoint, Color.yellow);
    }

    public void SetMaxRange(float anotherrange)
    {
        maxrange = anotherrange;
    }
}
