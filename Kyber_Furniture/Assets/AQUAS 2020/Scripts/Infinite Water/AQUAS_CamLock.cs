using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AQUAS
{
    [AddComponentMenu("AQUAS/Essentials/Camera Lock")]

    //This Class locks the horizontal position of the waterplane to the camera's position
    //The waterplane can be scaled to fill the camera frustum (optional)
    public class AQUAS_CamLock : MonoBehaviour
    {
        public GameObject mainCamera;
        public bool scaleToFrustum;

        [Header("Settings for dynamic meshes")]
        public bool useDynamicMesh;
        [Range(32, 256)]
        public int resolution = 64;

        [HideInInspector]
        public Camera[] cameras;
        [HideInInspector]
        public Camera[] sceneCameras;

        // Use this for initialization
        void Start()
        {
            if (useDynamicMesh)
            {
#if UNITY_EDITOR

                if (GetComponent<Renderer>().sharedMaterials[0].shader.name != "AQUAS/Desktop/Front/Default" && GetComponent<Renderer>().sharedMaterials[0].shader.name != "Hidden/AQUAS/Desktop/Front/Opaque")
                {
                    if (GetComponent<Renderer>().sharedMaterials[0].shader.name == "AQUAS/Desktop/Front/Shallow")
                    {
                        EditorUtility.DisplayDialog("Unsupported Shader!", "AQUAS is currently using a shader for shallow water. This shader does not support dynamic meshes - Aborting grid projection.", "OK");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Unsupported Shader", "The shader in use on the waterplane does not support dynamic meshes - Aborting grid projection", "OK");
                    }
                    return;
                }
#endif

                transform.localScale = new Vector3(1, 1, 1);

                GetComponent<MeshFilter>().mesh = CreateMesh();

                Material[] materials = GetComponent<Renderer>().sharedMaterials;

                gameObject.AddComponent<SkinnedMeshRenderer>();
                GetComponent<SkinnedMeshRenderer>().localBounds = new Bounds(Vector3.zero, new Vector3(1000000000, 1000000000, 1000000000));
                GetComponent<SkinnedMeshRenderer>().sharedMaterials = materials;

                GetComponent<MeshFilter>().sharedMesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(1000000000, 1000000000, 1000000000));

                GetComponent<AQUAS_Reflection>().overrideWaterLevel = true;
                GetComponent<AQUAS_Reflection>().waterLevel = transform.position.y;

                mainCamera.AddComponent<AQUAS_ProjectedGrid>();
                mainCamera.GetComponent<AQUAS_ProjectedGrid>().hideFlags = HideFlags.HideInInspector;
                mainCamera.GetComponent<AQUAS_ProjectedGrid>().waterplane = gameObject;

                return;
            }

            if (mainCamera == null)
            {
                mainCamera = Camera.main.transform.gameObject;
            }

            if (scaleToFrustum)
            {
                transform.localScale = new Vector3(mainCamera.GetComponent<Camera>().farClipPlane*2f/10, 1, mainCamera.GetComponent<Camera>().farClipPlane*2f/10);
                Vector3 boundaryScale = transform.Find("Static Boundary").localScale;
                transform.Find("Static Boundary").localScale = new Vector3(5, boundaryScale.y, 5);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (useDynamicMesh)
            {
                /*if (Camera.allCameras.Length != cameras.Length)
                {
                    GetCameras();
                    HookUpCameras();
                }*/

#if UNITY_EDITOR
                /*if (SceneView.GetAllSceneCameras().Length != sceneCameras.Length)
                {
                    GetSceneCameras();
                    HookUpSceneCameras();
                }*/
#endif
                return;
            }

            transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z);
        }
        Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[resolution * resolution];
            Vector2[] uvs = new Vector2[resolution * resolution];
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

            float incrementX = 1 / (float)(resolution - 1);
            float incrementY = 1 / (float)(resolution - 1);

            for (int i = 0; i < resolution; i++)
            {
                float x = incrementX * i - 0.5f;

                for (int j = 0; j < resolution; j++)
                {
                    float y = incrementY * j - 0.5f;

                    vertices[i * resolution + j] = new Vector3(x, 0, y);
                    uvs[i * resolution + j] = new Vector2(x, y);
                }
            }

            for (int i = 0; i < (resolution - 1) * (resolution - 1); i++)
            {
                int column = (int)Mathf.Floor(i / (resolution - 1));

                triangles[i * 6] = column * (resolution) + i % (resolution - 1);
                triangles[i * 6 + 1] = column * (resolution) + i % (resolution - 1) + 1;
                triangles[i * 6 + 2] = (column + 1) * (resolution) + i % (resolution - 1);
                triangles[i * 6 + 3] = (column + 1) * (resolution) + i % (resolution - 1);
                triangles[i * 6 + 4] = column * (resolution) + i % (resolution - 1) + 1;
                triangles[i * 6 + 5] = (column + 1) * (resolution) + i % (resolution - 1) + 1;
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        void GetCameras()
        {
            cameras = Camera.allCameras;
        }

#if UNITY_EDITOR
        void GetSceneCameras()
        {
            sceneCameras = SceneView.GetAllSceneCameras();
        }
#endif

        void HookUpCameras()
        {
            foreach (Camera camera in cameras)
            {
                if (camera.gameObject.GetComponent<AQUAS_ProjectedGrid>() == null && camera.transform.parent == null)
                {
#if UNITY_EDITOR
                    /*if (camera.GetComponent<AQUAS_UnderWaterEffect>() == null)
                    {
                        EditorUtility.DisplayDialog("No Advanced Underwater Effects", "No AQUAS_UnderwaterEffects Component detected on " + camera.name + ". " + camera.name + " won't benefit from the dynamic mesh.", "OK");
                    }*/

                    if (GetComponent<Renderer>().sharedMaterials[0].shader.name != "AQUAS/Desktop/Front/Default" && GetComponent<Renderer>().sharedMaterials[0].shader.name != "Hidden/AQUAS/Desktop/Front/Opaque")
                    {
                        if (GetComponent<Renderer>().sharedMaterials[0].shader.name == "AQUAS/Desktop/Front/Shallow")
                        {
                            EditorUtility.DisplayDialog("Unsupported Shader!", "AQUAS is currently using a shader for shallow water. This shader does not support dynamic meshes - Aborting grid projection.", "OK");
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Unsupported Shader", "The shader in use on the waterplane does not support dynamic meshes - Aborting grid projection", "OK");
                        }
                        return;
                    }
#endif
                    camera.gameObject.AddComponent<AQUAS_ProjectedGrid>();
                    camera.gameObject.GetComponent<AQUAS_ProjectedGrid>().waterplane = gameObject;
                    camera.gameObject.GetComponent<AQUAS_ProjectedGrid>().hideFlags = HideFlags.HideInInspector;
                    //camera.gameObject.GetComponent<AQUAS_ProjectedGrid>().hideFlags = HideFlags.HideAndDontSave;
                }
            }
        }

        void HookUpSceneCameras()
        {
            foreach (Camera camera in sceneCameras)
            {
                if (camera.gameObject.GetComponent<AQUAS_ProjectedGrid>() == null)
                {
                    camera.gameObject.AddComponent<AQUAS_ProjectedGrid>();
                    camera.gameObject.GetComponent<AQUAS_ProjectedGrid>().waterplane = gameObject;
                    camera.gameObject.GetComponent<AQUAS_ProjectedGrid>().hideFlags = HideFlags.HideAndDontSave;
                    //camera.gameObject.GetComponent<AQUAS_ProjectedGrid>().hideFlags = HideFlags.HideAndDontSave;
                }
            }
        }
    }
}