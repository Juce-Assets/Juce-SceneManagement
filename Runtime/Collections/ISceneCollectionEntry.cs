namespace Juce.SceneManagement.Collections
{
    public interface ISceneCollectionEntry
    {
        string ScenePath { get; }
        bool LoadAsActive { get; }
    }
}
