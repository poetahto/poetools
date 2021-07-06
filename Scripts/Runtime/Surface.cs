using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] private SurfaceSettings settings;

    public SurfaceSettings Settings => settings;
}