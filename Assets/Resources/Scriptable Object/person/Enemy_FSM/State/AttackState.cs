using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "ScriptableObject/FSM State/Attack", order = 3)]
public class AttackState : ScriptableObject, IState
{
    public void Enter(EnemyTest owner)
    {
    }
    public void Excute(EnemyTest owner)
    {
        if(!owner.AttackDurationCheck()) owner.ReadyAttack();
        else if (!owner.IsAttackBounds) owner.OnChaseState();

        owner.AttackToTarget();
    }
    public void Exit(EnemyTest owner)
    {
    }
}
