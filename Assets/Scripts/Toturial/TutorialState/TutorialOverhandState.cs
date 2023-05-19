using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOverhandState : TutorialStateBase
{
    public TutorialOverhandState(TutorialManager context) : base(context)
    {

    }

    public override void EnterState()
    {
        m_context.EnemyOverhandSetup();
    }

    public override void SwitchState()
    {
        if (m_context.swinDownInputFlag || m_context.hitPlayerGround)
        {
            m_context.RepeatCurrentMsg();
            return;
        }
        if (m_context.swinUpInputFlag && m_context.hitEnemyGround)
        {
            m_context.SwitchToNextMsg();
        }
    }
}
