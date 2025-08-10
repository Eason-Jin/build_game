using System.Collections.Generic;
using UnityEngine;

public class CubeBuilder : ObjectBuilder
{
    public override string Tag => "PlacedCube";

    public override List<Vector3> attachmentDirections => new List<Vector3>
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };
}
