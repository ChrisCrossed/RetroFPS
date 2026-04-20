using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyMoveState
{
    Stationary,
    Patrolling
}


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
        {
            SetEnemyState(EnemyMoveState.Patrolling);

            // StartCoroutine(CyclePatrolLocations());
        }
    }

    private protected virtual void START_Connections()
    {
        ThisNavMeshAgent = GetComponent<NavMeshAgent>();

        Health = HEALTH_MAX;
        ThisNavMeshAgent.stoppingDistance = 1.0f;

        ThisNavMeshAgent.SetDestination(NavigationLocations[PatrolLocation].position);
    }

    #region Health and Damage

    public virtual void Damage(int _damage)
    {

    }

    #endregion Health and Damage

    #region Movement Functions

    EnemyMoveState MyMoveState;
    public void SetEnemyState(EnemyMoveState _state)
    {
        // Don't want to potentially reset a state if the state hasn't changed
        if(MyMoveState != _state)
            MyMoveState = _state;
    }

    int PatrolLocation;
    
    IEnumerator CyclePatrolLocations()
    {
        while( MyMoveState == EnemyMoveState.Patrolling )
        {
            print("---");
            print(PatrolLocation);

            while (ThisNavMeshAgent.remainingDistance > 0.1f)
                yield return new WaitForEndOfFrame();

            ThisNavMeshAgent.isStopped = true;

            PatrolLocation += 1;
            //PatrolLocation = ++PatrolLocation % NavigationLocations.Length;

            print(PatrolLocation);

            // yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
            // yield return new WaitForSeconds(0.1f);

            ThisNavMeshAgent.isStopped = false;

            yield return new WaitForEndOfFrame();

            ThisNavMeshAgent.SetDestination(NavigationLocations[PatrolLocation].position);
        }

        yield return null;
    }

    #endregion Movement Functions

    // Update is called once per frame
    private protected virtual void Update()
    {
        print("---");
        print(PatrolLocation);

        if (ThisNavMeshAgent.remainingDistance < 0.1f)
        {
            ThisNavMeshAgent.isStopped = true;

            PatrolLocation = ++PatrolLocation % NavigationLocations.Length;

            print(PatrolLocation);

            ThisNavMeshAgent.isStopped = false;

            ThisNavMeshAgent.SetDestination(NavigationLocations[PatrolLocation].position);
        }
    }
}
