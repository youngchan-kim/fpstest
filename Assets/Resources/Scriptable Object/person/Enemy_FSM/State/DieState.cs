using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Die State", menuName = "ScriptableObject/FSM State/Die", order = 5)]
public class DieState : ScriptableObject,IState
{
    public void Enter(EnemyTest owner)
    {
    }
    public void Excute(EnemyTest owner)
    {
    }
    public void Exit(EnemyTest owner)
    {
    }
}
