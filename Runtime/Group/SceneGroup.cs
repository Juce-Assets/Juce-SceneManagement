using Juce.SceneManagement.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juce.SceneManagement.Group
{
    [CreateAssetMenu(fileName = "SceneGroup", menuName = "Juce/SceneManagement/SceneGroup", order = 1)]
    public class SceneGroup : ScriptableObject
    {
        public List<SceneGroupEntry> Entries = default;

        public ISceneCollection SceneCollection => GenerateCollection();

        private ISceneCollection GenerateCollection()
        {
            List<ISceneCollectionEntry> sceneEntries = new List<ISceneCollectionEntry>();

            foreach(SceneGroupEntry entry in Entries)
            {
                if(entry.SceneReference == null)
                {
                    UnityEngine.Debug.LogError($"Missing scene detected at {nameof(SceneGroup)} {name}");
                    continue;
                }

                if(string.IsNullOrEmpty(entry.SceneReference.ScenePath))
                {
                    UnityEngine.Debug.LogError($"Missing scene detected at {nameof(SceneGroup)} {name}");
                    continue;
                }

                SceneCollectionEntry sceneEntry = new SceneCollectionEntry(
                    entry.SceneReference.ScenePath,
                    entry.LoadAsActive
                    );

                sceneEntries.Add(sceneEntry);
            }

            return new SceneCollection(sceneEntries);
        }
    }
}
