using UnityEngine;

namespace AQUAS
{
    public class AQUAS_UnderWaterEffect_Simple : MonoBehaviour
    {

        [Header("Underwater Settings")]

        ///<summary>
        /// Public variables
        /// Note: Not all underwater parameters are controlled from here - some parameters are controlled via the AQUAS_UnderwaterParameters attached to each waterplane
        /// </summary>
        [Range(0, 0.1f)]
        public float distortionStrength = 0.025f;

        [Range(0, 0.002f)]
        public float blurSize = 0.001f;

        [Range(0, 10)]
        public int blurSmoothness = 5;

        public bool enableWetLensEffect = true;
        [Range(0.5f, 3)]
        public float wetLensDuration = 1;

        [Space(10)]
        [Header("Underwater Audio Settings")]
        public AudioClip underwaterAmbientSound;
        [Range(0, 1)]
        public float underwaterAmbientVolume = 0.5f;
        public AudioClip diveSplashSound;
        [Range(0, 1)]
        public float diveSplashVolume = 1;
        public AudioClip surfaceSplashSound;
        [Range(0, 1)]
        public float surfaceSplashVolume = 0.2f;

        [Space(10)]
        [Header("Bubble Spawn Settings")]
        [Range(5, 50)]
        public int maximumBubbleCount = 10;
        public bool spawnBubbles = false;

        /// <summary>
        /// Fog parameters
        /// </summary>
        float fogDensity = 1;
        float maxFogDepth = 10;

        Color fogColor;
        Color deepFogColor;
        Color adjustedFogColor;

        /// <summary>
        /// Cache the default values of the fog settings to revert whenever not underwater
        /// </summary>
        bool defaultFog;
        Color defaultFogColor;
        FogMode defaultFogMode;
        float defaultFogDensity;

        /// <summary>
        /// The simple underwater effects calculate the duration of the droplet rendering in the script, not in the shader
        /// thus we need to store the passage of time somehow - this is what t is for
        /// The real blur smoothness is either 0 or equals the blur smoothness, depending on whether underwater or not
        /// </summary>
        float t = 1;
        int realBlurSmoothness;

        /// <summary>
        /// Gameobjects, Materials and render textures
        /// </summary>
        GameObject waterPlane;

        Material underwaterMat;
        Material blurMat;

        RenderTexture underwaterBuffer;
        RenderTexture buffer1;
        RenderTexture buffer2;

        /// <summary>
        /// This bool stores the information whether the camera is underwater or not. Since there is no smooth transition,
        /// we need this information to know whether to render underwater image effects or not
        /// Other scripts can use this bool, when they need to know if the camera is underwater (more than 50%)
        /// </summary>
        [HideInInspector]
        public bool underwater;

        //Required when using projected grid, because the waterplane's y-level will then not equal the water level
        [HideInInspector]
        public float waterLevel;

        /// <summary>
        /// Variables we need for the underwater audio
        /// </summary>
        GameObject audioObject;

        AudioSource underwaterAmbient;
        AudioSource diveSplash;
        AudioSource surfaceSplash;
        bool diveSplashPlayed = true;
        bool surfaceSplashPlayed = true;

        /// <summary>
        /// Variables for the bubble spawning system
        /// </summary>
        GameObject bubbleContainer;
        GameObject bubble;
        AQUAS_BubbleBehaviour bubbleBehaviour;
        float t2 = 0;
        float bubbleSpawnTimer = 0;
        int maxBubbleCount;
        int bubbleCount;


        void Start()
        {
            ///<summary>
            ///Cache default fog settings and restore whenever not underwater
            /// </summary>
            defaultFog = RenderSettings.fog;
            defaultFogColor = RenderSettings.fogColor;
            defaultFogMode = RenderSettings.fogMode;
            defaultFogDensity = RenderSettings.fogDensity;

            ///<summary>
            ///Set up the materials
            /// </summary>
            underwaterMat = new Material(Shader.Find("Hidden/AQUAS/Underwater/Fog Simple"));
            blurMat = new Material(Shader.Find("Hidden/AQUAS/Underwater/Blur Simple"));

            ///<summary>
            ///Get the screen ratio for the scale of the droplet textures
            /// </summary>
            float ratio = (float)Screen.width / (float)Screen.height;

            ///<summary>
            ///Preset some of the fog material's required texture properties
            /// </summary>
            underwaterMat.SetTexture("_DistortionLens", (Texture2D)Resources.Load("distortion_ellipse", typeof(Texture2D)));
            underwaterMat.SetTexture("_DropletNormals", (Texture2D)Resources.Load("wet_lens_normal", typeof(Texture2D)));
            underwaterMat.SetTextureScale("_DropletNormals", new Vector2(ratio, 1));
            underwaterMat.SetTexture("_DropletCutout", (Texture2D)Resources.Load("wet_lens_cutout", typeof(Texture2D)));
            underwaterMat.SetTextureScale("_DropletCutout", new Vector2(ratio, 1));

            ///<summary>
            ///Set up all required render textures
            /// </summary>
            underwaterBuffer = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
            buffer1 = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
            buffer2 = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);

            ///<summary>
            ///Set up the underwater audio:
            ///- Instantiate audio object as empty gameobject
            ///- Assign clips from the resources if they're null
            ///- Add Aadio aource components to the audio object
            ///- Assign clips to audio sources
            ///- Set looing of splash sounds to false
            /// </summary>
            audioObject = new GameObject("Underwater Audio");
            audioObject.transform.parent = transform;
            audioObject.hideFlags = HideFlags.HideAndDontSave;

            if (underwaterAmbientSound == null)
            {
                underwaterAmbientSound = (AudioClip)Resources.Load("underwater");
            }
            if (diveSplashSound == null)
            {
                diveSplashSound = (AudioClip)Resources.Load("dive-splash");
            }
            if (surfaceSplashSound == null)
            {
                surfaceSplashSound = (AudioClip)Resources.Load("surfacing-splash");
            }

            underwaterAmbient = audioObject.AddComponent<AudioSource>();
            diveSplash = audioObject.AddComponent<AudioSource>();
            surfaceSplash = audioObject.AddComponent<AudioSource>();

            underwaterAmbient.clip = underwaterAmbientSound;
            diveSplash.clip = diveSplashSound;
            surfaceSplash.clip = surfaceSplashSound;

            diveSplash.loop = false;
            surfaceSplash.loop = false;

            ///<summary>
            ///Get prefab and behaviour component for the bubble spawning
            /// </summary>
            bubble = (GameObject)Resources.Load("Bubble", typeof(GameObject));
            bubbleBehaviour = bubble.GetComponent<AQUAS_BubbleBehaviour>();

            if (spawnBubbles)
            {
                bubbleContainer = new GameObject("Bubble Container");
                bubbleContainer.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ///<summary>
            ///When making contact with the water do:
            ///- Get the waterplane
            ///- Enable the rendering of the static boundary
            ///- Get the additional underwater parameters from the waterplane itself
            ///- Change the shader type of the backface shader to the transparent type 
            ///     - we can't use the opaque type here, since we don't capture the background 
            ///     - this saves us a lot of performance, but disables depth buffer support at the same time (neccessary trade-off)
            /// </summary>
            if (other.gameObject.name == "Static Boundary")
            {
                if (other.transform.parent == null)
                {
                    waterPlane = GetComponent<AQUAS_ProjectedGrid>().waterplane;
                }
                else
                {
                    waterPlane = other.transform.parent.gameObject;
                }

                other.GetComponent<MeshRenderer>().enabled = true;

                fogColor = waterPlane.GetComponent<AQUAS_UnderwaterParameters>().mainFogColor;
                deepFogColor = waterPlane.GetComponent<AQUAS_UnderwaterParameters>().deepFogColor;
                fogDensity = waterPlane.GetComponent<AQUAS_UnderwaterParameters>().mainFogDensity / 10;
                maxFogDepth = waterPlane.GetComponent<AQUAS_UnderwaterParameters>().maxFogDepth;

                waterPlane.GetComponent<Renderer>().sharedMaterials[1].shader = Shader.Find("AQUAS/Desktop/Back/Transparent Back");
            }
            else
            {

            }
        }

        private void OnTriggerExit(Collider other)
        {
            ///<summary>
            ///When leaving the water entirely, do:
            ///- Disable the mesh renderer of the static boundary
            ///- Change the shader type of the backface shader back to the opaque type
            ///- Release the waterplane
            /// </summary>
            if (other.gameObject.name == "Static Boundary")
            {
                other.GetComponent<MeshRenderer>().enabled = false;

                waterPlane.GetComponent<Renderer>().sharedMaterials[1].shader = Shader.Find("AQUAS/Desktop/Back/Transparent Back");

                waterPlane = null;
            }
            else
            {

            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            ///<summary>
            ///Depending on the amount of real blur mmoothness blurring will be rendered in iterations,
            ///passing previous results to buffers and reuse them until the desired smoothness is achieved
            ///In any case (real blur smoothness > 0 or not, the duration of the droplet rendering is set based on t)
            /// </summary>
            if (realBlurSmoothness > 0)
            {
                underwaterMat.SetFloat("_Distortion", distortionStrength);

                if (enableWetLensEffect)
                {
                    underwaterMat.SetFloat("_Wetness", 1 - t);
                }

                Graphics.Blit(source, underwaterBuffer, underwaterMat);

                blurMat.SetFloat("_BlurSize", blurSize);

                if (realBlurSmoothness == 1)
                {
                    Graphics.Blit(underwaterBuffer, destination, blurMat);
                    return;
                }
                if (realBlurSmoothness == 2)
                {
                    Graphics.Blit(underwaterBuffer, buffer1, blurMat);
                    Graphics.Blit(buffer1, destination, blurMat);
                    buffer1.Release();
                    return;
                }
                if (realBlurSmoothness > 2)
                {
                    for (int i = 0; i < realBlurSmoothness; i++)
                    {
                        if (i == 0)
                        {
                            Graphics.Blit(underwaterBuffer, buffer1, blurMat);
                        }
                        if (i == realBlurSmoothness - 1)
                        {
                            if (i % 2 == 1)
                            {
                                Graphics.Blit(buffer1, destination, blurMat);
                                buffer1.Release();
                            }
                            else
                            {
                                Graphics.Blit(buffer2, destination, blurMat);
                                buffer2.Release();
                            }

                        }
                        if (i > 0 && i < realBlurSmoothness - 1)
                        {
                            if (i % 2 == 1)
                            {
                                Graphics.Blit(buffer1, buffer2, blurMat);
                                buffer1.Release();
                            }
                            else
                            {
                                Graphics.Blit(buffer2, buffer1, blurMat);
                                buffer2.Release();
                            }
                        }
                    }
                }
            }
            else
            {
                underwaterMat.SetFloat("_Distortion", distortionStrength);

                if (enableWetLensEffect)
                {
                    underwaterMat.SetFloat("_Wetness", 1 - t);
                }

                Graphics.Blit(source, destination, underwaterMat);

                blurMat.SetFloat("_BlurSize", blurSize);
            }
        }

        void Update()
        {
            ///<summary>
            ///Check if underwater and pass value to underwater variable
            ///Also useful for custom scripts that may want to access this information
            /// </summary>
            if (waterPlane != null)
            {

                //Check if the plane uses grid projection and if so, use static waterlevel rather than the y-position
                if(waterPlane.GetComponent<AQUAS_CamLock>() != null && waterPlane.GetComponent<AQUAS_CamLock>().useDynamicMesh)
                {
                    if (transform.position.y < waterLevel)
                    {
                        underwater = true;
                    }
                    else
                    {
                        underwater = false;
                    }
                }
                else
                {
                    if (transform.position.y < waterPlane.transform.position.y)
                    {
                        underwater = true;
                    }
                    else
                    {
                        underwater = false;
                    }
                }

                adjustedFogColor = Color.Lerp(fogColor, deepFogColor, (waterPlane.transform.position.y - transform.position.y) / maxFogDepth);
            }
            else
            {
                underwater = false;
            }

            ///<summary>
            ///See method descriptions for more information
            /// </summary>
            ToggleFog(underwater);
            WetLensDistortionSwitch(underwater);

            ///<summary>
            ///Update volume for audio sources
            /// </summary>
            underwaterAmbient.volume = underwaterAmbientVolume;
            diveSplash.volume = diveSplashVolume;
            surfaceSplash.volume = surfaceSplashVolume;

            ///<summary>
            ///Based on whether camera is underwater or not, do:
            ///- Set t and the real blur smoothness
            ///- Play underwater ambient sound
            ///- Play surfacing splash or diving splash
            /// </summary>
            if (underwater)
            {
                t = 0;
                realBlurSmoothness = blurSmoothness;

                surfaceSplashPlayed = false;

                if (!underwaterAmbient.isPlaying)
                {
                    underwaterAmbient.Play();
                }
                if (!diveSplashPlayed)
                {
                    diveSplash.Play();
                    diveSplashPlayed = true;
                }

                ///<summary>
                ///Call bubble spawner method, if enabled
                ///Run timer for the bubble spawner
                /// </summary>
                if (spawnBubbles)
                {
                    if (bubbleContainer == null)
                    {
                        bubbleContainer = new GameObject("Bubble Container");
                    }

                    t2 += Time.deltaTime;
                    BubbleSpawner();
                }
            }
            else
            {
                t = Mathf.Min(t + Time.deltaTime / wetLensDuration, 1);
                realBlurSmoothness = 0;

                diveSplashPlayed = false;

                if (underwaterAmbient.isPlaying)
                {
                    underwaterAmbient.Stop();
                }
                if (!surfaceSplashPlayed)
                {
                    surfaceSplash.Play();
                    surfaceSplashPlayed = true;
                }

                ///<summary>
                ///Reset timer for the bubble spawner
                ///Reset maximum bubble count to random value
                ///Set amount of actual bubbles to 0
                /// </summary>
                t2 = 0;
                bubbleSpawnTimer = 0;
                maxBubbleCount = (int)Random.Range(maximumBubbleCount / 2, maximumBubbleCount);
                bubbleCount = 0;

            }
        }

        /// <summary>
        /// This method decides which parameters to use for the fog based on whether the camera is considered underwater or afloat
        /// if underwater, the water specific parameters will be used
        /// if afloat, the cached default values will be used
        /// </summary>
        /// <param name="isUnderwater"></param>
        void ToggleFog(bool isUnderwater)
        {
            if (isUnderwater)
            {
                RenderSettings.fogMode = FogMode.Exponential;
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.fogColor = adjustedFogColor;
                RenderSettings.fog = true;
            }
            else
            {
                RenderSettings.fog = defaultFog;
                RenderSettings.fogMode = defaultFogMode;
                RenderSettings.fogColor = defaultFogColor;
                RenderSettings.fogDensity = defaultFogDensity;
            }
        }

        /// <summary>
        /// This method decides enables and disables image distortion based on whether the camera is considered underwater or afloat
        /// </summary>
        /// <param name="isUnderwater"></param>
        void WetLensDistortionSwitch(bool isUnderwater)
        {
            if (isUnderwater)
            {
                underwaterMat.SetFloat("_WetLensDistortionSwitch", 1);
            }
            else
            {
                underwaterMat.SetFloat("_WetLensDistortionSwitch", 0);
            }

        }

        ///<summary>
        ///Spawns bubbles according to the parameters set
        ///Small bubbles being spawned directly by the bubbles
        ///Small bubbles parameters & randomization are based on bubble parameters but not directly controllable
        ///</summary>
        void BubbleSpawner()
        {
            ///<summary>
            ///Applies spawning rules for initial dive
            ///</summary>
            #region Spawn for initial dive
            if (t2 > bubbleSpawnTimer && maxBubbleCount > bubbleCount)
            {

                float bubbleScaleFactor = Random.Range(0, 0.06f);

                bubbleBehaviour.mainCamera = this.gameObject;

                //Check if the plane uses grid projection and if so, use static waterlevel rather than the y-position
                if (waterPlane.GetComponent<AQUAS_CamLock>() != null && waterPlane.GetComponent<AQUAS_CamLock>().useDynamicMesh)
                {
                    bubbleBehaviour.waterLevel = waterLevel;
                }
                else
                {
                    bubbleBehaviour.waterLevel = waterPlane.transform.position.y;
                }
                
                bubbleBehaviour.averageUpdrift = 1 + Random.Range(-0.3f, 0.3f);

                bubble.transform.localScale += new Vector3(bubbleScaleFactor, bubbleScaleFactor, bubbleScaleFactor);

                Instantiate(bubble, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y - 0.4f, transform.position.z + Random.Range(-0.5f, 0.5f)), Quaternion.identity).transform.SetParent(bubbleContainer.transform);

                bubbleSpawnTimer += Random.Range(0.02f, 0.1f);

                bubbleCount += 1;

                bubble.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            }
            #endregion

            ///<summary>
            ///Applies spawning rules for long dive
            ///Definition for long dive: bubbleCount == maxBubbleCount
            /// </summary>
            #region Spawn for long dive
            else if (t2 > bubbleSpawnTimer && maxBubbleCount <= bubbleCount)
            {
                float bubbleScaleFactor = Random.Range(0, 0.06f);

                bubbleBehaviour.mainCamera = this.gameObject;

                //Check if the plane uses grid projection and if so, use static waterlevel rather than the y-position
                if (waterPlane.GetComponent<AQUAS_CamLock>() != null && waterPlane.GetComponent<AQUAS_CamLock>().useDynamicMesh)
                {
                    bubbleBehaviour.waterLevel = waterLevel;
                }
                else
                {
                    bubbleBehaviour.waterLevel = waterPlane.transform.position.y;
                }

                bubbleBehaviour.averageUpdrift = 1 + Random.Range(-0.3f, 0.3f);

                bubble.transform.localScale += new Vector3(bubbleScaleFactor, bubbleScaleFactor, bubbleScaleFactor);

                Instantiate(bubble, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y - 0.4f, transform.position.z + Random.Range(-0.5f, 0.5f)), Quaternion.identity).transform.SetParent(bubbleContainer.transform);

                bubbleSpawnTimer += Random.Range(0.02f, 0.2f);

                bubble.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            }
            #endregion
        }
    }
}
