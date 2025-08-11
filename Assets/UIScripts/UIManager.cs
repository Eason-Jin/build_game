namespace UIScripts
{
    using BuildingScripts;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIManager : MonoBehaviour
    {
        public Transform objectListParent;
        public Button playButton;

        private Dictionary<string, ObjectBuilder> builders = new Dictionary<string, ObjectBuilder>();
        private bool isPlaying = false;
        private TextMeshProUGUI playButtonText;

        void Start()
        {
            Image objectListBackground = objectListParent.GetComponent<Image>();
            if (objectListBackground == null)
            {
                objectListBackground = objectListParent.gameObject.AddComponent<Image>();
            }
            objectListBackground.color = new Color(0.9f, 0.9f, 0.9f, 0.5f);

            ObjectBuilder[] builderInstances = FindObjectsOfType<ObjectBuilder>();
            foreach (var builder in builderInstances)
            {
                string tagWithoutPlaced = builder.Tag.Replace("Placed", "  ");
                builders[tagWithoutPlaced] = builder;

                GameObject buttonObj = new GameObject(tagWithoutPlaced, typeof(RectTransform), typeof(Button), typeof(Image));
                buttonObj.transform.SetParent(objectListParent, false);

                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() => SwitchBuilder(builder));

                Image buttonBackground = buttonObj.GetComponent<Image>();
                buttonBackground.color = new Color(0.9f, 0.9f, 0.9f, 0.75f);

                GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
                textObj.transform.SetParent(buttonObj.transform, false);

                TextMeshProUGUI buttonText = textObj.GetComponent<TextMeshProUGUI>();
                buttonText.text = tagWithoutPlaced;
                buttonText.alignment = TextAlignmentOptions.Left;
                buttonText.color = Color.black;
                buttonText.fontSize = 14;

                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.offsetMin = Vector2.zero;
                textRect.offsetMax = Vector2.zero;

                RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
                buttonRect.localScale = Vector3.one;
            }

            playButton.onClick.AddListener(ChangePlayState);

            playButtonText = playButton.GetComponentInChildren<TextMeshProUGUI>();
            playButtonText.text = "Play";
        }

        private void SwitchBuilder(ObjectBuilder builder)
        {
            if (ObjectBuilder.activeBuilder != null)
                ObjectBuilder.activeBuilder.CancelPlacingObject();

            ObjectBuilder.activeBuilder = builder;
            builder.StartplacingObject();
        }

        private void ChangePlayState()
        {
            isPlaying = !isPlaying;
            if (isPlaying)
            {
                StartPlaying();
                playButtonText.text = "Cancel";
                objectListParent.gameObject.SetActive(false);
            }
            else
            {
                //ReturnToBuilder();
                playButtonText.text = "Play";
                objectListParent.gameObject.SetActive(true);
            }

        }


        private void StartPlaying()
        {
            ObjectBuilder[] builders = FindObjectsOfType<ObjectBuilder>();
            foreach (ObjectBuilder builder in builders)
            {
                builder.EnablePhysics();
            }
        }
    }
}
