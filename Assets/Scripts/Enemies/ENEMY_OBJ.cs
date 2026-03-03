using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ENEMY_OBJ : MonoBehaviour
{
    private protected int Health;
    [SerializeField, Range(50, 250)] private protected int HEALTH_MAX = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private protected virtual void Start()
    {
        START_Connections();
    }

    private protected virtual void START_Connections()
    {
        Health = HEALTH_MAX;
    }

    public virtual void Damage(int _damage)
    {

    }

    // Update is called once per frame
    private protected virtual void Update()
    {
        
    }
}
