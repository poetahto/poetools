using System;
using System.Collections.Generic;
using InternalRealtimeCSG;
using poetools.Core;
using UnityEngine;

public class RealtimeCsgSurfaceData : IDisposable
{
    private readonly Dictionary<Material, Tag> _tagLookup;

    public RealtimeCsgSurfaceData(Dictionary<Material, Tag> tagLookup)
    {
        _tagLookup = tagLookup;
        PoetoolsRealtimeCsgHook.MeshRebuilt += HandleMeshRebuilt;
    }

    public void Dispose()
    {
        PoetoolsRealtimeCsgHook.MeshRebuilt -= HandleMeshRebuilt;
    }

    private void HandleMeshRebuilt(GeneratedMeshInstance mesh)
    {
        if (mesh.RenderSurfaceType == RenderSurfaceType.Normal)
        {
            if (_tagLookup.TryGetValue(mesh.RenderMaterial, out var tag))
                mesh.gameObject.AddTags(tag);
        }
    }
}
