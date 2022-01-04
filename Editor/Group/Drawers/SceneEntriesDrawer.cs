using Juce.SceneManagement.Group.Data;
using Juce.SceneManagement.Group.Extensions;
using Juce.SceneManagement.Group.Helpers;
using Juce.SceneManagement.Group.Logic;
using Juce.SceneManagement.Loader;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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

                bool isValidScene = !string.IsNullOrEmpty(sceneName);

                if (!isValidScene)
                {
                    sceneName = "Missing Scene!";
                }

                if(i != 0)
                {
                    EditorGUILayout.Space(4);
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

                    if (isValidScene)
                    {
                        if (GUILayout.Button("Open"))
                        {
                            EditorSceneLoader.TryOpen(entry.SceneReference.ScenePath, OpenSceneMode.Single, out Scene _);

                            // For some reason when loading a new scene, unity disposes the below SerializedProperty, 
                            // so we simply return to avoid a console error.
                            return; 
                        }
                    }

                    SerializedProperty serializedProperty = serializedPropertiesData.EntriesProperty.GetArrayElementAtIndex(i);

                    serializedProperty.ForeachVisibleChildren(DrawChildPropertyField);

                    SceneEntryCustomDrawersDrawer.Draw(toolData, entry);
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
