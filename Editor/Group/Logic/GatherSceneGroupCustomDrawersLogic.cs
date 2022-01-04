using Assets.Juce_SceneManagement.Editor.Group.Utils;
using Juce.SceneManagement.Group.CustomDrawers;
using Juce.SceneManagement.Group.Data;
using System;
using System.Collections.Generic;

namespace Juce.SceneManagement.Group.Logic
{
    public static class GatherSceneGroupCustomDrawersLogic
    {
        public static void Execute(ToolData toolData)
        {
            toolData.SceneGroupCustomDrawer.Clear();

            List<Type> types = ReflectionUtils.GetInheritedTypes(typeof(ISceneGroupCustomDrawer));

            foreach (Type type in types)
            {
                bool hasDefaultConstructor = type.GetConstructor(Type.EmptyTypes) != null;

                if (!hasDefaultConstructor)
                {
                    continue;
                }

                ISceneGroupCustomDrawer customDrawer = Activator.CreateInstance(type) as ISceneGroupCustomDrawer;

                toolData.SceneGroupCustomDrawer.Add(customDrawer);
            }
        }
    }
}
