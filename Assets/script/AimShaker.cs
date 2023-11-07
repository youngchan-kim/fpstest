using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimShaker : MonoBehaviour
{
    float x = 0;
    float y = 0;
    //총구가 바라보는 곳의 position를 변경해준다.?
    Vector3 Look_aim;
    

    public IEnumerator AimShake(float spreed)
    {
        //정해진 크기만큼 랜덤하게 x,y축을 변형
        x = Random.Range(-1.1f, 1.1f) * spreed;
        y = Random.Range(0f, 2f) * spreed;
        Look_aim.x += x;
        Look_aim.y += y;

        yield return null;
    }

    public Vector3 GetLookpoint() { return Look_aim; }

    public float GetAimX()
    {
        if (x == 0)
            return 0;
        else
            return x;
    }
    public float GetAimY()
    {
        if (y == 0)
            return 0;
        else
            return y;
    }
}
