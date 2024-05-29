using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OnIsGroundedChanged : TouchingDirectionsEvent
{
    public E_OnIsGroundedChanged(TouchingDirections touchingDirections)
    {
        this.touchingDirections = touchingDirections;
    }
}
