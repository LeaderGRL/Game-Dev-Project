using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Renderer plane;

    public void DrawTexture(Texture2D texture)
    {
      
        
        plane.sharedMaterial.mainTexture = texture;
        plane.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
}