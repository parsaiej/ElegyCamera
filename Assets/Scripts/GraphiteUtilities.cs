using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct BodyViewData
{
    public GameObject body;
    public Texture2D positionMap;
    public Texture2D normalMap;
    public Texture2D tangentMap;
    public Texture2D cameraDistanceMap;
}

public class GraphiteUtilities {

    static BodyViewData m_BodyData;

    //Meta-data
    public struct TattooTransformationData
    {
        public Vector3 worldPosition;
        public Vector3 worldNormal;
        public Vector3 worldTangent;
        public float cameraDistance;
    };

    public static void SetBodyViewData(BodyViewData _data) { m_BodyData = _data; }
    
    public static TattooTransformationData TransformTattoo(Tattoo _t)
    {
        Vector2 uv = _t.GetTextureCoordinate(); //get the tattoo position uv
        
        Texture2D positionMap       = m_BodyData.positionMap;
        Texture2D normalMap         = m_BodyData.normalMap;
        Texture2D tangentMap        = m_BodyData.tangentMap;
        Texture2D cameraDistanceMap = m_BodyData.cameraDistanceMap;

        Transform surfaceTransform = m_BodyData.body.GetComponent<Transform>();
        Mesh surfaceMesh           = m_BodyData.body.GetComponent<MeshFilter>().sharedMesh;

        //calculate 0..res uv
        float x = (positionMap.width  * uv.x); //note, can just use the position map resolution if all maps are same resolution.
        float y = (positionMap.height * uv.y);

        //sample
        Vector4 localPos     = positionMap.GetPixel((int)x, (int)y);
        Vector4 localNorm    = normalMap.GetPixel((int)x, (int)y);
        Vector4 localTang    = tangentMap. GetPixel((int)x, (int)y);
        float cameraDistance = (cameraDistanceMap.GetPixel((int)x, (int)y)).r;

        //decompress
        localPos  = 2 * localPos  - Vector4.one;
        localNorm = 2 * localNorm - Vector4.one;
        localTang = 2 * localTang - Vector4.one;

        //substance painter + maya use right-handed coordinate system, invert the x.
        localPos.x  = -localPos.x;
        localNorm.x = -localNorm.x;
        localTang.x = -localTang.x;

        //denormalize position with respect to bounding box extents. 
        localPos.x *= surfaceMesh.bounds.extents.x;
        localPos.y *= surfaceMesh.bounds.extents.y;
        localPos.z *= surfaceMesh.bounds.extents.z;

        localPos.x += surfaceMesh.bounds.center.x;
        localPos.y += surfaceMesh.bounds.center.y;
        localPos.z += surfaceMesh.bounds.center.z;


        //final object to world space conversions + return
        TattooTransformationData data;
        data.worldPosition  = surfaceTransform.TransformPoint(localPos);
        data.worldNormal    = surfaceTransform.TransformDirection(localNorm);
        data.worldTangent   = surfaceTransform.TransformDirection(localTang);
        data.cameraDistance = cameraDistance;

        return data;
    }
}
