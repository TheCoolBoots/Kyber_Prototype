#if GAIA_PRESENT && UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using AQUAS;

namespace Gaia.GX.Dogmatic
{
    //<summary>
    //AQUAS setup for Gaia
    //</summary>
    public class AQUAS_Gaia : MonoBehaviour
    {
#region Generic informational methods
        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Dogmatic";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "AQUAS 2020";
        }
#endregion

        //<summary>
        //Gives some info on AQUAS
        //</summary>
        public static void GX_About()
        {
            EditorUtility.DisplayDialog("About AQUAS", "AQUAS Water is a feature rich water solution for all types of platforms, environments and games.", "OK");
        }

        //<summary>
        //Opens AQUAS's Setup Wizard
        //</summary>
        public static void GX_OpenSetupWizard()
        {
            //Get scene info
            GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();

            //Find camera
            Camera camera = Camera.main;

            if (camera == null)
            {
                camera = FindObjectOfType<Camera>();
            }

            float waterLevel = sceneInfo.m_seaLevel;

            AQUAS_SetupWizard setup = (AQUAS_SetupWizard)EditorWindow.GetWindow(typeof(AQUAS_SetupWizard), false);

            setup.camera = camera.transform.gameObject;
            setup.waterLevel = waterLevel;

            setup.Show();
        }
    }
}
#endif
