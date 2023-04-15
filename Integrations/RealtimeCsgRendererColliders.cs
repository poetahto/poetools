using System;
using InternalRealtimeCSG;
using UnityEngine;

public class RealtimeCsgRendererColliders : IDisposable
{
    public RealtimeCsgRendererColliders()
    {
        PoetoolsRealtimeCsgHook.MeshRebuilt += HandleMeshRebuilt;
    }
    
    public void Dispose()
    {
        PoetoolsRealtimeCsgHook.MeshRebuilt -= HandleMeshRebuilt;
    }

    private static void HandleMeshRebuilt(GeneratedMeshInstance mesh)
    {
        if (mesh.RenderSurfaceType == RenderSurfaceType.Collider && mesh.gameObject.activeSelf)
            mesh.gameObject.SetActive(false);

        if (mesh.RenderSurfaceType == RenderSurfaceType.Normal)
            mesh.gameObject.AddComponent<MeshCollider>();
    }
}