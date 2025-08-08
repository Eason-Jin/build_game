using UnityEngine;

public class ObjectShatter : MonoBehaviour
{
    public float shatterThreshold = 5f; // Impact speed threshold for shattering

    void OnCollisionEnter(Collision collision)
    {
        // Check the relative velocity of the collision
        if (collision.relativeVelocity.magnitude > shatterThreshold)
        {
            // Destroy the FixedJoint to shatter the object
            FixedJoint joint = GetComponent<FixedJoint>();
            if (joint != null)
            {
                Destroy(joint);
            }
        }
    }
}