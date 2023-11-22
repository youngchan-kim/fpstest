using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase State", menuName = "ScriptableObject/FSM State/Chase", order = 2)]
public class ChaseState : ScriptableObject, IState
{
    public void Enter(EnemyTest owner)
    {
    }
    public void Excute(EnemyTest owner)
    {
        if (owner.TargetLostCheck()) owner.OnIdleState();
        else if (owner.IsAttackBounds) owner.OnAttackState();
        owner.MoveToTarget();
    }
    public void Exit(EnemyTest owner)
    {
    }
}
