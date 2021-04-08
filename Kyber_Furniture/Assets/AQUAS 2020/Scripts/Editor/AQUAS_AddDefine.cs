using UnityEditor;

namespace AQUAS
{

    //Adds a scripting define symbol for other assets to detect whether AQUAS is imported or not
	[InitializeOnLoad]
	public class AQUAS_AddDefine : Editor {
        
		static AQUAS_AddDefine()
		{

            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

			if (!symbols.Contains("AQUAS_2020_PRESENT"))
			{
				symbols += ";" + "AQUAS_2020_PRESENT";
				PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
			}
        }
	}
}
