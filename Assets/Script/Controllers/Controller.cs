﻿using System;
using UnityEngine;

public abstract class Controller : MonoBehaviour, IController
{
    public ActionSet actionSet;
    public ControllerSet controllerSet;
    public Character controllerCharacter;
    
    protected void InitializeActionSet<A>() where A : ActionSet
    {
        actionSet = (A) Activator.CreateInstance(typeof(A));
        actionSet.actionController = this;
        actionSet.actor = controllerSet.controlledCharacter;
        actionSet.OnInitialize();
    }
}