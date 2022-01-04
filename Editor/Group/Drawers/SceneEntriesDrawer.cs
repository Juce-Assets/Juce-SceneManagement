using Juce.SceneManagement.Group.Data;
using Juce.SceneManagement.Group.Extensions;
using Juce.SceneManagement.Group.Helpers;
using Juce.SceneManagement.Group.Logic;
using Juce.SceneManagement.Group.Style;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Juce.SceneManagement.Group.Drawers
{
    public static class SceneEntriesDrawer
    {
        public static void Draw(
            SceneGroup sceneGroup,
            ToolData toolData,
            SerializedPropertiesData serializedPropertiesData,
            ReorderableHelper reorderableHelper
            )
        {
            for (int i = 0; i < sceneGroup.Entries.Count; ++i)
            {
                SceneGroupEntry entry = sceneGroup.Entries[i];

                string sceneName = Path.GetFileNameWithoutExtension(entry.SceneReference);

                if(string.IsNullOrEmpty(sceneName))
                {
                    sceneName = "Missing Scene";
                }

                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    ComponentHeaderDrawer.Draw(
                        sceneName,
                        string.Empty,
                        () => EntryContextMenuDrawer.Draw(toolData, entry),
                        out Rect reorderInteractionRect,
                        out Rect secondaryInteractionRect
                        );

                    reorderableHelper.CheckDraggingItem(
                        i,
                        Event.current,
                        reorderInteractionRect,
                        secondaryInteractionRect
                        );

                    SerializedProperty serializedProperty = serializedPropertiesData.EntriesProperty.GetArrayElementAtIndex(i);

                    serializedProperty.ForeachVisibleChildren(DrawChildPropertyField);
                }
            }

            // Finish dragging
            int startIndex;
            int endIndex;
            bool dragged = reorderableHelper.ResolveDragging(Event.current, out startIndex, out endIndex);

            if (dragged)
            {
                ReorderSceneEntriesLogic.Execute(sceneGroup, startIndex, endIndex);

                EditorUtility.SetDirty(sceneGroup);
            }
        }

        private static void DrawChildPropertyField(SerializedProperty childProperty)
        {
            EditorGUILayout.PropertyField(
                childProperty,
                includeChildren: true
                );
        }
    }
}
