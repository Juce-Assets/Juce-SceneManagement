using Juce.SceneManagement.Group.Data;
using Juce.SceneManagement.Group.Logic;
using UnityEditor;
using UnityEngine;

namespace Juce.SceneManagement.Group.Drawers
{
    public static class EntryContextMenuDrawer
    {
        public static void Draw(
            ToolData toolData,
            SceneGroupEntry sceneGroupEntry
            )
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Remove"), false,
                () =>
                {
                    RemoveSceneEntryLogic.Execute(toolData, sceneGroupEntry);
                });

            menu.ShowAsContext();
        }

    }
}
