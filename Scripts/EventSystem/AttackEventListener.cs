using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackEventListener : MonoBehaviour
{
    public AttackEvent Event;
    public AttackUnityEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(Attack player)
    { Response.Invoke(player); }
}

[Serializable]
public class AttackUnityEvent : UnityEvent<Attack>
{
}