public class CubeBuilder : ObjectBuilder
{
    public override string Tag => "PlacedCube";

    public override int[] attachmentVectors => new int[] {
        1,  // UP
        1,  // DOWN
        1,  // LEFT
        1,  // RIGHT
        1,  // FORWARD
        1   // BACKWARD
    };
}
