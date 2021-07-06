using UnityEngine;

// Displays information about the current framerate - is more accurate than the Unity Stats gizmo.
public class AccurateFrameCounter : MonoBehaviour
{
    [SerializeField] private GUIStyle style;
        
    private float _deltaTime;

    private void Awake()
    {
        if (style.name == "")
            CreateDefaultStyle();
    }

    private void CreateDefaultStyle()
    {
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = Screen.height * 2 / 100;
        style.normal.textColor = Color.white;
    }

    private void Update() 
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

    private void OnGUI() 
    {
        Rect rect = new Rect(0, 0, Screen.width, style.fontSize);
        string framerate = CreateFramerateMessage();
            
        GUI.Label(rect, framerate, style);
    }

    private string CreateFramerateMessage()
    {
        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;
        return $"{msec:0.0} ms ({fps:0.} fps)";
    }
}