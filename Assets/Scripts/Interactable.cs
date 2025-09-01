using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected MeshRenderer mesh;

    [SerializeField]
    private Material highlightMaterial;

    protected Material defaultMaterial;

    private void Start()
    {
        if (mesh == null)
        {
            mesh = GetComponentInChildren<MeshRenderer>();
        }

        defaultMaterial = mesh.material;
    }

    private void Update() { }

    protected void UpdateMeshAndMaterial(MeshRenderer meshRenderer)
    {
        mesh = meshRenderer;
        defaultMaterial = meshRenderer.sharedMaterial;
    }

    public void HighlighActive(bool active)
    {
        if (mesh == null || mesh.material == null)
            return;

        if (active)
        {
            mesh.material = highlightMaterial;
        }
        else
        {
            mesh.material = defaultMaterial;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
        {
            return;
        }

        HighlighActive(true);

        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateCLosestInteractble();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null)
        {
            return;
        }

        HighlighActive(false);
        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateCLosestInteractble();
    }

    public virtual void Interaction()
    {
        Debug.Log("Interact with " + gameObject.name);
    }
}
