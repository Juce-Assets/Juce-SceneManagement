using UnityEngine.SceneManagement;

namespace Juce.SceneManagement.Loader
{
    public interface ISceneLoadResult
    {
        bool Success { get; }
        Scene Scene { get; }
    }
}
