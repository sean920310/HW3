using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMoveState : TutorialStateBase
{
    public TutorialMoveState(TutorialManager context) : base(context)
    {
    
    }

    public override void EnterState()
    {
        
    }

    public override void SwitchState()
    {
        if (m_context.moveLeft && m_context.moveRight)
        {
            m_context.SwitchToNextMsg();
        }
    }
}
