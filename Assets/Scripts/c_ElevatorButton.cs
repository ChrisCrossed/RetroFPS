using UnityEngine;

public class c_ElevatorButton : MonoBehaviour
{
    Material UnselectedMaterial;
    [SerializeField] Material LockedMaterial;
    [SerializeField] Material UnlockedMaterial;

    MeshRenderer this_MeshRenderer;

    bool IsUnlocked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this_MeshRenderer = GetComponent<MeshRenderer>();

        UnselectedMaterial = this_MeshRenderer.material;

        IsUnlocked = false;
    }

    public void SetLockedFloorState(bool _isUnlocked)
    {
        IsUnlocked = _isUnlocked;
        // print(gameObject.name + " " + IsUnlocked);
    }

    public bool LookAtButton;

    bool WasLookedAt;
    // Update is called once per frame
    void Update()
    {
        #region Set Button Material
        if (LookAtButton && !WasLookedAt)
        {
            if (IsUnlocked) this_MeshRenderer.material = UnlockedMaterial;
            else this_MeshRenderer.material = LockedMaterial;
        }
        else if(!LookAtButton && WasLookedAt)
        {
            this_MeshRenderer.material = UnselectedMaterial;
        }

        WasLookedAt = LookAtButton;
        

        #endregion Set Button Material
    }
}
