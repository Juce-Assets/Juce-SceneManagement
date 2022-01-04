using Juce.SceneManagement.Group.Logic;
using UnityEngine;

namespace Juce.SceneManagement.Group.Drawers
{
    public static class AddEntriesDrawer
    {
        public static void Draw(
            SceneGroup sceneGroup
            )
        {
            if (GUILayout.Button("Add"))
            {
                AddSceneEntryLogic.Execute(sceneGroup);
            }
        }
    }
}
