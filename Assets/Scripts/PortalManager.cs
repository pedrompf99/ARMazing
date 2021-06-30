using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
    public GameObject mainCamera;

    public GameObject virtualWorld;

    private Material[] virtualWorldMaterials;

    public GameObject[] interactableObjects;

    void Start()
    {
        setUpInteractableObjects(false);
        virtualWorldMaterials = virtualWorld.GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < virtualWorldMaterials.Length; i++)
        {
            virtualWorldMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
        }
    }

    
    void OnTriggerStay(Collider collider)
    {
        Vector3 camPositionRelativeToVirtualWorld = transform.InverseTransformPoint(mainCamera.transform.position);
        if (camPositionRelativeToVirtualWorld.y < 0)
        {
            setUpInteractableObjects(true);
            for (int i = 0; i < virtualWorldMaterials.Length; i++)
            {
                virtualWorldMaterials[i].SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            }
        }
        else
        {
            setUpInteractableObjects(false);
            for (int i = 0; i < virtualWorldMaterials.Length; i++)
            {
                virtualWorldMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
        }
    }

    void setUpInteractableObjects(bool insidePortal)
    {
        for(int i = 0; i < interactableObjects.Length; i++)
        {
            interactableObjects[i].GetComponent<ARCubeInteraction>().setMaterialsStencilComp(insidePortal);
        }
    }
}
