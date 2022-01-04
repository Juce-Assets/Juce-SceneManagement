using Juce.SceneManagement.Group;
using Juce.SceneManagement.Loader;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Example1 : MonoBehaviour
{
    [SerializeField] private SceneGroup sceneGroup = default;

    private void Start()
    {
        RuntimeSceneLoader.Load(sceneGroup.SceneCollection, LoadSceneMode.Single).ContinueWith(
            (Task result) => { UnityEngine.Debug.Log("Scenes loaded successfully"); },
            TaskScheduler.FromCurrentSynchronizationContext()
            );
    }
}
