using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "ScriptableObject/FSM State/Idle", order = 1)]
public class IdleState : ScriptableObject, IState
{
    public void Enter(EnemyTest owner)
    {
        owner.MoveStop();
    }
    public void Excute(EnemyTest owner)
    {
        if (owner.FindTarget())
        {
            if (owner.IsAttackBounds) owner.OnAttackState();
            else owner.OnChaseState();
        }
    }
    public void Exit(EnemyTest owner)
    {
    }
}
