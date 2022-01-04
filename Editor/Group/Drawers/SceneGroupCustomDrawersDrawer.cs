using Juce.SceneManagement.Group.CustomDrawers;
using Juce.SceneManagement.Group.Data;

namespace Juce.SceneManagement.Group.Drawers
{
    public static class SceneGroupCustomDrawersDrawer
    {
        public static void Draw(ToolData toolData, SceneGroup sceneGroup)
        {
            foreach (ISceneGroupCustomDrawer customDrawer in toolData.SceneGroupCustomDrawer)
            {
                customDrawer.OnInspectorGUI(sceneGroup);
            }
        }
    }
}
