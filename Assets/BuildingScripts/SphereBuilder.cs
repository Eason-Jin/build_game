namespace BuildingScripts
{
    public class SphereBuilder : ObjectBuilder
    {
        public override string Tag => "PlacedSphere";

        public override float[] attachmentVectors => new float[] {
        0.0f,  // UP
        0.0f,  // DOWN
        0.0f,  // LEFT
        0.0f,  // RIGHT
        0.0f,  // FORWARD
        0.0f   // BACKWARD
        };
    }
}