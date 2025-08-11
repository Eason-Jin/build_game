namespace BuildingScripts
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ObjectBuilder : MonoBehaviour
    {
        public abstract string Tag { get; }
        public abstract float[] attachmentVectors { get; }    // UP, DOWN, LEFT, RIGHT, FORWARD, BACKWARD
        public GameObject objectPrefab;
        public LayerMask objectLayer;
        public bool placingObject = false;

        protected GameObject currentObject;
        protected static int objectCount = 0;

        public static ObjectBuilder activeBuilder;
        private bool rayHit = true;


        /* 
         *  ____        _     _ _        _____                 _   _                 
         * |  _ \ _   _| |__ | (_) ___  |  ___|   _ _ __   ___| |_(_) ___  _ __  ___ 
         * | |_) | | | | '_ \| | |/ __| | |_ | | | | '_ \ / __| __| |/ _ \| '_ \/ __|
         * |  __/| |_| | |_) | | | (__  |  _|| |_| | | | | (__| |_| | (_) | | | \__ \
         * |_|    \__,_|_.__/|_|_|\___| |_|   \__,_|_| |_|\___|\__|_|\___/|_| |_|___/
         */

        void Update()
        {

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
        }

        public void StartplacingObject()
        {
            if (currentObject != null)
            {
                return;
            }

            currentObject = Instantiate(objectPrefab);
            currentObject.name = objectPrefab.name + "_" + objectCount;
            currentObject.AddComponent<AttachmentVectors>();
            currentObject.GetComponent<AttachmentVectors>().Initialise(currentObject, attachmentVectors);
            currentObject.GetComponent<AttachmentVectors>().DrawVectors(true);
            placingObject = true;

            Collider objectCollider = currentObject.GetComponent<Collider>();
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
            }
        }

        public void CancelPlacingObject()
        {
            Debug.Log("Cancel placing object");
            Debug.Log("Current object: " + currentObject);
            if (currentObject != null)
            {
                Destroy(currentObject);
                currentObject = null;
            }
            placingObject = false;
        }

        public void EnablePhysics()
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.tag.StartsWith("Placed") && !obj.name.EndsWith("Builder"))
                {
                    AddPhysics(obj);
                }
            }
        }

        /*
         *  ____            _            _           _   _____                 _   _                 
         * |  _ \ _ __ ___ | |_ ___  ___| |_ ___  __| | |  ___|   _ _ __   ___| |_(_) ___  _ __  ___ 
         * | |_) | '__/ _ \| __/ _ \/ __| __/ _ \/ _` | | |_ | | | | '_ \ / __| __| |/ _ \| '_ \/ __|
         * |  __/| | | (_) | ||  __/ (__| ||  __/ (_| | |  _|| |_| | | | | (__| |_| | (_) | | | \__ \
         * |_|   |_|  \___/ \__\___|\___|\__\___|\__,_| |_|   \__,_|_| |_|\___|\__|_|\___/|_| |_|___/
         */

        protected virtual void AddCollider()
        {
            MeshCollider meshCollider = currentObject.AddComponent<MeshCollider>();
            MeshFilter meshFilter = currentObject.GetComponent<MeshFilter>();

            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                meshCollider.sharedMesh = meshFilter.sharedMesh;
                meshCollider.convex = true;
            }
            else
            {
                Debug.LogWarning("MeshFilter or sharedMesh is missing. MeshCollider cannot be added.");
            }
        }

        /*
         *  ____       _            _         _____                 _   _                 
         * |  _ \ _ __(_)_   ____ _| |_ ___  |  ___|   _ _ __   ___| |_(_) ___  _ __  ___ 
         * | |_) | '__| \ \ / / _` | __/ _ \ | |_ | | | | '_ \ / __| __| |/ _ \| '_ \/ __|
         * |  __/| |  | |\ V / (_| | ||  __/ |  _|| |_| | | | | (__| |_| | (_) | | | \__ \
         * |_|   |_|  |_| \_/ \__,_|\__\___| |_|   \__,_|_| |_|\___|\__|_|\___/|_| |_|___/
         */

        private void RotateObject(Vector3 axis)
        {
            float rotationAngle = 90f;
            currentObject.transform.Rotate(axis * rotationAngle, Space.World);
        }

        private void ClearBuilder()
        {
            if (activeBuilder != null)
            {
                activeBuilder.CancelPlacingObject();
            }
            activeBuilder = null;
        }

        private void AddPhysics(GameObject obj)
        {

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = obj.AddComponent<Rigidbody>();
                rb.mass = 1f;
            }

            GameObject parent = obj.transform.parent != null ? obj.transform.parent.gameObject : null;
            if (parent != null)
            {
                AttachmentVectors objAttachment = obj.GetComponent<AttachmentVectors>();
                AttachmentVectors parentAttachment = parent.GetComponent<AttachmentVectors>();

                if (objAttachment != null && parentAttachment != null)
                {
                    List<Vector3> objEndPoints = objAttachment.GetEndingPoints();
                    List<Vector3> parentEndPoints = parentAttachment.GetEndingPoints();

                    foreach (Vector3 objPoint in objEndPoints)
                    {
                        foreach (Vector3 parentPoint in parentEndPoints)
                        {
                            if (Vector3.Distance(objPoint, parentPoint) < 0.01f) // Allow for floating-point imprecision
                            {
                                FixedJoint joint = obj.GetComponent<FixedJoint>();
                                if (joint == null)
                                {
                                    joint = obj.AddComponent<FixedJoint>();
                                    obj.AddComponent<ObjectShatter>();
                                }
                                joint.connectedBody = parent.GetComponent<Rigidbody>();
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void FollowMouse()
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
        private void PlaceObject()
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
            currentObject.layer = LayerMask.NameToLayer("PlacedObjects");

            AddCollider();
            currentObject.tag = tag;
            currentObject.GetComponent<AttachmentVectors>().UpdatePosition(currentObject.transform.position);
            currentObject.GetComponent<AttachmentVectors>().DrawVectors(false);

            placingObject = false;
            objectCount++;
            currentObject = null;
        }
    }
}