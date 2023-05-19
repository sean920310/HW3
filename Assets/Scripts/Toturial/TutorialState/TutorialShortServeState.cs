using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShortServeState : TutorialStateBase
{
    public TutorialShortServeState(TutorialManager context) : base(context)
    {

    }

    public override void EnterState()
    {
        m_context.PlayerServeSetup();
    }

    public override void SwitchState()
    {
        if (m_context.swinUpInputFlag || m_context.hitPlayerGround)
        {
            m_context.RepeatCurrentMsg();
            return;
        }
        if (m_context.swinDownInputFlag && m_context.hitEnemyGround)
        {
            m_context.SwitchToNextMsg();
        }
    }
}
