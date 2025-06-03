using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionsReaction : MonoBehaviour, I_ObjectReaction
{
    [SerializeField] HUD HUD_ref;
    MeshRenderer MeshRendererCompontent_ref;

    void Awake()
    {
        MeshRendererCompontent_ref = gameObject.GetComponent<MeshRenderer>();

    }

    void I_ObjectReaction.ObjectHighlight()
    {
        List<Material> Materials = MeshRendererCompontent_ref.sharedMaterials.ToList();
        if (!Materials.Contains(HUD_ref.HighlightMaterial_ref))
        {
            Materials.Add(HUD_ref.HighlightMaterial_ref);
        }
        MeshRendererCompontent_ref.materials = Materials.ToArray();
    }

    void I_ObjectReaction.ObjectRemoveHighlight()
    {
        List<Material> Materials = MeshRendererCompontent_ref.sharedMaterials.ToList();
        if (Materials.Contains(HUD_ref.HighlightMaterial_ref))
        {
            Materials.Remove(HUD_ref.HighlightMaterial_ref);
        }
        MeshRendererCompontent_ref.materials = Materials.ToArray();
    }

    void I_ObjectReaction.ObjectInteract()
    {
        HUD_ref.ToggleOptionsMenu();
    }
}
