using System.Collections.Generic;
using UnityEngine;

public class AttachmentVectors : MonoBehaviour
{
    private List<Vector3> directions = new List<Vector3>();
    private Dictionary<Vector3, LineRenderer> directionLineRenderers = new Dictionary<Vector3, LineRenderer>();
    private Dictionary<Vector3, Color> directionColorMap = new Dictionary<Vector3, Color>
    {
        { Vector3.up, Color.green },
        { Vector3.down, Color.red },
        { Vector3.left, Color.blue },
        { Vector3.right, Color.yellow },
        { Vector3.forward, Color.cyan },
        { Vector3.back, Color.magenta }
    };

    public void Initialise(GameObject currentGameObject, int[] attachmentVectors)
    {
        // Clear existing directions
        directions.Clear();

        // Map attachmentVectors to directions
        if (attachmentVectors.Length == 6)
        {
            if (attachmentVectors[0] > 0) directions.Add(Vector3.up * attachmentVectors[0]);
            if (attachmentVectors[1] > 0) directions.Add(Vector3.down * attachmentVectors[1]);
            if (attachmentVectors[2] > 0) directions.Add(Vector3.left * attachmentVectors[2]);
            if (attachmentVectors[3] > 0) directions.Add(Vector3.right * attachmentVectors[3]);
            if (attachmentVectors[4] > 0) directions.Add(Vector3.forward * attachmentVectors[4]);
            if (attachmentVectors[5] > 0) directions.Add(Vector3.back * attachmentVectors[5]);
        }

        // Create LineRenderers for each direction
        foreach (var direction in directions)
        {
            if (directionColorMap.TryGetValue(direction.normalized, out Color color))
            {
                GameObject lineObject = new GameObject("LineRenderer_" + direction);
                lineObject.transform.SetParent(currentGameObject.transform); // Attach to parent
                LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;

                // Store the LineRenderer for this direction
                directionLineRenderers[direction] = lineRenderer;
            }
        }
    }

    private void Update()
    {
        // Update the positions of the LineRenderers
        foreach (var direction in directions)
        {
            if (directionLineRenderers.TryGetValue(direction, out LineRenderer lineRenderer))
            {
                Vector3 startPoint = transform.position; // Parent object's position
                Vector3 endPoint = startPoint + transform.TransformDirection(direction);
                lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });
            }
        }
    }

    public void DrawVectors(bool on)
    {
        if (!on)
        {
            // Destroy all LineRenderers
            foreach (var lineRenderer in directionLineRenderers.Values)
            {
                if (lineRenderer != null)
                {
                    lineRenderer.startWidth = 0.0f;
                    lineRenderer.endWidth = 0.0f;
                }
            }
        }
    }
}
