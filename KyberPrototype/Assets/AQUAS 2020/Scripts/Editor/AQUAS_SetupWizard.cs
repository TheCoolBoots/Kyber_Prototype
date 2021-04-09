using UnityEngine;
using UnityEditor;

namespace AQUAS
{

    public enum TARGETPLATFORM
    {
        desktopWebAndConsole = 0,
        mobile = 1
    }

    public enum MOBILEPRESETS
    {
        lake = 0,
        swamp = 1,
        sewers = 2,
        poolsAndPonds = 3,
        rtsStyle2 = 4
    }

    public enum WATERTYPE
    {
        deepWater = 0,
        shallowWater = 1
    }

    public enum PRESETS
    {
        lake = 0,
        mountainLake = 1,
        swamp = 2,
        sewers = 3,
        calmOcean = 4,
        pondsAndPools = 5,
        rtsStyle = 6
    }

    public enum SHALLOWPRESETS
    {
        clear = 0,
        dirty = 1,
        muddy = 2,
        rusty = 3,
    }

    public enum UNDERWATERTYPE
    {
        Simple = 0,
        Advanced = 1
    }

    public enum CAUSTICSTYPE
    {
        noCaustics = 0,
        singleCaustics = 1,
        doubleCaustics = 2
    }

    public class AQUAS_SetupWizard : EditorWindow
    {
        public static AQUAS_SetupWizard window;

        public WATERTYPE waterType;
        public PRESETS presets;
        public SHALLOWPRESETS shallowPresets;
        public MOBILEPRESETS mobilePresets;
        public TARGETPLATFORM targetPlatform;
        public UNDERWATERTYPE underwaterType;
        public CAUSTICSTYPE causticsType = CAUSTICSTYPE.singleCaustics;

        [MenuItem("Window/AQUAS/Setup Wizard")]
        public static void OpenWindow()
        {
            window = (AQUAS_SetupWizard)EditorWindow.GetWindow(typeof(AQUAS_SetupWizard));
            window.titleContent.text = "AQUAS";
        }

        public float waterLevel = 0;
        public GameObject terrain = null;
        public GameObject camera = null;

        public GameObject waterplane;
        public float maxDepth;
        public bool useWaterLevelForMaxDepth;

        Vector3 waterPosition;

        public float maskDepth = 5;

        int tab = 0;
        int tabTools = 0;

        Vector2 scrollPosition;

        private void OnGUI()
        {
            EditorStyles.textField.wordWrap = true;

            if(window == null)
            {
                OpenWindow();
            }

            EditorGUILayout.BeginVertical();
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));


            GUILayout.Label("AQUAS Setup Wizard", EditorStyles.boldLabel);

            tab = GUILayout.Toolbar(tab, new string[] { "Water Setup", "Underwater Setup", "Tools" });

            if (tab == 0)
            {
                GUILayout.Label("Water Setup", EditorStyles.boldLabel);
                //targetPlatform = (TARGETPLATFORM)EditorGUILayout.EnumPopup("Select Target Platform", targetPlatform);

                waterType = (WATERTYPE)EditorGUILayout.EnumPopup("Select a type of Water", waterType);

                if (targetPlatform == TARGETPLATFORM.desktopWebAndConsole)
                {
                    if(waterType == WATERTYPE.deepWater)
                    {
                        presets = (PRESETS)EditorGUILayout.EnumPopup("Select a Preset", presets);
                    }
                    else
                    {
                        shallowPresets = (SHALLOWPRESETS)EditorGUILayout.EnumPopup("Select a Preset", shallowPresets);
                    }
                }

                else
                {
                    if (waterType == WATERTYPE.deepWater)
                    {
                        mobilePresets = (MOBILEPRESETS)EditorGUILayout.EnumPopup("Select a Preset", mobilePresets);
                    }
                    else
                    {

                    }
                }

                waterLevel = EditorGUILayout.FloatField("Water Level", waterLevel);

                terrain = (GameObject)EditorGUILayout.ObjectField("Terrain", terrain, typeof(GameObject), true);

                if (terrain == null)
                {
                    EditorGUILayout.HelpBox("If no terrain is provided, water will be placed at the center of the scene without scaling.", MessageType.Info);
                }

                camera = (GameObject)EditorGUILayout.ObjectField("Camera", camera, typeof(GameObject), true);

                causticsType = (CAUSTICSTYPE)EditorGUILayout.EnumPopup("Select the caustics type", causticsType);
                
                if(causticsType == CAUSTICSTYPE.doubleCaustics)
                {
                    EditorGUILayout.HelpBox("Note: Double caustics may be expensive on performance!", MessageType.Info);
                }

                if (GUILayout.Button("About"))
                {
                    About();
                }

                if (GUILayout.Button("Add Water"))
                {
                    AddWater();
                }
            }

            if (tab == 1)
            {
                GUILayout.Label("Underwater Setup", EditorStyles.boldLabel);

                underwaterType = (UNDERWATERTYPE)EditorGUILayout.EnumPopup("Select type of underwater effects", underwaterType);

                switch(underwaterType)
                {
                    case UNDERWATERTYPE.Simple:
                        EditorGUILayout.HelpBox("Simple underwater effects offer a lot more performance but offer less features (no split rendering, no screenspace fog).", MessageType.Info);
                        break;

                    case UNDERWATERTYPE.Advanced:
                        EditorGUILayout.HelpBox("Advanced underwater effects offer split rendering and screen space fog but are much heavier on performance than simple underwater effects. \n \n Attention: \n Advanced underwater effects do not work on Android and are not recommended for Mobile devices!", MessageType.Info);
                        break;
                }

                camera = (GameObject)EditorGUILayout.ObjectField("Camera", camera, typeof(GameObject), true);

                if (GUILayout.Button("About"))
                {
                    AboutAddUnderwaterEffects();
                }

                if(GUILayout.Button("Add Underwater Effects"))
                {
                    AddUnderwaterEffects();
                }
            }

            if (tab == 2)
            {
                GUILayout.Label("Tools", EditorStyles.boldLabel);
                tabTools = GUILayout.Toolbar(tabTools, new string[] { "River Reference Tool", "Terrain Masking Tool" });

                if (tabTools == 0)
                {
                    GUILayout.Label("River Reference Export", EditorStyles.boldLabel);
                    waterplane = (GameObject)EditorGUILayout.ObjectField("Water Plane", waterplane, typeof(GameObject), true);

                    if (GUILayout.Button("About"))
                    {
                        AboutExportRiverReference();
                    }

                    if (GUILayout.Button("Create river reference image"))
                    {
                        CreateRiverReference();
                    }
                }

                if (tabTools == 1)
                {
                    GUILayout.Label("Terrain Mask Export", EditorStyles.boldLabel);
                    waterplane = (GameObject)EditorGUILayout.ObjectField("Water Plane", waterplane, typeof(GameObject), true);
                    terrain = (GameObject)EditorGUILayout.ObjectField("Terrain", terrain, typeof(GameObject), true);
                    maskDepth = EditorGUILayout.FloatField("Maximum Mask Depth", maskDepth);

                    if (GUILayout.Button("About"))
                    {
                        AboutExportTerrainMask();
                    }

                    if (GUILayout.Button("Create Terrain Mask"))
                    {
                        CreateTerrainMask();
                    }
                }

                

            }


            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            
        }

        void Update()
        {
            Repaint();
        }

        //<summary>
        //Gives some info on the quick setup feature
        //</summary>
        void About()
        {
            EditorUtility.DisplayDialog("AQUAS Setup Wizard", "The setup wizard will help you with the basic setup of AQUAS. You can add water and underwater effects. Please keep in mind that the wizard only provides a basic setup. More advanced setups (e.g. multiple water levels) have to be done manually.", "Got It");
        }

        void AboutAddUnderwaterEffects()
        {
            EditorUtility.DisplayDialog("AQUAS Underwater Setup", "The underwater setup will automatically set up your camera for underwater effects. Underwater effects are added per water plane, since not all water planes require underwater effects to work.", "Got It");
        }

        void AboutExportRiverReference()
        {
            EditorUtility.DisplayDialog("AQUAS River Reference Export", "The river reference tool allows you to export a reference image to use as a template for painting a flow map that matches your water. The reference image will be saved in the 'AQUAS 2020/RiverReferences' folder", "Got it");
        }

        void AboutExportTerrainMask()
        {
            EditorUtility.DisplayDialog("AQUAS Terrain Mask Export", "The terrain mask exporter allows you to export a shoreline mask of the specified terrain to use for the material to simulate shoreline effects. The terrain mask will be saved in the 'AQUAS 2020/TerrainMasks' folder", "Got it");
        }

        void AddWater()
        {
            if(GameObject.Find("AQUAS Container") == null)
            {
                EditorUtility.DisplayDialog("No Container Object Found", "AQUAS could not find a container object for the water planes - trying to add one...", "OK");
                GameObject aquasContainer = new GameObject("AQUAS Container");
                aquasContainer.AddComponent<AQUAS_Container>();

                if(GameObject.Find("AQUAS Container") == null)
                {
                    Debug.LogError("Adding AQUAS Container failed!");
                    return;
                }
                else
                {
                    Debug.Log("AQUAS Container added!");
                }
            }

            Vector3 position;
            Vector3 scale;

            if (terrain == null)
            {
                if(EditorUtility.DisplayDialog("No Terrain Specified!", "You have not specified a terrain. AQUAS will add water at the center of the scene without scaling. Do you wish to continue?", "Add Anyway", "Cancel"))
                {
                    position = new Vector3(0, waterLevel, 0);
                    scale = new Vector3(1, 1, 1);
                }
                else
                {
                    return;
                }
            }
            else
            {
                float terrainX = terrain.GetComponent<Terrain>().terrainData.size.x;
                float terrainZ = terrain.GetComponent<Terrain>().terrainData.size.z;

                position = new Vector3(terrain.transform.position.x + terrainX / 2, waterLevel, terrain.transform.position.z + terrainZ / 2);
                scale = new Vector3(terrainX / 10, 1, terrainZ / 10);
            }

            GameObject aquasObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            
            aquasObj.name = "AQUAS Waterplane " + GameObject.Find("AQUAS Container").transform.childCount;
            aquasObj.transform.parent = GameObject.Find("AQUAS Container").transform;
            aquasObj.transform.position = position;
            aquasObj.transform.localScale = scale;

            aquasObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            Material mat;

            if (waterType == WATERTYPE.deepWater)
            {
                mat = new Material(Shader.Find("AQUAS/Desktop/Front/Default"));

                switch (presets)
                {
                    case PRESETS.lake:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/AQUAS_Lake.mat", typeof(Material)));
                        break;

                    case PRESETS.mountainLake:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/AQUAS_Mountain Lake.mat", typeof(Material)));
                        break;

                    case PRESETS.swamp:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/AQUAS Swamp.mat", typeof(Material)));
                        break;

                    case PRESETS.calmOcean:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/AQUAS_Calm Ocean.mat", typeof(Material)));
                        break;

                    case PRESETS.sewers:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/AQUAS_Sewer.mat", typeof(Material)));
                        break;

                    case PRESETS.pondsAndPools:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/AQUAS_Pools.mat", typeof(Material)));
                        break;

                    case PRESETS.rtsStyle:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/AQUAS_RTS Style.mat", typeof(Material)));
                        break;
                }
            }
            else
            {
                mat = new Material(Shader.Find("AQUAS/Desktop/Front/Shallow"));

                switch (shallowPresets)
                {
                    case SHALLOWPRESETS.clear:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/Shallow Water/AQUAS_Clear.mat", typeof(Material)));
                        break;

                    case SHALLOWPRESETS.dirty:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/Shallow Water/AQUAS_Dirty.mat", typeof(Material)));
                        break;

                    case SHALLOWPRESETS.muddy:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/Shallow Water/AQUAS_Muddy.mat", typeof(Material)));
                        break;

                    case SHALLOWPRESETS.rusty:
                        mat.CopyPropertiesFromMaterial((Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/Water/Presets/Shallow Water/AQUAS_Rusty.mat", typeof(Material)));
                        break;
                }
            }
            
            mat.name = aquasObj.name + " Material";

            

            

            aquasObj.GetComponent<MeshRenderer>().sharedMaterial = mat;

            DestroyImmediate(aquasObj.GetComponent<MeshCollider>());

            aquasObj.AddComponent<AQUAS_Reflection>();

            if (camera == null)
            {
                camera = Camera.main.transform.gameObject;
            }

            if(camera.GetComponent<AQUAS_Camera>() == null)
            {
                camera.AddComponent<AQUAS_Camera>();
            }

            if (causticsType == CAUSTICSTYPE.singleCaustics)
            {
                GameObject primaryCausticsPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Prefabs/PrimaryCausticsProjector.prefab", typeof(GameObject));
                GameObject primaryCausticsObj = Instantiate(primaryCausticsPrefab);
                primaryCausticsObj.name = "PrimaryCausticsProjector";

                primaryCausticsObj.transform.parent = aquasObj.transform;
                primaryCausticsObj.transform.localPosition = new Vector3(0, 0, 0);
                primaryCausticsObj.GetComponent<Projector>().orthographicSize = aquasObj.transform.localScale.z * 5;

                Material primaryCausticsMaterial = new Material(Shader.Find("AQUAS/Misc/Caustics"));
                primaryCausticsMaterial.SetFloat("_CausticsScale", 6);
                primaryCausticsMaterial.SetFloat("_Intensity", 5);
                primaryCausticsMaterial.SetFloat("_DistanceVisibility", 10);
                primaryCausticsMaterial.SetFloat("_Fade", 0.5f);
                primaryCausticsObj.GetComponent<Projector>().material = primaryCausticsMaterial;
            }
            else if (causticsType == CAUSTICSTYPE.doubleCaustics)
            {
                GameObject primaryCausticsPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Prefabs/PrimaryCausticsProjector.prefab", typeof(GameObject));
                GameObject primaryCausticsObj = Instantiate(primaryCausticsPrefab);
                primaryCausticsObj.name = "PrimaryCausticsProjector";

                primaryCausticsObj.transform.parent = aquasObj.transform;
                primaryCausticsObj.transform.localPosition = new Vector3(0, 0, 0);
                primaryCausticsObj.GetComponent<Projector>().orthographicSize = aquasObj.transform.localScale.z * 5;

                Material primaryCausticsMaterial = new Material(Shader.Find("AQUAS/Misc/Caustics"));
                primaryCausticsMaterial.SetFloat("_CausticsScale", 6);
                primaryCausticsMaterial.SetFloat("_Intensity", 2);
                primaryCausticsMaterial.SetFloat("_DistanceVisibility", 10);
                primaryCausticsMaterial.SetFloat("_Fade", 0.5f);
                primaryCausticsObj.GetComponent<Projector>().material = primaryCausticsMaterial;

                GameObject secondaryCausticsPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Prefabs/SecondaryCausticsProjector.prefab", typeof(GameObject));
                GameObject secondaryCausticsObj = Instantiate(secondaryCausticsPrefab);
                secondaryCausticsObj.name = "SecondaryCausticsProjector";

                secondaryCausticsObj.transform.parent = aquasObj.transform;
                secondaryCausticsObj.transform.localPosition = new Vector3(0, 0, 0);
                secondaryCausticsObj.GetComponent<Projector>().orthographicSize = aquasObj.transform.localScale.z * 5;
                
                Material secondaryCausticsMaterial = new Material(Shader.Find("AQUAS/Misc/Caustics"));
                secondaryCausticsMaterial.SetFloat("_CausticsScale", 6);
                secondaryCausticsMaterial.SetFloat("_Intensity", 2);
                secondaryCausticsMaterial.SetFloat("_DistanceVisibility", 10);
                secondaryCausticsMaterial.SetFloat("_Fade", 0.5f);
                secondaryCausticsObj.GetComponent<Projector>().material = secondaryCausticsMaterial;
            }
            else
            {

            }

            GameObject refProbe = new GameObject("Reflection Probe");
            refProbe.transform.parent = aquasObj.transform;
            refProbe.AddComponent<ReflectionProbe>();
        }

        void AddUnderwaterEffects()
        {
            GameObject aquasContainer = GameObject.Find("AQUAS Container");
                   
            for(int i = 0; i < aquasContainer.transform.childCount; i++)
            {
                waterplane = aquasContainer.transform.Find("AQUAS Waterplane " + i.ToString()).gameObject;

                maxDepth = waterplane.transform.position.y + 0.1f;

                if (waterplane.transform.Find("Static Boundary") != null)
                {
                    if (EditorUtility.DisplayDialog("Warning!", "AQUAS Waterplane " + i.ToString() +" already has a static boundary. Do you want to replace the existing boundary?", "Replace", "Cancel"))
                    {
                        DestroyImmediate(waterplane.transform.Find("Static Boundary").gameObject);

                        GameObject staticBoundary = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Prefabs/horizonBox.prefab", typeof(GameObject)), waterplane.transform);
                        staticBoundary.transform.name = "Static Boundary";
                        staticBoundary.transform.localScale = new Vector3(10, maxDepth, 10);
                        staticBoundary.transform.localPosition = new Vector3(0, 0, 0);
                        staticBoundary.GetComponent<Renderer>().enabled = false;
                    }
                    else
                    {

                    }
                }
                else
                {
                    GameObject staticBoundary = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Prefabs/horizonBox.prefab", typeof(GameObject)), waterplane.transform);
                    staticBoundary.transform.name = "Static Boundary";
                    staticBoundary.transform.localScale = new Vector3(10, maxDepth, 10);
                    staticBoundary.transform.localPosition = new Vector3(0, 0, 0);
                    staticBoundary.GetComponent<Renderer>().enabled = false;
                }

                Material tempMat = waterplane.GetComponent<Renderer>().sharedMaterials[0];
                Material backfaceMat;

                if (underwaterType == UNDERWATERTYPE.Advanced)
                {
                    backfaceMat = new Material(Shader.Find("AQUAS/Desktop/Back/Default Back"));
                }
                else
                {
                    backfaceMat = new Material(Shader.Find("AQUAS/Desktop/Back/Transparent Back"));
                }

                backfaceMat.name = tempMat.name + " Backface";
                backfaceMat.CopyPropertiesFromMaterial(tempMat);
                backfaceMat.SetColor("_DeepWaterColor", waterplane.GetComponent<Renderer>().sharedMaterial.GetColor("_DeepWaterColor") / 2);

                waterplane.GetComponent<Renderer>().sharedMaterials = new Material[2] { tempMat, backfaceMat };
                
                if (waterplane.GetComponent<AQUAS_UnderwaterParameters>() == null)
                {
                    waterplane.AddComponent<AQUAS_UnderwaterParameters>();
                    waterplane.GetComponent<AQUAS_UnderwaterParameters>().mainFogColor = waterplane.GetComponent<Renderer>().sharedMaterials[1].GetColor("_DeepWaterColor");

                    Color deepFogColor = waterplane.GetComponent<Renderer>().sharedMaterials[1].GetColor("_DeepWaterColor");
                    deepFogColor = new Color(deepFogColor.r * 0.5f, deepFogColor.g * 0.5f, deepFogColor.b * 0.5f);

                    waterplane.GetComponent<AQUAS_UnderwaterParameters>().deepFogColor = deepFogColor;
                }
            }
                 
            

            if (camera == null)
            {
                camera = Camera.main.transform.gameObject;
            }

            switch (underwaterType)
            {
                case UNDERWATERTYPE.Advanced:

                    if (camera.GetComponent<AQUAS_UnderWaterEffect>() == null)
                    {
                        if(camera.GetComponent<AQUAS_UnderWaterEffect_Simple>() == null)
                        {
                            camera.AddComponent<AQUAS_UnderWaterEffect>();
                        }
                        else
                        {
                            if(EditorUtility.DisplayDialog("Wargning!", "You already have simple underwater effects enabled for your camera. Do you wish to replace them with advanced underwater effects?", "Replace", "Don't Replace"))
                            {
                                camera.AddComponent<AQUAS_UnderWaterEffect>();
                                DestroyImmediate(camera.GetComponent<AQUAS_UnderWaterEffect_Simple>());
                            }
                            else
                            {

                            }
                        }
                    }

                    break;

                case UNDERWATERTYPE.Simple:

                    if (camera.GetComponent<AQUAS_UnderWaterEffect_Simple>() == null)
                    {
                        if (camera.GetComponent<AQUAS_UnderWaterEffect>() == null)
                        {
                            camera.AddComponent<AQUAS_UnderWaterEffect_Simple>();
                        }
                        else
                        {
                            if (EditorUtility.DisplayDialog("Wargning!", "You already have advanced underwater effects enabled for your camera. Do you wish to replace them with simple underwater effects?", "Replace", "Don't Replace"))
                            {
                                camera.AddComponent<AQUAS_UnderWaterEffect_Simple>();
                                DestroyImmediate(camera.GetComponent<AQUAS_UnderWaterEffect>());
                            }
                            else
                            {

                            }
                        }
                    }

                    break;
            }

            if(camera.GetComponent<BoxCollider>() == null)
            {
                camera.AddComponent<BoxCollider>();
            }

            camera.GetComponent<BoxCollider>().isTrigger = true;
        }

        void CreateRiverReference()
        {
            if (camera == null)
            {
                camera = Camera.main.transform.gameObject;

                if (camera == null)
                {
                    EditorUtility.DisplayDialog("Camera Missing!", "Please specify the main camera before creating a river reference.", "OK");
                    return;
                }
            }

            if (waterplane == null)
            {
                EditorUtility.DisplayDialog("Water plane Missing", "Please select a water plane before creating a river reference.", "OK");
                return;
            }

            //Cache the camera's parameters
            Vector3 cameraPosition = camera.transform.position;
            Quaternion cameraRotation = camera.transform.rotation;
            bool projection = camera.GetComponent<Camera>().orthographic;
            float orthographicSize = camera.GetComponent<Camera>().orthographicSize;

            //Set the camera's position and parameters for taking a screenshot
            camera.transform.position = new Vector3(waterplane.transform.position.x, waterplane.transform.position.y + Mathf.Min((camera.GetComponent<Camera>().farClipPlane - 50), 1000), waterplane.transform.position.z);
            camera.transform.rotation = Quaternion.Euler(90, waterplane.transform.rotation.y, waterplane.transform.rotation.z);
            camera.GetComponent<Camera>().orthographic = true;
            camera.GetComponent<Camera>().orthographicSize = waterplane.transform.localScale.x * 5;

            //Cache the material from the river plane and set the material with the reference texture on it
            Material cachedMaterial = waterplane.GetComponent<UnityEngine.Renderer>().sharedMaterial;
            Material referenceTexMat = (Material)AssetDatabase.LoadAssetAtPath("Assets/AQUAS 2020/Materials/RiverRef.mat", typeof(Material));
            waterplane.GetComponent<Renderer>().sharedMaterial = referenceTexMat;

            //Cache Fog Settings
            bool fog = RenderSettings.fog;

            RenderSettings.fog = false;

            //Take a screenshot from the river plane and save it to use as a reference texture to help paint flowmaps
            //Source: http://answers.unity3d.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
            RenderTexture rt = new RenderTexture(1024, 1024, 24);

            camera.GetComponent<Camera>().targetTexture = rt;

            Texture2D riverReference = new Texture2D(1024, 1024, TextureFormat.RGB24, false);

            camera.GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            riverReference.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
            camera.GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);

            byte[] bytes = riverReference.EncodeToJPG();
            string filename = string.Format("{0}/AQUAS 2020/RiverReferences/Reference_{1}.jpg", Application.dataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            if (!System.IO.Directory.Exists(string.Format("{0}/AQUAS 2020/RiverReferences", Application.dataPath)))
            {
                System.IO.Directory.CreateDirectory(string.Format("{0}/AQUAS 2020/RiverReferences", Application.dataPath));
            }

            System.IO.File.WriteAllBytes(filename, bytes);

            camera.transform.position = cameraPosition;
            camera.transform.rotation = cameraRotation;
            camera.GetComponent<Camera>().orthographic = projection;
            camera.GetComponent<Camera>().orthographicSize = orthographicSize;

            waterplane.GetComponent<UnityEngine.Renderer>().sharedMaterial = cachedMaterial;

            RenderSettings.fog = fog;

            AssetDatabase.Refresh();
        }

        void CreateTerrainMask()
        {
            TerrainData terrainData = terrain.GetComponent<Terrain>().terrainData;
            
            int index = 0;
            Texture2D duplicateHeightMap = new Texture2D(terrainData.heightmapResolution, terrainData.heightmapResolution, TextureFormat.ARGB32, false);
            duplicateHeightMap.filterMode = FilterMode.Trilinear;
            duplicateHeightMap.anisoLevel = 16;
            float[,] rawHeights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

            float seaLevel = waterplane.transform.position.y;

            float testHeight;
            float nrmSeaLevel = (seaLevel+0.9f) / terrainData.size.y;
            float nrmStartDepth = (seaLevel - maskDepth) / terrainData.size.y;
            float nrmRange = (nrmSeaLevel - nrmStartDepth);

            for (int i = 0; i < duplicateHeightMap.height-1; i++)
            {
                for (int j = 0; j < duplicateHeightMap.width; j++)
                {
                    testHeight = rawHeights[i, j];

                    Color color;

                    if (testHeight < nrmStartDepth)
                    {
                        color = Color.black;
                    }
                    /*else if(testHeight > nrmSeaLevel)
                    {
                        color = Color.black;
                    }*/
                    else
                    {
                        color = new Vector4(1 - ((nrmSeaLevel - testHeight) / nrmRange), 1 - ((nrmSeaLevel - testHeight) / nrmRange), 1 - ((nrmSeaLevel - testHeight) / nrmRange), 1);
                    }

                    duplicateHeightMap.SetPixel(i, j, color);
                    index++;
                }
            }

            duplicateHeightMap.Apply();

            byte[] bytes = duplicateHeightMap.EncodeToJPG();
            string filename = string.Format("{0}/AQUAS 2020/TerrainMasks/Mask_{1}.jpg", Application.dataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            if (!System.IO.Directory.Exists(string.Format("{0}/AQUAS 2020/TerrainMasks", Application.dataPath)))
            {
                System.IO.Directory.CreateDirectory(string.Format("{0}/AQUAS 2020/TerrainMasks", Application.dataPath));
            }

            System.IO.File.WriteAllBytes(filename, bytes);
            
            /*string path = EditorUtility.SaveFilePanel(
                "Save texture as",
                "",
                "Rename Me",
                "png, jpg");

            var extension = Path.GetExtension(path);
            byte[] pngData = null;// duplicateHeightMap.EncodeToPNG();

            switch (extension)
            {
                case ".jpg":
                    pngData = duplicateHeightMap.EncodeToJPG();
                    break;

                case ".png":
                    pngData = duplicateHeightMap.EncodeToPNG();
                    break;
            }

            if (pngData != null)
            {
                File.WriteAllBytes(path, pngData);
                EditorUtility.DisplayDialog("Heightmap Duplicated", "Saved as" + extension + " in " + path, "Awesome");
            }
            else
            {
                EditorUtility.DisplayDialog("Failed to duplicate height map", "eh something happen hu? lol", "Check Script");
            }*/

            AssetDatabase.Refresh();
        }
    }
}