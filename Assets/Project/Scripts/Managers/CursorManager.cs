using UnityEngine;
using UnityEngine.Rendering;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursor;
    private void Awake()
    {
        if(defaultCursor != null)
            Cursor.SetCursor(defaultCursor,new Vector2(defaultCursor.width / 2, defaultCursor.height / 2), CursorMode.Auto);
    }
}
