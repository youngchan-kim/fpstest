using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    //비동기 작업과 비슷한 작업을 하게 해줌
    //총알이 나가는 동시에 흔들리는 효과가 들어가야하기 때문
    public IEnumerator Shake (float duration, float magnitude)
    {
        
        //카메라의 기본 위치에서
        Vector3 originalPos = transform.localPosition;
        
        //호출 될 때 마다 초기화 되어야함
        float elapsed = 0.0f;

        //시간 값이 들어오면
        //시간 값보다 경과 시간이 작으면 계속 반복
        while (elapsed < duration) 
        {
            //정해진 크기만큼 랜덤하게 x,y축을 변형
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            //카메라의 로컬위치에 해당 값을 대입
            transform.localPosition = new Vector3(x, y, originalPos.z);

            //경과시간 은 실제지나간 시간을 더한 값으로 경과시간을 체크
            elapsed += Time.deltaTime;

            yield return null;
        }
        //예제에서는 복귀 하지만 복귀할 필요가 없음
        //처음 위치로 카메라의 위치를 복귀
        transform.localPosition = originalPos;

    }

}
