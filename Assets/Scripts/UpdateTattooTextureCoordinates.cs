using UnityEngine;
using System.Collections;

public class UpdateTattooTextureCoordinates : MonoBehaviour {

    private Camera m_TattooCamera;
    private Tattoo[] m_Tattoos;

    void Start()
    {
        m_TattooCamera = GetComponent<Camera>();
        m_Tattoos = Object.FindObjectsOfType<Tattoo>();
    }

    void Update() 
    {
        //update the tattoo texture coordinates with their viewport position
        foreach (Tattoo tattoo in m_Tattoos)
        {
            tattoo.SetTextureCoordinate(m_TattooCamera.WorldToViewportPoint(tattoo.transform.position));
        }
    }

}
