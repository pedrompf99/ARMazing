using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PortalSpawner : MonoBehaviour
{
    public GameObject portal;
    public GameObject placementIndicator;
    private ARReferencePointManager aRReferencePointManager;
    private List<ARReferencePoint> referencePoints = new List<ARReferencePoint>();


    private ARRaycastManager rayManager;
    private bool canPlacePortal = true;
    void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
        aRReferencePointManager = FindObjectOfType<ARReferencePointManager>();
    }


    // Update is called once per frame
    void Update()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        rayManager.Raycast(screenCenter, hits, TrackableType.Planes);

        if (canPlacePortal)
        {
            if (hits.Count > 0)
            {
                placementIndicator.SetActive(true);

                placementIndicator.transform.position = hits[0].pose.position;
                placementIndicator.transform.rotation = hits[0].pose.rotation;


                Touch touch;
                if (Input.touchCount > 0 || (touch = Input.GetTouch(0)).phase == TouchPhase.Began)
                {
                    portal.SetActive(true);
                    Pose location = hits[0].pose;
                    

                    ARReferencePoint referencePoint = aRReferencePointManager.AddReferencePoint(location);
                    

                    if (referencePoint != null)
                    {
                        referencePoints.Add(referencePoint);
                        portal.transform.position = hits[0].pose.position;
                        portal.transform.rotation = hits[0].pose.rotation;
                        portal.transform.eulerAngles = new Vector3(hits[0].pose.rotation.x, hits[0].pose.rotation.y + 180, hits[0].pose.rotation.z);
                        portal.transform.SetParent(referencePoint.transform);
                        
                    }

                    canPlacePortal = false;
                    placementIndicator.SetActive(false);
                }
            }
        }

    }


}
