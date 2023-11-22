using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    EnemyTest owner;
    IState currState = null;

    FSM() { }
    public FSM(EnemyTest owner) { this.owner = owner; }
    public void Update() { currState.Excute(owner); }
    
    //현재 state를 설정하는 함수
    public bool SetCurrState(IState state)
    {
        if (null == state) return false;
        currState = state;
        Debug.Log("SetCurrState : " + state);
        return true;
    }

    //state를 바꿔주는 함수
    public bool ChangeState(IState state)
    {
        if(null == state) return false;
        currState.Exit(owner);
        currState = state;
        currState.Enter(owner);
        return true;
    }

}
