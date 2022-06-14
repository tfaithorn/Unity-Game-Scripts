using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorHelper : MonoBehaviour
{
    private float width = 5;
    private float height = 5;

    private GameObject projectorGO;
    private Projector projector;
    /// <summary>
    /// Used to instantiate the projector GameObject
    /// </summary>
    /// <param name="name"></param>
    public void Initialize(string name, Transform parentTransform = null)
    {
        projectorGO = Resources.Load<GameObject>("Projectors/Projector");
        
        projector = projectorGO.GetComponent<Projector>();

        projectorGO.SetActive(false);
        projectorGO = Instantiate(projectorGO);
        projectorGO.name = name;

        if (parentTransform) {
            projectorGO.transform.SetParent(parentTransform, false);
        }
    }

    public void SetImage(string imagePath)
    {
        Texture texture = Resources.Load<Texture>(imagePath);
        Material originalMaterial = Resources.Load<Material>("Projectors/ProjectorMaterial");

        Debug.Log(originalMaterial.name);

        //Shader shader = Shader.Find("Projector/Multiply");
        Material material = new Material(originalMaterial);
        material.SetTexture("_ShadowTex", texture);

        projector.material = material;
    }

    public void SetWidth(float width)
    {
        this.width = width;
    }

    public void Show()
    {
        projectorGO.SetActive(true);
    }

    public void Place(Vector3 position)
    {   
        projectorGO.transform.position = new Vector3(position.x, position.x + height, position.z);
    }

    public void Hide()
    {
        projectorGO.SetActive(false);
    }
}
