using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

    public float m_Rotation;

    void OnTriggerEnter2D(Collider2D o)
    {
        Camera.main.GetComponent<LookAtTattoo>().SetCameraRotation(m_Rotation);
    }

    void OnTriggerExit2D(Collider2D o)
    {
        Camera.main.GetComponent<LookAtTattoo>().SetCameraRotation(0.0f);
    }

}
