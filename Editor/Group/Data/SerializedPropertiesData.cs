using UnityEditor;

namespace Juce.SceneManagement.Group.Data
{
    public class SerializedPropertiesData
    {
        private readonly SerializedObject serializedObject;

        public SerializedPropertiesData(
            SerializedObject serializedObject
            )
        {
            this.serializedObject = serializedObject;
        }

        public SerializedProperty EntriesProperty { get => serializedObject.FindProperty("Entries"); }
    }
}
