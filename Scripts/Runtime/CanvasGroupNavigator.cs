using JetBrains.Annotations;
using UnityEngine;

// Contains a list of CanvasGroups and allows other scripts to toggle their activity,
// while ensuring that only one CanvasGroup can be active at a time: useful in UI design.
public class CanvasGroupNavigator : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] canvasGroups;

    private int _activeGroupIndex;
        
    private void Start()
    {
        if (canvasGroups.Length < 2)
        {
            Debug.LogWarning($"The CanvasGroupNavigator on {gameObject.name} " +
                             $"has only been assigned {canvasGroups.Length} canvas groups.");
        }
            
        UpdateGroups();
    }
        
    private void UpdateGroups()
    {
        foreach (var group in canvasGroups)
            SetActivity(group, false);

        var activeGroup = canvasGroups[_activeGroupIndex];
        SetActivity(activeGroup, true);
    }

    private static void SetActivity(CanvasGroup group, bool isActive)
    {
        group.blocksRaycasts = isActive;
        group.gameObject.SetActive(isActive);
    }
        
    [PublicAPI] 
    public void SetActiveGroup(int groupIndex)
    {
        _activeGroupIndex = groupIndex;
        UpdateGroups();
    }
}