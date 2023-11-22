using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Transform player_Rotation;
    //[SerializeField] public 
    // 주로 오브젝트를 따라가게 설정한 카메라는 LateUpdate 를 사용합니다
    // 카메라가 따라가는 오브젝트가 Update함수 안에서 움직일 경우가 있기 때문
    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player_Rotation.eulerAngles.y, 0f);
    }
}
