using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFrontUnderhandState : TutorialStateBase
{
    public TutorialFrontUnderhandState(TutorialManager context) : base(context)
    {
    
    }

    public override void EnterState()
    {
        m_context.EnemyUnderhandSetup();
    }

    public override void SwitchState()
    {

        if (m_context.hitPlayerGround)
        {
            m_context.RepeatCurrentMsg();
            return;
        }

        if (m_context.hitEnemyGround)
        {
            if (!m_context.underhandFront)
            {
                m_context.RepeatCurrentMsg();
                return;
            }

            m_context.SwitchToNextMsg();
        }
    }
}
