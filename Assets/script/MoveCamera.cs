using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 스크립트 실행하면 아바타의 움직임대로 움직임
//(아바타의 부위에 고정하는 코드이기 때문에 뛸때 카메라도 같이 움직임)
public class MoveCamera : MonoBehaviour
{
    //카메라의 포지션에 케릭터의 포지션을 넣으면
    //카메라는 케릭터와  같은 포지션을 유지하게됨
    public Transform cameraPosition;
    public Transform Orientation;
    Transform Head;
    Vector3 pos;
    private void Start()
    {
        Head = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0);
    }
    // Update is called once per frame
    public void Update()
    {
        //Debug.Log(Head.name);
        /*pos = Head.position + (Vector3.up * 0.1f) + (Vector3.forward * 0.1f);
        cameraPosition.position = pos;*/
        cameraPosition.position = Head.position + (Vector3.up * 0.1f) + (Vector3.forward * 0.1f);
       
    }
}
