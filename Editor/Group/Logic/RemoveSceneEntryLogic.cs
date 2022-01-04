using Juce.SceneManagement.Group.Data;

namespace Juce.SceneManagement.Group.Logic
{
    public static class RemoveSceneEntryLogic
    {
        public static void Execute(
            ToolData toolData,
            SceneGroupEntry sceneGroupEntry
            )
        {
            toolData.EntriesToRemove.Add(sceneGroupEntry);
        }
    }
}
