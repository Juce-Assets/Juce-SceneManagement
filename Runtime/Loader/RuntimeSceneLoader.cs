﻿using Juce.SceneManagement.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Juce.SceneManagement.Loader
{
    public static class RuntimeSceneLoader
    {
        public static Task<SceneLoadResult> LoadFromPath(string scenePath, LoadSceneMode mode)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            TaskCompletionSource<SceneLoadResult> taskCompletionSource = new TaskCompletionSource<SceneLoadResult>();

            return LoadFromName(sceneName, mode);
        }

        public static async Task<SceneLoadResult> LoadFromName(string sceneName, LoadSceneMode mode)
        {
            TaskCompletionSource<SceneLoadResult> taskCompletionSource = new TaskCompletionSource<SceneLoadResult>();

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
                    UnityEngine.Debug.LogError($"There was an error loading scene: {sceneName}. " +
                        $"Loaded scene is not valid at {nameof(RuntimeSceneLoader)}");
                }

                taskCompletionSource.SetResult(new SceneLoadResult(loadedScene));
            };

            SceneLoadResult result = await taskCompletionSource.Task;

            await Task.Yield();

            return result;
        }

        public static Task<bool> UnloadFromPath(string scenePath)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            return UnloadFromName(sceneName);
        }

        public static Task<bool> UnloadFromName(string sceneName)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            Scene loadedScene = SceneManager.GetSceneByName(sceneName);

            if (!loadedScene.IsValid())
            {
                // Is already unloaded or wrong name.
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

                SceneLoadResult result = await LoadFromPath(sceneEntry.ScenePath, LoadSceneMode.Additive);

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
                bool result = await UnloadFromPath(sceneEntry.ScenePath);

                ret.Add(result);
            }

            return ret;
        }
    }
}
