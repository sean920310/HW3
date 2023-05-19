using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum TutorialType
{
    Beginning,
    ShortServe,
    LongServe,
    Move,
    Underhand,
    Overhand,
    Jump,
    SwitchToAdvenced,
    Smash,
    AdvencedShortServe,
    FrontUnderhand,
    BackUnderhand,
    End
}

public class TutorialStatesFactory
{
    private Dictionary<TutorialType, Func<TutorialStateBase>> toturialMap = new Dictionary<TutorialType, Func<TutorialStateBase>>();

    public TutorialStatesFactory(TutorialManager context)
    {
        // Register object types and their constructors in the dictionary
        RegisterObjectType(TutorialType.Beginning, () => new TutorialBeginningState(context));
        RegisterObjectType(TutorialType.ShortServe, () => new TutorialShortServeState(context));
        RegisterObjectType(TutorialType.LongServe, () => new TutorialLongServeState(context));
        RegisterObjectType(TutorialType.Move, () => new TutorialMoveState(context));
        RegisterObjectType(TutorialType.Underhand, () => new TutorialUnderhandState(context));
        RegisterObjectType(TutorialType.Overhand, () => new TutorialOverhandState(context));
        RegisterObjectType(TutorialType.Jump, () => new TutorialJumpState(context));
        RegisterObjectType(TutorialType.SwitchToAdvenced, () => new TutorialSwitchToAdvencedState(context));
        RegisterObjectType(TutorialType.Smash, () => new TutorialSmashState(context));
        RegisterObjectType(TutorialType.AdvencedShortServe, () => new TutorialAdvencedShortServeState(context));
        RegisterObjectType(TutorialType.FrontUnderhand, () => new TutorialFrontUnderhandState(context));
        RegisterObjectType(TutorialType.BackUnderhand, () => new TutorialBackUnderhandState(context));
        RegisterObjectType(TutorialType.End, () => new TutorialEndState(context));
    }
    public void RegisterObjectType(TutorialType toturialType, Func<TutorialStateBase> constructor)
    {
        toturialMap[toturialType] = constructor;
    }

    public TutorialStateBase CreateObject(TutorialType objectType)
    {
        if (toturialMap.ContainsKey(objectType))
        {
            return toturialMap[objectType].Invoke();
        }
        else
        {
            Debug.LogError("Invalid object type: " + objectType);
            return null;
        }
    }
}
