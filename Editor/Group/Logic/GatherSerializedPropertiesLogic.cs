using Juce.SceneManagement.Group;
using Juce.SceneManagement.Group.Data;

namespace Juce.SceneManagement.Group.Logic
{
    public static class GatherSerializedPropertiesLogic
    {
        public static void Execute(
            SceneGroupEditor sceneGroupEditor,
            SerializedPropertiesData serializedPropertiesData
            )
        {
            serializedPropertiesData.EntriesProperty = sceneGroupEditor.serializedObject.FindProperty("Entries");
        }
    }
}
