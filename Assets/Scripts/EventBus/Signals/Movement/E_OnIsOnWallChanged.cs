using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OnIsOnWallChanged : TouchingDirectionsEvent
{
    public E_OnIsOnWallChanged(TouchingDirections touchingDirections)
    {
        this.touchingDirections = touchingDirections;
    }
}
