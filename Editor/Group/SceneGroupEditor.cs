﻿using Juce.SceneManagement.Loader;
using Juce.SceneManagement.Group.Logic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Juce.SceneManagement.Group.Data;
using Juce.SceneManagement.Group.Drawers;
using Juce.SceneManagement.Group.Helpers;
using UnityEditor.SceneManagement;

namespace Juce.SceneManagement.Group
{
    [CustomEditor(typeof(SceneGroup))]
    public class SceneGroupEditor : Editor
    {
        private readonly ToolData toolData = new ToolData();
        private readonly SerializedPropertiesData serializedPropertiesData = new SerializedPropertiesData();

        private readonly ReorderableHelper reorderableHelper = new ReorderableHelper();

        private SceneGroup ActualTarget { get; set; }

        private void OnEnable()
        {
            ActualTarget = (SceneGroup)target;

            GatherSerializedPropertiesLogic.Execute(
                this,
                serializedPropertiesData
                );
        }

        public override void OnInspectorGUI()
        {
            LoadAsActiveChangeCheckLogic.Execute(toolData, ActualTarget);

            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            SceneEntriesDrawer.Draw(
                ActualTarget, 
                toolData,
                serializedPropertiesData, 
                reorderableHelper
                );

            EditorGUILayout.Space(2);

            AddEntriesDrawer.Draw(ActualTarget);

            EditorGUILayout.Space(2);

            OpenCloseDrawer.Draw(ActualTarget);

            if (Event.current.type != EventType.Layout)
            {
                ActuallyRemoveSceneEntriesLogic.Execute(toolData, ActualTarget);
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ActualTarget);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
