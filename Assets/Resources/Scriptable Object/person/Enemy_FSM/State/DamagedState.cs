using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damaged State", menuName = "ScriptableObject/FSM State/Damaged", order = 4)]
public class DamagedState : ScriptableObject, IState
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