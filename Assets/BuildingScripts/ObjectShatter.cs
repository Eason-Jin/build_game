namespace BuildingScripts
{
    using UnityEngine;

    public class ObjectShatter : MonoBehaviour
    {
        public float shatterThreshold = 1.0f; // Impact speed threshold for shattering

        void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > shatterThreshold)
            {
                FixedJoint joint = gameObject.GetComponent<FixedJoint>();
                if (joint != null)
                {
                    Destroy(joint);
                }
            }
        }
    }
}