using Unity.VisualScripting;
using UnityEngine;

public abstract class ObjectBuilder : MonoBehaviour
{
    public GameObject objectPrefab;
    public LayerMask objectLayer;
    protected GameObject currentObject;
    private static int objectCount = 0;
    private bool placingObject = false;
    private bool rayHit = true;

    private static ObjectBuilder activeBuilder; // Tracks the currently active builder

    void Update()
    {
        // Switch to CubeBuilder when "X" is pressed
        if (Input.GetKeyDown(KeyCode.X))
        {
            SwitchBuilder<CubeBuilder>();
        }

        // Switch to CylinderBuilder when "C" is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchBuilder<CylinderBuilder>();
        }

        // Cancel placing object when "Escape" is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearBuilder();
        }

        // Rotate the current object
        if (placingObject && currentObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateObject(Vector3.right); // Rotate around x-axis
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                RotateObject(Vector3.up); // Rotate around y-axis
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject(Vector3.forward); // Rotate around z-axis
            }
        }

        if (placingObject)
        {
            FollowMouse();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }

        // Enable physics when "V" is pressed
        if (Input.GetKeyDown(KeyCode.V))
        {
            EnablePhysics();
        }
    }

    private void RotateObject(Vector3 axis)
    {
        float rotationAngle = 90f; // Fixed rotation angle
        currentObject.transform.Rotate(axis * rotationAngle, Space.World);
    }

    private void SwitchBuilder<T>() where T : ObjectBuilder
    {
        if (activeBuilder != null)
        {
            activeBuilder.CancelPlacingObject(); // Stop the current builder
        }

        activeBuilder = FindObjectOfType<T>();
        if (activeBuilder != null)
        {
            activeBuilder.StartplacingObject();
        }
    }

    private void ClearBuilder()
    {
        if (activeBuilder != null)
        {
            activeBuilder.CancelPlacingObject(); // Stop the current builder
        }
        activeBuilder = null;
    }

    void EnablePhysics()
    {
        GameObject[] placedObjects = GameObject.FindGameObjectsWithTag("PlacedCube");
        foreach (GameObject obj in placedObjects)
        {
            AddPhysics(obj);
        }

        placedObjects = GameObject.FindGameObjectsWithTag("PlacedCylinder");
        foreach (GameObject obj in placedObjects)
        {
            AddPhysics(obj);
        }
    }

    private void AddPhysics(GameObject obj)
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

        if (obj.GetComponent<ObjectShatter>() == null)
        {
            obj.AddComponent<ObjectShatter>();
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

        Collider objectCollider = currentObject.GetComponent<Collider>();
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    void CancelPlacingObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
        }
        placingObject = false;
    }

    void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (objectCount == 0)
        {
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
            if (Physics.Raycast(ray, out hit, 100f, objectLayer))
            {
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

    void PlaceObject()
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

        currentObject.layer = LayerMask.NameToLayer("PlacedObjects");

        // Assign tag based on the active builder
        if (this is CubeBuilder)
        {
            currentObject.tag = "PlacedCube";
        }
        else if (this is CylinderBuilder)
        {
            currentObject.tag = "PlacedCylinder";
        }

        currentObject = null;
        placingObject = false;
        objectCount++;
    }
}
