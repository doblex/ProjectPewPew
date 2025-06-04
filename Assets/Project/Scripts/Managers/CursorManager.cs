using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursor;
    private void Awake()
    {
        if(defaultCursor != null)
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
