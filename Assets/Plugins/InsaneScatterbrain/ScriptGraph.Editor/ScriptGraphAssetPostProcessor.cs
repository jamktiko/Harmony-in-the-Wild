using System.Linq;
using System.Threading.Tasks;
using InsaneScatterbrain.ScriptGraph;
using InsaneScatterbrain.ScriptGraph.Editor;
using UnityEditor;

namespace InsaneScatterbrain.MapGraph.Editor
{
    internal class ScriptGraphAssetPostProcessor : AssetPostprocessor
    {
#if UNITY_2021_2_OR_NEWER
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
#else
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
#endif
        {
            // Windows need to be reloaded if any of the graphs has been moved. 
            foreach (var movedAsset in movedAssets)
            {
                var graph = AssetDatabase.LoadAssetAtPath<ScriptGraphGraph>(movedAsset);

                if (graph == null) continue;

                ReloadWindows();
                break;
            }

            if (deletedAssets.Length > 0)
            {
                // A graph used as sub graph may have been deleted, reload the editor windows so it's immediately shown
                // where these nodes are now missing, without having to reload the editor windows by hand.
                var instances = ScriptGraphViewWindow.Instances.ToArray();
                foreach (var instance in instances)
                {
                    if (instance == null) continue;
                    
                    if (instance.GraphView == null || instance.GraphView.Graph == null)
                    {
                        // This instance no longer exists, it might be the asset that's just been deleted. Close the
                        // corresponding windows.
                        instance.Close();
                    }
                    instance.Reload();
                }
            }
            
#if UNITY_2021_2_OR_NEWER
            // In older versions of Unity OnPostprocessAllAssets is not called when a script is recompiled, so
            // it's done through the ScriptGraphInitializer.
            if (!didDomainReload)
                return;

            var updater = new ScriptGraphAssetUpdater();
            updater.Start();
#endif
        }
        
        private static async void ReloadWindows()
        {
            await Task.Delay(1);
            ScriptGraphViewWindow.ReloadAll();
        }
    }
}