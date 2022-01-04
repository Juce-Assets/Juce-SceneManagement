using Juce.SceneManagement.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Juce.SceneManagement.Loader
{
    public static class RuntimeSceneLoader
    {
        public static async Task<SceneLoadResult> Load(string scenePath, LoadSceneMode mode)
        {
            TaskCompletionSource<SceneLoadResult> taskCompletionSource = new TaskCompletionSource<SceneLoadResult>();

            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);

            if (asyncLoad == null)
            {
                return new SceneLoadResult();
            }

            asyncLoad.completed += (UnityEngine.AsyncOperation operation) =>
            {
                Scene loadedScene = SceneManager.GetSceneByName(sceneName);

                if (!loadedScene.IsValid())
                {
                    UnityEngine.Debug.LogError($"There was an error loading scene: {scenePath}. " +
                        $"Loaded scene is not valid at {nameof(RuntimeSceneLoader)}");
                }

                taskCompletionSource.SetResult(new SceneLoadResult(loadedScene));
            };

            SceneLoadResult result = await taskCompletionSource.Task;

            await Task.Yield();

            return result;
        }

        public static Task<bool> Unload(string scenePath)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            Scene loadedScene = SceneManager.GetSceneByPath(scenePath);

            if (!loadedScene.IsValid())
            {
                // Is already unloaded or wrong path.
                // We only need to log errors at loading.
                return Task.FromResult(true);
            }

            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(loadedScene);

            if (asyncUnload == null)
            {
                return Task.FromResult(false);
            }

            asyncUnload.completed += (UnityEngine.AsyncOperation operation) =>
            {
                taskCompletionSource.SetResult(true);
            };

            return taskCompletionSource.Task;
        }

        public static async Task<List<SceneLoadResult>> Load(ISceneCollection sceneCollection, LoadSceneMode mode)
        {
            List<SceneLoadResult> ret = new List<SceneLoadResult>();

            bool first = true;

            foreach (ISceneCollectionEntry sceneEntry in sceneCollection.SceneEntries)
            {
                LoadSceneMode actualMode = LoadSceneMode.Additive;

                if (first)
                {
                    first = false;

                    actualMode = mode;
                }

                SceneLoadResult result = await Load(sceneEntry.ScenePath, LoadSceneMode.Additive);

                bool shouldBeSetToActive = sceneEntry.LoadAsActive && result.Success;

                if (shouldBeSetToActive)
                {
                    SceneManager.SetActiveScene(result.Scene);
                }

                ret.Add(result);
            }

            return ret;
        }

        public static async Task<List<bool>> Unload(ISceneCollection sceneCollection)
        {
            List<bool> ret = new List<bool>();

            foreach (ISceneCollectionEntry sceneEntry in sceneCollection.SceneEntries)
            {
                bool result = await Unload(sceneEntry.ScenePath);

                ret.Add(result);
            }

            return ret;
        }
    }
}
