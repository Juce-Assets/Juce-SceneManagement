using System.Collections.Generic;

namespace Juce.SceneManagement.Group.Data
{
    public class ToolData
    {
        public List<SceneGroupEntry> EntriesToRemove { get; } = new List<SceneGroupEntry>();
        public Dictionary<SceneGroupEntry, bool> LastUpdateSceneEntryLoadAsActiveMap { get; } = new Dictionary<SceneGroupEntry, bool>();
    }
}
