using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField]
    private Texture[] colorTexture;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {
        // Change color every 0.5s
        InvokeRepeating(nameof(SetupLook), 0, 1.5f);
    }

    public void SetupLook()
    {
        SetupRandomColor();
    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTexture.Length);

        Material newMaterial = new(skinnedMeshRenderer.material);

        newMaterial.mainTexture = colorTexture[randomIndex];

        skinnedMeshRenderer.material = newMaterial;
    }
}
