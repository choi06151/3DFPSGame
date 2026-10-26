using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}