using UnityEngine;

namespace AQUAS
{
    public class AQUAS_UnderWaterEffect : MonoBehaviour
    {
        public enum QUALITY
        {
            low,
            medium,
            high
        };

        [Header("Underwater Settings")]

        ///<summary>
        /// Public variables
        /// Note: Not all underwater parameters are controlled from here - some parameters are controlled via the AQUAS_UnderwaterParameters attached to each waterplane
        /// </summary>

        public LayerMask cullingMask = -1;

        public QUALITY backFaceQuality = QUALITY.medium;

        [Range(0.1f, 1)]
        public float fogFade = 1;

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
        float deepFogDensity = 1.5f;
        float maxFogDepth = 10;
        float adjustedFogDensity;

        Color fogColor;
        Color deepFogColor;
        Color adjustedFogColor;

        /// <summary>
        /// Gameobjects
        /// </summary>
        GameObject container;

        GameObject boundaryMask;
        GameObject volumeMask;
        GameObject frontFaceMask;
        GameObject background;

        GameObject waterPlane;

        //Get all waterplanes and their static boundaries
        GameObject[] waterObjects;
        GameObject[] staticBoundaries;

        //a dynamic boundary to use if there is a projected mesh in use
        [HideInInspector]
        public GameObject dynamicBoundary;

        /// <summary>
        /// Materials for underwater image effects
        /// </summary>
        Material fogMat;
        Material blurMat;
        Material dropletMaskMat;

        /// <summary>
        /// Mask that is used to tell the image effects which part of the screen is underwater and which part is afloat.
        /// </summary>
        [HideInInspector]
        public RenderTexture mask;

        /// <summary>
        /// Various render textures required for underwater image effects.
        /// </summary>
        RenderTexture buffer1;
        RenderTexture buffer2;

        RenderTexture fogBuffer;

        RenderTexture dropletBuffer;
        RenderTexture dropletMask;

        /// <summary>
        /// Other scripts can use this bool, when they need to know if the camera is underwater (more than 50%)
        /// </summary>
        [HideInInspector]
        public bool underwater;

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
        [HideInInspector]
        public float waterLevel;

        void Start()
        {

            ///<summary>
            ///Get the container of the waterplanes and give warning if not found
            ///Assign waterplanes and static boundaries to the gameobject arrays respectively
            /// </summary>
            container = GameObject.Find("AQUAS Container");

            if (container == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("AQUAS couldn't find the object called 'AQUAS Container', that should hold all waterplane objects as children. Underwater effects cannot be rendered. Parent all waterplanes to an object called 'AQUAS Container' to enable underwater effects.");
#endif
                this.enabled = false;
                return;
            }

            waterObjects = new GameObject[container.transform.childCount];
            staticBoundaries = new GameObject[container.transform.childCount];

            for (int i = 0; i < container.transform.childCount; i++)
            {
                waterObjects[i] = container.transform.GetChild(i).gameObject;

                if (waterObjects[i].transform.Find("Static Boundary") != null)
                {
                    staticBoundaries[i] = waterObjects[i].transform.Find("Static Boundary").gameObject;
                }
                else
                {
                    staticBoundaries[i] = dynamicBoundary;
                }
            }

            ///<summary>
            ///Instantiate objects for creating the final mask and the capturing of the background
            ///Add all required components to each mask object
            ///Set up the layer settings for the mask objects
            /// </summary>
            boundaryMask = new GameObject("Boundary Mask");
            volumeMask = new GameObject("Volume Mask");
            frontFaceMask = new GameObject("Front Face Mask");
            background = new GameObject("Background");

            boundaryMask.hideFlags = HideFlags.HideAndDontSave;
            volumeMask.hideFlags = HideFlags.HideAndDontSave;
            frontFaceMask.hideFlags = HideFlags.HideAndDontSave;
            background.hideFlags = HideFlags.HideAndDontSave;

            boundaryMask.transform.SetParent(transform);
            boundaryMask.AddComponent<Camera>().CopyFrom(GetComponent<Camera>());
            boundaryMask.GetComponent<Camera>().cullingMask &= ~1 << LayerMask.NameToLayer("Everything");
            boundaryMask.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Water");
            boundaryMask.GetComponent<Camera>().depth = GetComponent<Camera>().depth - 4;
            boundaryMask.AddComponent<AQUAS_BoundaryMask>();
            boundaryMask.GetComponent<AQUAS_BoundaryMask>().nextCam = volumeMask;
            boundaryMask.AddComponent<AQUAS_ReflectNot>();

            volumeMask.transform.SetParent(transform);
            volumeMask.AddComponent<Camera>().CopyFrom(GetComponent<Camera>());
            volumeMask.GetComponent<Camera>().cullingMask &= ~1 << LayerMask.NameToLayer("Everything");
            volumeMask.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Water");
            volumeMask.GetComponent<Camera>().depth = GetComponent<Camera>().depth - 3;
            volumeMask.AddComponent<AQUAS_VolumeMask>();
            volumeMask.GetComponent<AQUAS_VolumeMask>().nextCam = frontFaceMask;
            volumeMask.AddComponent<AQUAS_ReflectNot>();

            frontFaceMask.transform.SetParent(transform);
            frontFaceMask.AddComponent<Camera>().CopyFrom(GetComponent<Camera>());
            frontFaceMask.GetComponent<Camera>().cullingMask &= ~1 << LayerMask.NameToLayer("Everything");
            frontFaceMask.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Water");
            frontFaceMask.GetComponent<Camera>().depth = GetComponent<Camera>().depth - 2;
            frontFaceMask.AddComponent<AQUAS_FrontFaceMask>();
            frontFaceMask.GetComponent<AQUAS_FrontFaceMask>().nextCam = gameObject;
            frontFaceMask.AddComponent<AQUAS_ReflectNot>();

            background.transform.SetParent(transform);
            background.AddComponent<Camera>().CopyFrom(GetComponent<Camera>());
            background.GetComponent<Camera>().cullingMask = cullingMask;
            background.GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("Water"));
            background.GetComponent<Camera>().depth = GetComponent<Camera>().depth - 1;
            background.AddComponent<AQUAS_CaptureBackground>();
            background.AddComponent<AQUAS_ReflectNot>();

            switch (backFaceQuality)
            {
                case QUALITY.low:
                    background.GetComponent<AQUAS_CaptureBackground>().quality = 3;
                    break;

                case QUALITY.medium:
                    background.GetComponent<AQUAS_CaptureBackground>().quality = 2;
                    break;

                case QUALITY.high:
                    background.GetComponent<AQUAS_CaptureBackground>().quality = 0;
                    break;
            }
            

            ///<summary>
            ///Set up the materials
            /// </summary>
            fogMat = new Material(Shader.Find("Hidden/AQUAS/Underwater/Fog"));
            blurMat = new Material(Shader.Find("Hidden/AQUAS/Underwater/Blur"));
            dropletMaskMat = new Material(Shader.Find("Hidden/AQUAS/Underwater/Droplet Mask"));

            ///<summary>
            ///Get the screen ratio for the scale of the droplet textures
            /// </summary>
            float ratio = (float)Screen.width / (float)Screen.height;

            ///<summary>
            ///Preset some of the fog material's required texture properties
            /// </summary>
            fogMat.SetTexture("_DistortionLens", (Texture2D)Resources.Load("distortion_ellipse", typeof(Texture2D)));
            fogMat.SetTexture("_DropletNormals", (Texture2D)Resources.Load("wet_lens_normal", typeof(Texture2D)));
            fogMat.SetTextureScale("_DropletNormals", new Vector2(ratio, 1));
            fogMat.SetTexture("_DropletCutout", (Texture2D)Resources.Load("wet_lens_cutout", typeof(Texture2D)));
            fogMat.SetTextureScale("_DropletCutout", new Vector2(ratio, 1));

            ///<summary>
            ///Set up all required render textures
            /// </summary>
            mask = new RenderTexture(Screen.height, Screen.width, 8, RenderTextureFormat.ARGB32);

            buffer1 = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
            buffer2 = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
            fogBuffer = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
            dropletBuffer = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);
            dropletMask = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32);

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
            ///- Set the correct layer for all waterplanes
            ///- Assign the waterplante to the masking components of the mask objects
            /// </summary>
            if (other.gameObject.name == "Static Boundary")
            {
                if(other.transform.parent == null)
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
                fogDensity = waterPlane.GetComponent<AQUAS_UnderwaterParameters>().mainFogDensity;
                deepFogDensity = waterPlane.GetComponent<AQUAS_UnderwaterParameters>().deepFogDensity;
                maxFogDepth = waterPlane.GetComponent<AQUAS_UnderwaterParameters>().maxFogDepth;

                for (int i = 0; i < waterObjects.Length; i++)
                {
                    waterObjects[i].layer = LayerMask.NameToLayer("Default");
                    staticBoundaries[i].layer = LayerMask.NameToLayer("Default");
                }

                boundaryMask.GetComponent<AQUAS_BoundaryMask>().boundaryObj = other.gameObject;

                volumeMask.GetComponent<AQUAS_VolumeMask>().waterObj = waterPlane;
                volumeMask.GetComponent<AQUAS_VolumeMask>().rend = waterPlane.GetComponent<Renderer>();
                volumeMask.GetComponent<AQUAS_VolumeMask>().waterShaders[0] = waterPlane.GetComponent<Renderer>().sharedMaterials[0].shader;
                volumeMask.GetComponent<AQUAS_VolumeMask>().waterShaders[1] = waterPlane.GetComponent<Renderer>().sharedMaterials[1].shader;

                frontFaceMask.GetComponent<AQUAS_FrontFaceMask>().waterObj = waterPlane;
                frontFaceMask.GetComponent<AQUAS_FrontFaceMask>().rend = waterPlane.GetComponent<Renderer>();
                frontFaceMask.GetComponent<AQUAS_FrontFaceMask>().waterShaders[0] = waterPlane.GetComponent<Renderer>().sharedMaterials[0].shader;
                frontFaceMask.GetComponent<AQUAS_FrontFaceMask>().waterShaders[1] = waterPlane.GetComponent<Renderer>().sharedMaterials[1].shader;

                background.GetComponent<AQUAS_CaptureBackground>().waterObj = waterPlane;
                background.GetComponent<AQUAS_CaptureBackground>().layerMask = cullingMask;
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
            ///- Release the waterplane
            ///- Set the correct layer for all waterplanes
            ///- Release the waterplante from the masking components of the mask objects
            /// </summary>
            if (other.gameObject.name == "Static Boundary")
            {
                other.GetComponent<MeshRenderer>().enabled = false;

                waterPlane = null;

                for (int i = 0; i < waterObjects.Length; i++)
                {
                    waterObjects[i].layer = LayerMask.NameToLayer("Water");
                    staticBoundaries[i].layer = LayerMask.NameToLayer("Water");
                }

                boundaryMask.GetComponent<AQUAS_BoundaryMask>().boundaryObj = null;

                volumeMask.GetComponent<AQUAS_VolumeMask>().waterObj = null;

                frontFaceMask.GetComponent<AQUAS_FrontFaceMask>().waterObj = null;

                background.GetComponent<AQUAS_CaptureBackground>().waterObj = null;
            }
            else
            {

            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            ///<summary>
            ///This is used to render the underwater image effects
            /// </summary>

            ///<summary>
            ///First render the droplet mask - It tells the underwater image effect where to render droplets
            /// </summary>
            dropletMaskMat.SetTexture("_DropletMask", dropletMask);
            dropletMaskMat.SetFloat("_Duration", wetLensDuration);
            Graphics.Blit(mask, dropletBuffer, dropletMaskMat);
            Graphics.Blit(dropletBuffer, dropletMask);
            fogMat.SetTexture("_DropletMask", dropletMask);

            ///<summary>
            ///Depending on the amount of blur Smoothness blurring will be rendered in iterations,
            ///passing previous results to buffers and reuse them until the desired smoothness is achieved
            /// </summary>
            if (blurSmoothness > 0)
            {
                fogMat.SetTexture("_DepthMask", mask);
                fogMat.SetFloat("_Density", adjustedFogDensity);
                fogMat.SetColor("_FogColor", adjustedFogColor);
                fogMat.SetFloat("_Distortion", distortionStrength);
                fogMat.SetFloat("_Fade", fogFade);

                if (enableWetLensEffect)
                {
                    fogMat.SetFloat("_EnableWetLens", 1);
                }
                else
                {
                    fogMat.SetFloat("_EnableWetLens", 0);
                }

                Graphics.Blit(source, fogBuffer, fogMat);

                blurMat.SetTexture("_DepthMask", mask);
                blurMat.SetFloat("_BlurSize", blurSize);

                if (blurSmoothness == 1)
                {
                    Graphics.Blit(fogBuffer, destination, blurMat);
                    return;
                }
                if (blurSmoothness == 2)
                {
                    Graphics.Blit(fogBuffer, buffer1, blurMat);
                    Graphics.Blit(buffer1, destination, blurMat);
                    buffer1.Release();
                    return;
                }
                if (blurSmoothness > 2)
                {
                    for (int i = 0; i < blurSmoothness; i++)
                    {
                        if (i == 0)
                        {
                            Graphics.Blit(fogBuffer, buffer1, blurMat);
                        }
                        if (i == blurSmoothness - 1)
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
                        if (i > 0 && i < blurSmoothness - 1)
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

            ///<summary>
            ///If blurSmoothness is 0, blur will not be rendered
            /// </summary>
            else
            {
                fogMat.SetTexture("_DepthMask", mask);
                fogMat.SetFloat("_Density", adjustedFogDensity);
                fogMat.SetColor("_FogColor", adjustedFogColor);
                fogMat.SetFloat("_Distortion", distortionStrength);
                fogMat.SetFloat("_Fade", fogFade);

                Graphics.Blit(source, destination, fogMat);
            }
        }

        void Update()
        {

            ///<summa
            ///Check if underwater and pass value to underwater variable
            ///This script doesn't require it, but it can be useful for custom scripts that may want to access this information
            /// </summary>
            if (waterPlane != null)
            {
                if (waterPlane.GetComponent<AQUAS_CamLock>() != null && waterPlane.GetComponent<AQUAS_CamLock>().useDynamicMesh)
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
            ///Update volume for audio sources
            /// </summary>
            underwaterAmbient.volume = underwaterAmbientVolume;
            diveSplash.volume = diveSplashVolume;
            surfaceSplash.volume = surfaceSplashVolume;

            ///<summary>
            ///Based on whether camera is underwater or not, do:
            ///- Play underwater ambient sound
            ///- Play surfacing splash or diving splash
            /// </summary>
            if (underwater)
            {
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
                        bubbleContainer.hideFlags = HideFlags.HideAndDontSave;
                    }

                    t2 += Time.deltaTime;
                    BubbleSpawner();
                }
            }
            else
            {
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

            ///<summar>
            ///If aspect ratio changes, adjust the render textures accordingly
            /// </summar>
            /*if (mask.width != Screen.width || mask.height != Screen.height)
            {
                mask.Release();
                buffer1.Release();
                buffer2.Release();
                fogBuffer.Release();
                dropletBuffer.Release();
                dropletMask.Release();

                mask.width = Screen.width;
                mask.height = Screen.height;

                buffer1.width = Screen.width;
                buffer1.height = Screen.height;

                buffer2.width = Screen.width;
                buffer2.height = Screen.height;

                fogBuffer.width = Screen.width;
                fogBuffer.height = Screen.height;

                dropletBuffer.width = Screen.width;
                dropletBuffer.height = Screen.height;

                dropletMask.width = Screen.width;
                dropletMask.height = Screen.height;
            }*/

            ///<summary>
            ///Set the fog color and the density based on the current depth
            ///Min and max values are obtained from the underwater parameters of the individual waterplane
            /// </summary>
            if (waterPlane != null)
            {
                adjustedFogDensity = Mathf.Lerp(fogDensity, deepFogDensity, (waterPlane.transform.position.y - transform.position.y) / maxFogDepth) * GetComponent<Camera>().farClipPlane / 10;
                adjustedFogColor = Color.Lerp(fogColor, deepFogColor, (waterPlane.transform.position.y - transform.position.y) / maxFogDepth);
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

                Instantiate(bubble, new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y - 0.4f, transform.position.z + Random.Range(-3f, 3f)), Quaternion.identity).transform.SetParent(bubbleContainer.transform);

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
