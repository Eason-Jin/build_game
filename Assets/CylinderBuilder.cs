using UnityEngine;

public class CylinderBuilder : ObjectBuilder
{
    protected override void PlaceObject()
    {
        base.PlaceObject();

        // Additional logic for aligning cylinders
        if (currentObject != null && currentObject.CompareTag("Cylinder"))
        {
            AlignCylinderToSurface();
        }
    }

    private void AlignCylinderToSurface()
    {
        // Custom alignment logic for cylinders
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, objectLayer))
        {
            Vector3 surfaceNormal = hit.normal;
            currentObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        }
    }
}