using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AQUAS
{
    public class AQUAS_RippleController : MonoBehaviour
    {

        //Objects
        public GameObject body;
        GameObject particleObject;
        GameObject particleObjectMove;

        //Camera
        Camera cam;

        //Particle Systems && their modules
        ParticleSystem particles;
        ParticleSystem.MainModule main;
        ParticleSystem.EmissionModule emission;
        ParticleSystem.ShapeModule shape;
        ParticleSystemRenderer particleRenderer;

        ParticleSystem particlesMove;
        ParticleSystem.MainModule mainMove;
        ParticleSystem.EmissionModule emissionMove;
        ParticleSystem.ShapeModule shapeMove;
        ParticleSystemRenderer particleRendererMove;

        //Data for size-dependent particle params
        float maxBound;

        //Data for speed dependent particle params
        Vector3 lastPosition;
        float speed;

        //Directional data
        Vector2 firstVec;
        Vector2 secondVec;
        float angle;

        //Start values for particle params
        float startLifetime = 2;
        float startSize = 5;
        float rateOverTime = 2;
        float shapeSize = 0.25f;

        //Mesh Data
        Mesh mesh;
        Mesh referenceMesh;

        //Water data
        public float waterLevel;

        //If true, ripples will not be rendered
        bool ceaseRipples;

        //Rendertexture to render to
        RenderTexture target;

        //Waterplane to pass the result to
        public GameObject waterPlane;
        List<Material> waterMatList;
        Material[] waterMat;
        public int index;

        // Use this for initialization
        void Start()
        {

            //Get objects that hold particle systems
            particleObject = transform.Find("Particle System").gameObject;
            particleObjectMove = transform.Find("Particle System Move").gameObject;

            //Get particle components
            particles = particleObject.GetComponent<ParticleSystem>();
            particlesMove = particleObjectMove.GetComponent<ParticleSystem>();

            //Get particle renderer components
            particleRenderer = particleObject.GetComponent<ParticleSystemRenderer>();
            particleRendererMove = particleObjectMove.GetComponent<ParticleSystemRenderer>();

            //Get mesh data from object
            mesh = body.GetComponent<MeshFilter>().mesh;

            //Set up Render Texture
            target = new RenderTexture(1024, 1024, 0, RenderTextureFormat.R8);
            target.wrapMode = TextureWrapMode.Clamp;
            target.filterMode = FilterMode.Bilinear;
            target.anisoLevel = 0;

            //Get camera component and make it render to the render texture
            cam = GetComponent<Camera>();
            cam.targetTexture = target;

            //Get waterplane's materials to pass the result to
            //First add them to a list, then convert the list to an array
            waterMatList = new List<Material>();
            
            for(int j = 0; j < waterPlane.GetComponent<Renderer>().sharedMaterials.Length; j++)
            {
                waterMatList.Add(waterPlane.GetComponent<Renderer>().sharedMaterials[j]);
            }

            waterMat = waterMatList.ToArray();

            //Get the water level from the waterplane's position
            waterLevel = waterPlane.transform.position.y;

            //Objects are temporary and not to be saved
            particleObject.hideFlags = HideFlags.HideAndDontSave;
            particleObjectMove.hideFlags = HideFlags.HideAndDontSave;

            //Don't know why but HideAndDontSave will cause exceptions - HideInHierarchy works, but bears that risk that it is being serialized at some point.
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private void OnPreCull()
        {
            if(body == null)
            {
                DestroyImmediate(this.gameObject);
            }

            //Cache particle modules from both particle systems
            //Unfortunately this has to be done every frame.
            main = particles.main;
            emission = particles.emission;
            shape = particles.shape;

            mainMove = particlesMove.main;
            emissionMove = particlesMove.emission;
            shapeMove = particlesMove.shape;

            //Enable Particle renderers before rendering
            particleRenderer.enabled = true;
            particleRendererMove.enabled = true;

            //Calculate speed based on the distance between the current position and the position in the last frame
            // Old Position
            lastPosition = transform.position;

            //New Position
            transform.position = new Vector3(body.transform.position.x, -10000, body.transform.position.z);

            speed = Vector3.Distance(transform.position, lastPosition) * 1 / Time.deltaTime;


            //Get Movement Direction
            firstVec = new Vector2(0, 1);
            secondVec = new Vector2(transform.position.x - lastPosition.x, transform.position.z - lastPosition.z);

            angle = -Vector2.SignedAngle(firstVec, secondVec);


            //Size-dependent emission values

            //Don't create reference mesh until camera size is figured out
            //referenceMesh = getRelevantMesh(mesh, underwaterObject);

            //Get mesh's extents for calculating size-dependent emission values
            maxBound = Mathf.Max(mesh.bounds.extents.x * body.transform.localScale.x, mesh.bounds.extents.z * body.transform.localScale.z);

            //Adjust camera's orthographic size based on the mesh's extents, to make sure, ripples are not drawn outside the camera's view.
            cam.orthographicSize = 30 * maxBound;

            //Size-dependent emission parameters
            startLifetime = Mathf.Sqrt(3 * maxBound);
            startSize = 7 * maxBound;
            rateOverTime = 2 / maxBound;
            shapeSize = maxBound / 2;


            //Speed-dependent emission values
            if (speed > 0)
            {
                startLifetime /= Mathf.Max(Mathf.Sqrt(2 * maxBound), speed / 10);
                rateOverTime *= speed * 1.5f;

                main.startLifetimeMultiplier = startLifetime;
                main.startSizeMultiplier = startSize;

                mainMove.startLifetimeMultiplier = startLifetime;
                mainMove.startSizeMultiplier = startSize;

                mainMove.startRotation = Mathf.Deg2Rad * angle;
                emissionMove.rateOverTimeMultiplier = rateOverTime;
                emission.rateOverTimeMultiplier = 0;
            }
            else
            {
                main.startLifetimeMultiplier = startLifetime;
                main.startSizeMultiplier = startSize;

                mainMove.startLifetimeMultiplier = startLifetime;
                mainMove.startSizeMultiplier = startSize;

                mainMove.startRotation = 0;
                emissionMove.rateOverTimeMultiplier = 0;
                emission.rateOverTimeMultiplier = rateOverTime;
            }

            shape.radius = shapeSize;
            shapeMove.radius = shapeSize;

            //Check if the volume is intersecting with the water surface and stop emitting particles, if not
            if(body.transform.position.y - mesh.bounds.extents.y < waterLevel && body.transform.position.y + mesh.bounds.extents.y > waterLevel)
            {
                ceaseRipples = false;
            }
            else
            {
                ceaseRipples = true;
            }
            
            if (ceaseRipples)
            {
                emission.rateOverTimeMultiplier = 0;
                emissionMove.rateOverTimeMultiplier = 0;
            }
        }

        private void OnPostRender()
        {
            //Pass results and parameters to water material
            foreach(Material mat in waterMat)
            {
                mat.SetTexture("_RippleTex" + index.ToString(), target);
                mat.SetFloat("_XOffset" + index.ToString(), -transform.position.x);
                mat.SetFloat("_ZOffset" + index.ToString(), -transform.position.z);
                mat.SetFloat("_Scale" + index.ToString(), cam.orthographicSize * 2);
            }

            //Disable particle renderers after rendering, so that other cameras won't capture them as well
            particleRenderer.enabled = false;
            particleRendererMove.enabled = false;
        }
        
        //Get part of the mesh that it either above or below the water surface (not used in this version)
        Mesh getRelevantMesh(Mesh inputMesh, bool underwaterObject)
        {
            Vector3[] vertices = inputMesh.vertices;
            List<Vector3> targetVerts = new List<Vector3>();

            if (underwaterObject)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (body.transform.TransformPoint(vertices[i]).y >= waterLevel)
                    {
                        targetVerts.Add(vertices[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (body.transform.TransformPoint(vertices[i]).y <= waterLevel)
                    {
                        targetVerts.Add(vertices[i]);
                    }
                }
            }

            Mesh output = new Mesh();
            output.vertices = targetVerts.ToArray();

            return output;
        }
    }
}
