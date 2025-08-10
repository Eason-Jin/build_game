using System.Collections.Generic;
using UnityEngine;

public class CylinderBuilder : ObjectBuilder
{
    public override string Tag => "PlacedCylinder";

    public override List<Vector3> attachmentDirections => new List<Vector3>
    {
        Vector3.up,
        Vector3.down
    };
}