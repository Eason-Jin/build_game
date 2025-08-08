using UnityEngine;

public abstract class ObjectBuilder : MonoBehaviour
{
    public GameObject objectPrefab;      // Assign your prefab here
    public LayerMask objectLayer;        // Layer for placed objects
    protected GameObject currentObject;  // Object being placed now
    protected int objectCount = 0;       // Count of objects placed
    protected bool placingObject = false;
    protected bool rayHit = true;        // True to place the first object

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartPlacingObject();
        }

        if (placingObject)
        {
            FollowMouse();

            // Rotate the object along the x, y, and z axes
            if (Input.GetKey(KeyCode.Q))
            {
                RotateObject(Vector3.right); // Rotate along the x-axis
            }
            if (Input.GetKey(KeyCode.E))
            {
                RotateObject(Vector3.up); // Rotate along the y-axis
            }
            if (Input.GetKey(KeyCode.R))
            {
                RotateObject(Vector3.forward); // Rotate along the z-axis
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            EnablePhysics();
        }
    }

    public virtual void StartPlacingObject()
    {
        if (currentObject != null)
        {
            return;
        }

        currentObject = Instantiate(objectPrefab);
        placingObject = true;

        // Disable the collider to prevent raycast hits
        Collider objectCollider = currentObject.GetComponent<Collider>();
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    protected virtual void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (objectCount == 0)
        {
            // No objects placed yet - place freely on some ground plane, say y=0
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
            // Raycast against placed objects
            if (Physics.Raycast(ray, out hit, 100f, objectLayer))
            {
                // Snap to the surface normal of the hit object
                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                Vector3 snapPosition = hitPoint + hitNormal * 0.5f;

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
                currentObject.transform.position = new Vector3(1000, 1000, 1000);
                rayHit = false;
            }
        }
    }

    protected virtual void PlaceObject()
    {
        if (currentObject == null || rayHit == false)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, objectLayer))
        {
            currentObject.transform.SetParent(hit.collider.transform);
        }

        Collider objectCollider = currentObject.GetComponent<Collider>();
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }

        // Add the ObjectShatter script to the placed object
        if (currentObject.GetComponent<ObjectShatter>() == null)
        {
            currentObject.AddComponent<ObjectShatter>();
        }

        currentObject.layer = LayerMask.NameToLayer("PlacedObjects");
        currentObject.tag = "PlacedObject";
        currentObject = null;
        placingObject = false;
        objectCount++;
    }

    protected virtual void EnablePhysics()
    {
        GameObject[] placedObjects = GameObject.FindGameObjectsWithTag("PlacedObject");

        foreach (GameObject obj in placedObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = obj.AddComponent<Rigidbody>();
                rb.mass = 1f;
            }

            if (obj.transform.parent != null)
            {
                FixedJoint joint = obj.GetComponent<FixedJoint>();
                if (joint == null)
                {
                    joint = obj.AddComponent<FixedJoint>();
                }
                joint.connectedBody = obj.transform.parent.GetComponent<Rigidbody>();
            }
        }
    }

    protected virtual void RotateObject(Vector3 axis)
    {
        if (currentObject == null)
        {
            return;
        }

        float rotationSpeed = 100f * Time.deltaTime;
        currentObject.transform.Rotate(axis, rotationSpeed);
    }
}