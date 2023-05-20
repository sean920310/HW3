using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialStateBase : ITutorialState
{
    protected TutorialManager m_context;

    public TutorialStateBase(TutorialManager context)
    {
        m_context = context;
    }

    abstract public void EnterState();
    abstract public void SwitchState();
}