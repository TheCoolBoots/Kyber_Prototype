using UnityEditor;

namespace AQUAS
{

    //Removes AQUAS's define symbolf when AQUAS is removed from the project
    public class AQUAS_RemoveDefine : UnityEditor.AssetModificationProcessor
    {

        static string symbols;

        public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions rao)
        {

            if (assetPath.Contains("AQUAS_2020"))
            {
                symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

                if (symbols.Contains("AQUAS_2020_PRESENT"))
                {
                    symbols = symbols.Replace("AQUAS_2020_PRESENT;", "");
                    symbols = symbols.Replace("AQUAS_2020_PRESENT", "");
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
                }
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}
