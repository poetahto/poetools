using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu]
public class ApplicationSettings : ScriptableObject
{
    [Header("Framerate Settings")]
    [SerializeField] private int vSyncCount;
    [SerializeField] private int targetFrameRate = -1;
        
    [Header("Cursor Settings")]
    [SerializeField] private bool visible;
    [SerializeField] private CursorLockMode lockMode = CursorLockMode.Confined;

    [Header("Screen Settings")] 
    [SerializeField] private bool fullscreen = true;
    [SerializeField] private FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        
    [PublicAPI]
    public void Apply()
    {
        QualitySettings.vSyncCount = vSyncCount;
        Application.targetFrameRate = targetFrameRate;
        Cursor.visible = visible;
        Cursor.lockState = lockMode;
        Screen.fullScreen = fullscreen;
        Screen.fullScreenMode = fullScreenMode;
    }
}