using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new();

    private Interactable closestInteractable;

    private void Start()
    {
        Player player = GetComponent<Player>();

        player.playerInputActions.character.Interaction.performed += (ctx) =>
        {
            InteractWithClosest();
        };
    }

    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);
        UpdateCLosestInteractble();
    }

    public void UpdateCLosestInteractble()
    {
        closestInteractable?.HighlighActive(false);

        closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        closestInteractable?.HighlighActive(true);
    }

    public List<Interactable> GetInteractables() => interactables;
}
