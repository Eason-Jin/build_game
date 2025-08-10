using UnityEngine;

public class CubeBuilder : ObjectBuilder
{
    public override string Tag => "PlacedCube";

    public override float[] attachmentVectors => new float[] {
        0.5f,  // UP
        0.5f,  // DOWN
        0.5f,  // LEFT
        0.5f,  // RIGHT
        0.5f,  // FORWARD
        0.5f   // BACKWARD
    };

    protected override void AddCollider()
    {
        BoxCollider boxCollider = currentObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(
            attachmentVectors[0] + attachmentVectors[1],
            attachmentVectors[2] + attachmentVectors[3],
            attachmentVectors[4] + attachmentVectors[5]);
        boxCollider.center = Vector3.zero;
    }
}
