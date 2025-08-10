public class CylinderBuilder : ObjectBuilder
{
    public override string Tag => "PlacedCylinder";

    public override float[] attachmentVectors => new float[] {
        0.5f,  // UP
        0.5f,  // DOWN
        0.0f,  // LEFT
        0.0f,  // RIGHT
        0.0f,  // FORWARD
        0.0f   // BACKWARD
    };
}