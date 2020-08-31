using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

/// <summary>
/// GraphicRaycaster which only accepts input events that have a certain hash value
/// </summary>
[ExecuteInEditMode]
public class CurvedUICanvas : GraphicRaycaster
{
    private int pointerEventDataHashMask;
    public override Camera eventCamera
    {
        get
        {
            return GetComponent<Canvas>().worldCamera;
        }
    }
    public void setPointerEventDataHashMask(int h)
    {
        pointerEventDataHashMask = h;
    }

    public bool InputPossible
    {
        get;
        set;
    }


    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        if (eventData.GetHashCode() == pointerEventDataHashMask && InputPossible)
        {

            base.Raycast(eventData, resultAppendList);
        }
    }
    protected override void OnDestroy()
    {
        if (Event.current != null && (Event.current.commandName == "SoftDelete" || Event.current.commandName == "Delete"))
        {
#if UNITY_EDITOR
            //destroy rendertexture,  camera
            AssetDatabase.DeleteAsset("Assets/SofaUnity/UI/RenderTexturesAndMaterials/" + eventCamera.targetTexture.name + ".renderTexture");
            DestroyImmediate(eventCamera);
#endif
        }
        base.OnDestroy();
    }
}
