using UnityEngine;

public class PixelScreen : MonoBehaviour
{
    [SerializeField] RenderTexture renderTex;
    Camera camera;

    bool lowResActive;
    public bool LowResActive
    {
        get { return lowResActive; }
        set
        {
            camera.targetTexture = value ? renderTex : null;
            lowResActive = value;
        }
    }

    void Awake()
    {
        camera = GetComponent<Camera>();
        LowResActive = false;
    }

    void OnGUI()
    {
        if (lowResActive && Event.current.type == EventType.Repaint)
            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTex);
    }
}
