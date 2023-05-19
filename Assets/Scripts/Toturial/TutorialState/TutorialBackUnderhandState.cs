using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBackUnderhandState : TutorialStateBase
{
    public TutorialBackUnderhandState(TutorialManager context) : base(context)
    {
    
    }

    public override void EnterState()
    {
        m_context.EnemyOverhandSetup();
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
            if (!m_context.underhandBack)
            {
                m_context.RepeatCurrentMsg();
                return;
            }

            m_context.SwitchToNextMsg();
        }
    }
}
