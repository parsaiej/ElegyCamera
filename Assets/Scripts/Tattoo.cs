using UnityEngine;
using System.Collections;

public class Tattoo : MonoBehaviour {

    protected Vector2 m_TextureCoordinate;
    public void SetTextureCoordinate(Vector2 _uv) { m_TextureCoordinate = _uv; }
    public Vector2 GetTextureCoordinate()         { return m_TextureCoordinate; }
}
