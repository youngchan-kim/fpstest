using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimShaker : MonoBehaviour
{
    float x = 0;
    float y = 0;
    //총구가 바라보는 곳의 position를 변경해준다.?
    Vector2 Look_aim;


    public IEnumerator AimShake(float spreed)
    {
        //정해진 크기만큼 랜덤하게 x,y축을 변형
        x = Random.Range(-1.1f, 1.1f) * spreed;
        y = Random.Range(0f, 2f) * spreed;
        Look_aim.x += x;
        Look_aim.y += y;

        yield return null;
        x = y = 0;

    }

    //사용되는 곳 찾아볼 것
    public Vector2 GetLookpoint() { return Look_aim; }

    //사용 되는 곳 에임중 마우스의 값을 강제적으로 추가할때 사용
    public float GetAimX()
    {
        return x;
    }
    public float GetAimY()
    {
        return y;
    }
}