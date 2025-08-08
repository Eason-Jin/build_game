using UnityEngine;

public class CubeBuilder : MonoBehaviour
{
    public GameObject objectPrefab;
    public LayerMask objectLayer;
    private GameObject currentObject;
    private int objectCount = 0;
    private bool placingObject = false;
    private bool rayHit = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartplacingObject();
        }

        if (placingObject)
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceCube();
            }
        }

        // Enable physics when "V" is pressed
        if (Input.GetKeyDown(KeyCode.V))
        {
            EnablePhysics();
        }
    }

    void EnablePhysics()
    {
        // Find all placed cubes
        GameObject[] placedCubes = GameObject.FindGameObjectsWithTag("PlacedObject");

        // Add Rigidbody and FixedJoint to each cube
        foreach (GameObject cube in placedCubes)
        {
            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = cube.AddComponent<Rigidbody>();
                rb.mass = 1f; // Set mass or other Rigidbody properties as needed
            }

            // If the cube has a parent, add a FixedJoint to connect it to the parent
            if (cube.transform.parent != null)
            {
                FixedJoint joint = cube.GetComponent<FixedJoint>();
                if (joint == null)
                {
                    joint = cube.AddComponent<FixedJoint>();
                }
                joint.connectedBody = cube.transform.parent.GetComponent<Rigidbody>();
            }

            if (cube.GetComponent<ObjectShatter>() == null)
            {
                cube.AddComponent<ObjectShatter>();
            }
        }
    }

    void StartplacingObject()
    {
        if (currentObject != null)
        {
            return;
        }

        currentObject = Instantiate(objectPrefab);
        placingObject = true;

        // Disable the collider to prevent raycast hits
        Collider cubeCollider = currentObject.GetComponent<Collider>();
        if (cubeCollider != null)
        {
            cubeCollider.enabled = false;
        }
    }


    void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (objectCount == 0)
        {
            // No cubes placed yet - place freely on some ground plane, say y=0
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            if (ground.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                currentObject.transform.position = new Vector3(
                    Mathf.Round(hitPoint.x),
                    Mathf.Round(hitPoint.y),
                    Mathf.Round(hitPoint.z)
                );
            }
        }
        else
        {
            // Raycast against placed cubes
            if (Physics.Raycast(ray, out hit, 100f, objectLayer))
            {
                // Snap to the surface normal of the hit cube
                Vector3 hitPoint = hit.point; // Exact point of the raycast hit
                Vector3 hitNormal = hit.normal; // Surface normal at the hit point

                Vector3 snapPosition = hitPoint + hitNormal*0.5f;

                snapPosition = new Vector3(
                    Mathf.Round(snapPosition.x),
                    Mathf.Round(snapPosition.y),
                    Mathf.Round(snapPosition.z)
                );
                currentObject.transform.position = snapPosition;
                rayHit = true;
            }
            else
            {
                // If no cube hit, hide the cube or place it out of sight
                currentObject.transform.position = new Vector3(1000, 1000, 1000);
                rayHit = false;
            }
        }
    }

    void PlaceCube()
    {
        if (currentObject == null || rayHit == false)
        {
            return;
        }

        // Check if the current cube is snapping to another cube
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, objectLayer))
        {
            // Parent the new cube to the cube it is snapping to
            currentObject.transform.SetParent(hit.collider.transform);
        }

        // Re-enable the collider
        Collider objectCollider = currentObject.GetComponent<Collider>();
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }

        // Finalize placement
        currentObject.layer = LayerMask.NameToLayer("PlacedObjects");
        currentObject.tag = "PlacedObject";
        currentObject = null;
        placingObject = false;
        objectCount++;
    }
}
