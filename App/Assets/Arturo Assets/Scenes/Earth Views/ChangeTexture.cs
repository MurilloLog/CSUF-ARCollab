using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTexture : MonoBehaviour
{
    [SerializeField]
    private GameObject sphere;

    [SerializeField]
    private Texture[] textures;

    private Renderer sphereRenderer;

    private int nextTextureIndex;

    // Start is called before the first frame update
    void Start()
    {
        sphereRenderer = sphere.GetComponent<Renderer>();
        gameObject.GetComponent<Button>().onClick.AddListener(ChangeSphereTexture);
        nextTextureIndex = 0;
        
    }

    // Update is called once per frame
    private void ChangeSphereTexture()
    {
        sphereRenderer.material.mainTexture = textures[nextTextureIndex];

        if (nextTextureIndex >= textures.Length-1)
        {
            nextTextureIndex = 0;
            Debug.Log(nextTextureIndex.ToString());

        }
        else
        {
            nextTextureIndex++;
            Debug.Log(nextTextureIndex.ToString());
        }
    }
}
