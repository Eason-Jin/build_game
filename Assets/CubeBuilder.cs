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
}
