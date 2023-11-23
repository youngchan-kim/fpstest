using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class StaticViewAngle
{
    //타겟을 받아 타겟과의 방향을 얻을 수 있는 함수.
    //사용법 : transform값을 매개변수로 넣어주면 오브젝트와의 방향을 얻을 수있다.
    //방향으로 사용하기 위해서는 normalized을 따로 해줘야한다.
    public static Vector3 static_Obj_to_Target_Dir(Vector3 checktarget, Vector3 Objectpos)
    {
        return checktarget - Objectpos;
    }
    //타겟의 방향을 매개변수로 받고 타겟이 오브젝트로 부터 몇도의 각에 있는지 리턴
    //사용법 : Object_to_Target_Dir함수에 normalized한 값을 넣어주면된다.
    public static float static_Obj_ViewAngle_in_Target_Angle(Vector3 TargetDir, Vector3 ObjectDir)
    {
        return Vector3.Angle(TargetDir.normalized, ObjectDir);
    }
}


