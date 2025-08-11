namespace BuildingScripts
{
    public class TriangleBuilder : ObjectBuilder
    {
        public override string Tag => "PlacedTriangle";
        public override float[] attachmentVectors => new float[] {
        0.0f,  // UP
        0.5f,  // DOWN
        0.5f,  // LEFT
        0.0f,  // RIGHT
        0.5f,  // FORWARD
        0.5f   // BACKWARD
        };


    }
}