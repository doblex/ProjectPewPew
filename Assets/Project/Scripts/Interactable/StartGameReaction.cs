using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StartGameReaction : MonoBehaviour, I_ObjectReaction
{
    [SerializeField] HUD HUD_ref;

    void I_ObjectReaction.ObjectHighlight()
    {
        List<Material> Materials;

        foreach (Transform Child in transform)
        {
            if (Child)
            {
                Materials = Child.GetComponent<SkinnedMeshRenderer>().sharedMaterials.ToList();
                if (!Materials.Contains(HUD_ref.HighlightMaterial_ref))
                {
                    Materials.Add(HUD_ref.HighlightMaterial_ref);
                }
                Child.GetComponent<SkinnedMeshRenderer>().materials = Materials.ToArray();
            }
        }
    }

    void I_ObjectReaction.ObjectRemoveHighlight()
    {
        List<Material> Materials;

        foreach (Transform Child in transform)
        {
            if (Child)
            {
                Materials = Child.GetComponent<SkinnedMeshRenderer>().sharedMaterials.ToList();
                if (Materials.Contains(HUD_ref.HighlightMaterial_ref))
                {
                    Materials.Remove(HUD_ref.HighlightMaterial_ref);
                }
                Child.GetComponent<SkinnedMeshRenderer>().materials = Materials.ToArray();
            }
        }
    }

    void I_ObjectReaction.ObjectInteract()
    {
        HUD_ref.StartGame();
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
}
