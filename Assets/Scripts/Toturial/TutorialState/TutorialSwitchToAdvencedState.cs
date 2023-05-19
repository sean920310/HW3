using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSwitchToAdvencedState : TutorialStateBase
{
    public TutorialSwitchToAdvencedState(TutorialManager context) : base(context)
    {
    
    }

    public override void EnterState()
    {
        m_context.NoActionSetup();
    }

    public override void SwitchState()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_context.SwitchToNextMsg();
        }
    }
}
