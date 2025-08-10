public class CylinderBuilder : ObjectBuilder
{
    public override string Tag => "PlacedCylinder";

    public override int[] attachmentVectors => new int[] {
        1,  // UP
        1,  // DOWN
        0,  // LEFT
        0,  // RIGHT
        0,  // FORWARD
        0   // BACKWARD
    };
}