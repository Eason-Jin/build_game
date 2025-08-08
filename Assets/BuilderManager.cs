//using UnityEngine;

//public class BuilderManager : MonoBehaviour
//{
//    public CubeBuilder cubeBuilder;         // Reference to the CubeBuilder
//    public CylinderBuilder cylinderBuilder; // Reference to the CylinderBuilder

//    private ObjectBuilder activeBuilder;    // The currently active builder

//    void Start()
//    {
        
//    }

//    void Update()
//    {
//        // Switch to CubeBuilder when X is pressed
//        if (Input.GetKeyDown(KeyCode.X))
//        {
//            SetActiveBuilder(cubeBuilder);
//        }

//        // Switch to CylinderBuilder when C is pressed
//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            SetActiveBuilder(cylinderBuilder);
//        }

//        // Esc to stop placing objects
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            SetActiveBuilder(null);
//        }

//        // Delegate Update logic to the active builder
//        if (activeBuilder != null)
//        {
//            activeBuilder.enabled = true;
//            activeBuilder.StartPlacingObject();
//        }
//    }

//    private void SetActiveBuilder(ObjectBuilder builder)
//    {
//        // Disable the current active builder
//        if (activeBuilder != null)
//        {
//            activeBuilder.enabled = false;
//        }

//        // Set the new active builder
//        activeBuilder = builder;

//        // Enable the new active builder
//        if (activeBuilder != null)
//        {
//            activeBuilder.enabled = true;
//        }
//    }
//}