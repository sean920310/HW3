using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialJumpState : TutorialStateBase
{
    public TutorialJumpState(TutorialManager context) : base(context)
    {
    
    }

    public override void EnterState()
    {
    
    }

    public override void SwitchState()
    {
        if (m_context.jumpInputFlag)
        {
            m_context.SwitchToNextMsg();
        }
    }
}
