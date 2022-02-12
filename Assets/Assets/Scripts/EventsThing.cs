using System;
using System.Collections;
using System.Collections.Generic;

public enum CustomEvents
{
    bpmChange,
    playAnimation
}

[Serializable]
public class EventsThing
{
    public List<EventThing> Events;
}

public class EventThing
{
    public float time;
    public string eventName;

    public string[] args;
}
