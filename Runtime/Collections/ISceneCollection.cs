using System.Collections.Generic;
using System.Threading.Tasks;

namespace Juce.SceneManagement.Collections
{
    public interface ISceneCollection
    {
        IReadOnlyList<ISceneCollectionEntry> SceneEntries { get; }
    }
}
