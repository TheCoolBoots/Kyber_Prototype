using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AQUAS
{
    public class AQUAS_ProjectedGrid : MonoBehaviour
    {
        //The waterplane, its parent object, its children and the AQUAS_Caustics components on the children
        public GameObject waterplane;
        GameObject ContainerObj;
        public Transform[] children;
        AQUAS_Caustics[] causticComponents;

        //This camera and a projector
        Camera cam;
        GameObject projectorObj;
        Camera projector;

        //Arrays holding the camera frustum's corner points
        Vector3[] farFrustumCorners;
        Vector3[] nearFrustumCorners;

        //Projector's frustum corners only needed for debugging
        Vector3[] pFarFrustumCorners;
        Vector3[] pNearFrustumCorners;

        //Materials currently on the waterplane
        Material[] materials;
        
        public float waterLevel;

        //This will be filled with the intersection points between the camera's frustum and the displacable water volume
        List<Vector3> intersectionPoints;

        //Range conversion vector
        Vector4 minMax;

        // Use this for initialization
        void Start()
        {
            //Get the waterplane's children and the AQUAS_Caustics components
            children = new Transform[waterplane.transform.childCount];

            List<AQUAS_Caustics> causticList = new List<AQUAS_Caustics>();

            for (int i = 0; i < waterplane.transform.childCount; i++)
            {
                children[i] = waterplane.transform.GetChild(i);

                if (waterplane.transform.GetChild(i).GetComponent<AQUAS_Caustics>() != null)
                {
                    causticList.Add(waterplane.transform.GetChild(i).GetComponent<AQUAS_Caustics>());
                }
            }     
            
            foreach(Transform trans in children)
            {
                if (trans.name == "Static Boundary")
                {
                    trans.parent = null;
                }
                if(trans.GetComponent<AQUAS_Caustics>() != null)
                {
                    trans.parent = null;
                }
            }     
            
            causticComponents = causticList.ToArray();
            
            cam = GetComponent<Camera>();

            //Replace the Mesh renderer with a Skinned mesh renderer to manipulate the bounds
            // This is helpful in scene view, because the scene view camera won't see the displaced mesh, if the undisplaced mesh is outside of the camera's frustum
            materials = waterplane.GetComponent<SkinnedMeshRenderer>().sharedMaterials;

            //Initialise frustum corner arrays
            nearFrustumCorners = new Vector3[4];
            farFrustumCorners = new Vector3[4];

            //Projector's frustum corners only needed for debugging
            pFarFrustumCorners = new Vector3[4];
            pNearFrustumCorners = new Vector3[4];

            //Get the water level from the waterplane
            //The waterplane will be in constant motion from now on the water level parameter will replace the waterplane's y-position
            waterLevel = waterplane.transform.position.y;

            //Set up the projector object
            projectorObj = new GameObject("Projector Object " + "(" + transform.name + ")");
            projectorObj.hideFlags = HideFlags.HideAndDontSave;
            projector = projectorObj.AddComponent<Camera>();
            projector.CopyFrom(cam);
            projector.depth = cam.depth - 1;
            projector.enabled = false;
            
            //Initialise the intersection points list
            intersectionPoints = new List<Vector3>();

            //Get the waterplane's original parent and only unparent it before rendering - parent it back, when rendering is done!
            ContainerObj = waterplane.transform.parent.gameObject;

            //Pass the water level to the underwater effects component should there be one - it will pass it on to its masks
            //This is neccessary, because the masks would normally read the water level from the waterplane's y-position, which is wrong when using grid projection
            if(GetComponent<AQUAS_UnderWaterEffect_Simple>() != null)
            {
                GetComponent<AQUAS_UnderWaterEffect_Simple>().waterLevel = waterLevel;
                foreach (Transform trans in children)
                {
                    if (trans.name == "Static Boundary" && GetComponent<AQUAS_UnderWaterEffect>() != null)
                    {
                        GetComponent<AQUAS_UnderWaterEffect>().dynamicBoundary = trans.gameObject;
                    }
                }
            }

            if(GetComponent<AQUAS_UnderWaterEffect>() != null)
            {
                GetComponent<AQUAS_UnderWaterEffect>().waterLevel = waterLevel;

                foreach(Transform trans in children)
                {
                    if(trans.name == "Static Boundary")
                    {
                        GetComponent<AQUAS_UnderWaterEffect>().dynamicBoundary = trans.gameObject;
                    }
                }
            }
        }

        private void Update()
        {
            //For some reason sometimes the projector object is not being created - if it doesn't exist, Start() runs over.
            if (!projectorObj)
            {
                Start();
            }

            //Make sure the projector is always slightly behind the camera, and in any case at least 3 m above the water surface
            Vector3 projectorOffset = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -1)); ;
            projector.transform.position = new Vector3(projectorOffset.x, Mathf.Max(waterLevel + Mathf.Abs(waterLevel - cam.transform.position.y), waterLevel + 3), projectorOffset.z);

            //Readjust the waterlevel on the caustics components and prevent them from reading it from the waterplane's y-position
            foreach (AQUAS_Caustics caustic in causticComponents)
            {
                caustic.waterLevel = waterLevel;
                caustic.overrideWaterLevel = true;
            }

            //Get the camera's frustum corners and pass them to their arrays
            cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, farFrustumCorners);
            cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), cam.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, nearFrustumCorners);

            //Do the same for the projector's frustum corners - this is for debugging ony, we don't really need that
            projector.CalculateFrustumCorners(new Rect(0, 0, 1, 1), projector.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, pFarFrustumCorners);
            projector.CalculateFrustumCorners(new Rect(0, 0, 1, 1), projector.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, pNearFrustumCorners);

            //Transform the frustum corners from local space to world space
            for (int i = 0; i < 4; i++)
            {
                farFrustumCorners[i] = transform.TransformPoint(farFrustumCorners[i]);
                nearFrustumCorners[i] = transform.TransformPoint(nearFrustumCorners[i]);

                pFarFrustumCorners[i] = projectorObj.transform.TransformPoint(pFarFrustumCorners[i]);
                pNearFrustumCorners[i] = projectorObj.transform.TransformPoint(pNearFrustumCorners[i]);
            }

            //Clear the list that is holding the intersection points and get the frustum's intersection points with the displacable water volume
            intersectionPoints.Clear();
            GetAllIntersectionPoints(waterLevel);
            GetAllIntersectionPoints(waterLevel + 1);
            GetAllIntersectionPoints(waterLevel - 1);

            //Adjust the projector to always match up with the camera
            projector.aspect = cam.aspect;
            projector.fieldOfView = cam.fieldOfView;

            //Get the point the projector is supposed to look at for a convenient projection
            projector.transform.LookAt(AimProjector());
            
        }

        private void OnPreCull()
        {
            //Adjust the child object's scale and position to line up as if the water was a simple plane
            foreach (Transform trans in children)
            {
                if(trans == null)
                {
                    continue;
                }

                if (trans.name == "PrimaryCausticsProjector" || trans.name == "SecondaryCausticsProjector")
                {
                    trans.position = new Vector3(transform.position.x, waterLevel, transform.position.z);
                    trans.eulerAngles = new Vector3(90, 0, 0);
                }
                else
                {
                    trans.position = new Vector3(transform.position.x, waterLevel, transform.position.z);

                    trans.eulerAngles = new Vector3(0, 0, 0);
                    trans.localScale = new Vector3(cam.farClipPlane / 2, cam.farClipPlane / 2, cam.farClipPlane / 2);
                }
            }

            //Position the waterplane in front of the camera
            PositionPlane();
            
            //Get the range conversion vector
            minMax = GetBordersInViewport();

            //Get the near clipping plane's scale to correctly scale the waterplane
            float scaleX = Vector3.Distance(pNearFrustumCorners[0], pNearFrustumCorners[3]);
            float scaleZ = Vector3.Distance(pNearFrustumCorners[0], pNearFrustumCorners[1]);
            
            //Pass the near clipping plane's scale, the range conversion vector and the waterlevel to the shader and enable grid projection on the shader
            foreach (Material mat in materials)
            {
                mat.SetVector("_ObjectScale", new Vector2(scaleX / projector.nearClipPlane, scaleZ / projector.nearClipPlane));
                mat.SetVector("_RangeVector", minMax);
                mat.SetFloat("_waterLevel", waterLevel);
                mat.SetFloat("_ProjectGrid", 1);

                //This only needs to be passed to shaders that actually render on the main camera
                if (mat.HasProperty("_PhysicalNormalStrength"))
                {
                    mat.SetFloat("_PhysicalNormalStrength", 1);
                }
            }
        }

        private void OnPostRender()
        {
            //Parent the waterplane back to its original parent object
            waterplane.transform.parent = ContainerObj.transform;
        }

        //This method positions the waterplane in front of the camera's near clipping plane
        void PositionPlane()
        {
            //Set position and rotation
            waterplane.transform.position = projector.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, projector.nearClipPlane));
            waterplane.transform.eulerAngles = new Vector3(projector.transform.eulerAngles.x - 90, projector.transform.eulerAngles.y, projector.transform.eulerAngles.z);

            //Parent the waterplane to the projector object - this will be reverted after rendering
            waterplane.transform.parent = projectorObj.transform;
        }

        //Get all intersection points between the camera's frustum and the waterplane
        void GetAllIntersectionPoints(float level)
        {
            Vector3 origin = new Vector3(0, level, 0);

            for (int i = 0; i < 4; i++)
            {
                int j;

                if (i == 3)
                {
                    j = 0;
                }
                else
                {
                    j = i + 1;
                }

                if (LinePlaneIntersection(nearFrustumCorners[i], nearFrustumCorners[j], origin))
                {
                    intersectionPoints.Add(GetIntersectionPoint(nearFrustumCorners[i], nearFrustumCorners[j], origin));
                }

                if (LinePlaneIntersection(farFrustumCorners[i], farFrustumCorners[j], origin))
                {
                    intersectionPoints.Add(GetIntersectionPoint(farFrustumCorners[i], farFrustumCorners[j], origin));
                }

                if (LinePlaneIntersection(nearFrustumCorners[i], farFrustumCorners[i], origin))
                {
                    intersectionPoints.Add(GetIntersectionPoint(nearFrustumCorners[i], farFrustumCorners[i], origin));
                }
            }
        }

        //Check if a line between to points intersects with a plane that has a given origin
        bool LinePlaneIntersection(Vector3 startPoint, Vector3 targetPoint, Vector3 planeOrigin)
        {
            Vector3 normal = Vector3.up;

            float distV1 = Vector3.Dot(normal, startPoint - planeOrigin);
            float distV2 = Vector3.Dot(normal, targetPoint - planeOrigin);

            if (distV1 * distV2 <= 0)
            {
                return true;
            }

            return false;
        }

        //Get the intersection point between a line and a plane
        Vector3 GetIntersectionPoint(Vector3 startPoint, Vector3 targetPoint, Vector3 planeOrigin)
        {
            Vector3 normal = Vector3.up;

            float distV2 = Vector3.Dot(normal, targetPoint - planeOrigin);

            Vector3 x = (targetPoint - startPoint) / Vector3.Magnitude(targetPoint - startPoint);

            float cosPhi = Vector3.Dot(normal, x);

            return targetPoint - (x * (distV2 / cosPhi));
        }

        //This is used to specify where the projector should look
        //It's rather simple as long as projector and camera are in the same position, but since the projector must stay outside the displacable water volume, it can
        //be tricky to aim the projector such that the result is the same as if the projector had the same position as the camera. - This is still a weak spot.
        Vector3 AimProjector()
        {
            //Initialise the point to aim at
            Vector3 aimAt = Vector3.zero;

            //Define 2 states - one in which the camera view intersects with the waterplane and one in which it doesn't select the aiming point for the projector based on that
            if (LinePlaneIntersection(cam.transform.position, cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.farClipPlane)), new Vector3(0, waterLevel, 0)))
            {
                Vector3 startPoint;
                Vector3 horizon = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.farClipPlane));

                //Here calculate the aiming point based on whether the camers is below, above or right on the water level
                if (cam.transform.position.y > waterLevel)
                {
                    startPoint = new Vector3(cam.transform.position.x, Mathf.Max(cam.transform.position.y, waterLevel + 3), cam.transform.position.z);
                    aimAt = GetIntersectionPoint(startPoint, cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.farClipPlane)), new Vector3(0, waterLevel, 0));
                }
                else if(cam.transform.position.y < waterLevel)
                {
                    startPoint = new Vector3(cam.transform.position.x, Mathf.Min(cam.transform.position.y, waterLevel - 3), cam.transform.position.z);
                    aimAt = GetIntersectionPoint(startPoint, cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.farClipPlane)), new Vector3(0, waterLevel, 0));
                }
                else
                {
                    horizon = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.farClipPlane));
                    aimAt = new Vector3(horizon.x, waterLevel, horizon.z);
                }

                //aimAt = GetIntersectionPoint(startPoint, cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.farClipPlane)), new Vector3(0, waterLevel, 0));
            }
            else
            {
                Vector3 horizon = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.farClipPlane));
                aimAt = new Vector3(horizon.x, -Mathf.Abs(horizon.y), horizon.z);
            }

            /*for (int i = 0; i < intersectionPoints.Count; i++)
            {
                aimAt += intersectionPoints[i];
            }
            aimAt /= intersectionPoints.Count;*/

            return aimAt;
        }

        //Calculate range conversion vector in viewport coordinates to later pass them to the shader
        Vector4 GetBordersInViewport()
        {
            float xMin = 1;
            float xMax = 0;
            float yMin = 1;
            float yMax = 0;

            for (int i = 0; i < intersectionPoints.Count; i++)
            {
                if (projector.WorldToViewportPoint(intersectionPoints[i]).x < xMin)
                {
                    xMin = projector.WorldToViewportPoint(intersectionPoints[i]).x;
                }
            }

            for (int i = 0; i < intersectionPoints.Count; i++)
            {
                if (projector.WorldToViewportPoint(intersectionPoints[i]).x > xMax)
                {
                    xMax = projector.WorldToViewportPoint(intersectionPoints[i]).x;
                }
            }

            for (int i = 0; i < intersectionPoints.Count; i++)
            {
                if (projector.WorldToViewportPoint(intersectionPoints[i]).y < yMin)
                {
                    yMin = projector.WorldToViewportPoint(intersectionPoints[i]).y;
                }
            }

            for (int i = 0; i < intersectionPoints.Count; i++)
            {
                if (projector.WorldToViewportPoint(intersectionPoints[i]).y > yMax)
                {
                    yMax = projector.WorldToViewportPoint(intersectionPoints[i]).y;
                }
            }

            return new Vector4((xMin-0.5f) * 1.1f, yMin-0.5f, (xMax-0.5f) * 1.1f, yMax-0.5f);
        }

        //Only for debugging
        /*private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                /*for (int i = 0; i < intersectionPoints.Count; i++)
                {
                    Gizmos.DrawSphere(new Vector3(intersectionPoints[i].x, waterLevel, intersectionPoints[i].z), 5f);
                }

                //Gizmos.DrawSphere(AimProjector(), 5f);

                //=================DEBUG==================///

                //Camera's Frustum
                Debug.DrawLine(farFrustumCorners[0], farFrustumCorners[1], Color.blue);
                Debug.DrawLine(farFrustumCorners[1], farFrustumCorners[2], Color.blue);
                Debug.DrawLine(farFrustumCorners[2], farFrustumCorners[3], Color.blue);
                Debug.DrawLine(farFrustumCorners[3], farFrustumCorners[0], Color.blue);

                Debug.DrawLine(nearFrustumCorners[0], nearFrustumCorners[1], Color.blue);
                Debug.DrawLine(nearFrustumCorners[1], nearFrustumCorners[2], Color.blue);
                Debug.DrawLine(nearFrustumCorners[2], nearFrustumCorners[3], Color.blue);
                Debug.DrawLine(nearFrustumCorners[3], nearFrustumCorners[0], Color.blue);

                Debug.DrawLine(nearFrustumCorners[0], farFrustumCorners[0], Color.blue);
                Debug.DrawLine(nearFrustumCorners[1], farFrustumCorners[1], Color.blue);
                Debug.DrawLine(nearFrustumCorners[2], farFrustumCorners[2], Color.blue);
                Debug.DrawLine(nearFrustumCorners[3], farFrustumCorners[3], Color.blue);


                //Projector's Frustum
                Debug.DrawLine(pFarFrustumCorners[0], pFarFrustumCorners[1], Color.red);
                Debug.DrawLine(pFarFrustumCorners[1], pFarFrustumCorners[2], Color.red);
                Debug.DrawLine(pFarFrustumCorners[2], pFarFrustumCorners[3], Color.red);
                Debug.DrawLine(pFarFrustumCorners[3], pFarFrustumCorners[0], Color.red);

                Debug.DrawLine(pNearFrustumCorners[0], pNearFrustumCorners[1], Color.red);
                Debug.DrawLine(pNearFrustumCorners[1], pNearFrustumCorners[2], Color.red);
                Debug.DrawLine(pNearFrustumCorners[2], pNearFrustumCorners[3], Color.red);
                Debug.DrawLine(pNearFrustumCorners[3], pNearFrustumCorners[0], Color.red);

                Debug.DrawLine(pNearFrustumCorners[0], pFarFrustumCorners[0], Color.red);
                Debug.DrawLine(pNearFrustumCorners[1], pFarFrustumCorners[1], Color.red);
                Debug.DrawLine(pNearFrustumCorners[2], pFarFrustumCorners[2], Color.red);
                Debug.DrawLine(pNearFrustumCorners[3], pFarFrustumCorners[3], Color.red);

                //=================DEBUG==================///
            }
        }*/
    }
}
