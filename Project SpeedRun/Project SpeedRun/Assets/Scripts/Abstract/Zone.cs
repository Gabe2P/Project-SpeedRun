using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{

    public enum Origin { Down, Above, Right, Left }

    public Origin zoneOrigin; // tells us the origin of the zone because after the walker finishes it places the tiles in reverse order

    public Vector2 gridPos;

    public int zoneType; // 0 = start, 1 = straight, 2 = curved, 3 = tJunct, 4 = cross, 5 = deadEnd

    public Zone(Vector2 _gridPos, Origin _zoneOrigin)
    {

        zoneOrigin = _zoneOrigin;
        gridPos = _gridPos;

    }

}
