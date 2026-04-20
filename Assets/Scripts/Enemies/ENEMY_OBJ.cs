using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class ENEMY_OBJ : MonoBehaviour
{
    private protected int Health;
    [SerializeField, Range(50, 250)] private protected int HEALTH_MAX = 100;
    [SerializeField] protected Transform[] NavigationLocations;

    NavMeshAgent ThisNavMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private protected virtual void Start()
    {
        START_Connections();

        if(NavigationLocations.Length > 0)
            ThisNavMeshAgent.SetDestination(NavigationLocations[0].position);
    }

    private protected virtual void START_Connections()
    {
        ThisNavMeshAgent = GetComponent<NavMeshAgent>();

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
