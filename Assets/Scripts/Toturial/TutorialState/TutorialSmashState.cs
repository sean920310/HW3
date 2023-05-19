using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSmashState : TutorialStateBase
{
    public TutorialSmashState(TutorialManager context) : base(context)
    {
    
    }

    public override void EnterState()
    {
        m_context.EnemyOverhandSetup();
    }

    public override void SwitchState()
    {
        if (!m_context.smash && m_context.hitEnemyGround || m_context.hitPlayerGround)
        {
            m_context.RepeatCurrentMsg();
            return;
        }
        if (m_context.smash && m_context.hitEnemyGround)
        {
            m_context.SwitchToNextMsg();
        }
    }
}
