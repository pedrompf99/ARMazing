using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ARCubeInteraction : MonoBehaviour
{
    public Material touchingMaterial, holdingMaterial;
    private Material[] startingMaterials;
    private Material[] newMaterials;

    void Start()
    {
        startingMaterials = transform.GetComponent<MeshRenderer>().materials;
        newMaterials = new Material[startingMaterials.Length];
    }

    private void OnTriggerStay(Collider other)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == ManoGestureContinuous.CLOSED_HAND_GESTURE)
        {
            newMaterials[0] = holdingMaterial;
            transform.GetComponent<MeshRenderer>().materials = newMaterials;
            transform.parent = other.gameObject.transform;
        }
        else
        {
            newMaterials[0] = touchingMaterial;
            transform.GetComponent<MeshRenderer>().materials = newMaterials;
            transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        newMaterials[0] = touchingMaterial;
        transform.GetComponent<MeshRenderer>().materials = newMaterials;
    }

    private void OnTriggerExit(Collider other)
    {
        transform.GetComponent<MeshRenderer>().materials = startingMaterials;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
    public void setMaterialsStencilComp(bool insidePortal)
    {
        if (insidePortal)
        {
            for (int i = 0; i < transform.GetComponent<MeshRenderer>().materials.Length; i++)
            {
                transform.GetComponent<MeshRenderer>().materials[i].SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            }

        }
        else
        {
            for (int i = 0; i < transform.GetComponent<MeshRenderer>().materials.Length; i++)
            {
                transform.GetComponent<MeshRenderer>().materials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
        }
    }
}