using UnityEngine;
using System.Collections;

public class LookAtTattoo : MonoBehaviour {

    public Tattoo       m_ObservableTattoo;
    public BodyViewData m_Body;

    public float m_CameraMovementDrag;
    public float m_CameraOffset;
    public float m_CameraScale;
    
    private Vector3 m_LastTattooPosition;
    private Vector3 m_LastTattooNormal;
    private Vector3 m_LastTattooTangent;
    private float   m_LastCameraDistance;

    [Range(0, 360)]
    public float m_CameraRotation;

    public void SetCameraRotation(float _r) { m_CameraRotation = _r; }

    void Start()
    {
        //set the data for the tattoo'd body
        GraphiteUtilities.SetBodyViewData(m_Body);
    }

    void Update()
    {
        UpdateCameraLookAt();
    }

    void UpdateCameraLookAt()
    {
        GraphiteUtilities.TattooTransformationData info = GraphiteUtilities.TransformTattoo(m_ObservableTattoo);

        //smooth
        Vector3 tattooWP = Vector3.Lerp(m_LastTattooPosition, info.worldPosition, m_CameraMovementDrag);
        Vector3 tattooWN = Vector3.Lerp(m_LastTattooNormal,   info.worldNormal,   m_CameraMovementDrag);
        Vector3 tattooWT = Vector3.Lerp(m_LastTattooTangent, Quaternion.AngleAxis(m_CameraRotation, tattooWN) * info.worldTangent,  m_CameraMovementDrag);
        float cameraDistance = m_CameraOffset + Mathf.Lerp(m_LastCameraDistance, (info.cameraDistance * m_CameraScale), 0.040f);

        //move camera transform
        Camera.main.transform.position = tattooWP + (cameraDistance * tattooWN.normalized);
        Camera.main.transform.LookAt(tattooWP, tattooWT);

        //cache last values for smoothing
        m_LastTattooPosition = tattooWP;
        m_LastTattooNormal   = tattooWN;
        m_LastTattooTangent  = tattooWT;
        m_LastCameraDistance = cameraDistance;
    }
}
