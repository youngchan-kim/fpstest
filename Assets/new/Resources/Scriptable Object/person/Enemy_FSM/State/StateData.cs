using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    void Enter(EnemyTest owner);
    void Excute(EnemyTest owner);
    void Exit(EnemyTest owner);
}
public class StateData : ScriptableObject
{
    public IState IdleState { get; private set; }
    public IState ChaseState { get; private set; }
    public IState AttackState { get; private set; }
    public IState DamagedState { get; private set; }
    public IState DieState { get; private set; }

    public void SetData(IState idle, IState chase, IState attack, IState damaged, IState die)
    {
        IdleState = idle;
        ChaseState = chase;
        AttackState = attack;
        DamagedState = damaged;
        DieState = die;
    }
}
