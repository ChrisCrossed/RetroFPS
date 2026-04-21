using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyMoveState
{
    Patrolling,
    Patrolling_ShortHalt,
    Stationary
}

[System.Serializable]
public struct NavPoints
{
    public Transform transform;
    public EnemyMoveState moveState;
}


public class ENEMY_OBJ : MonoBehaviour
{
    private protected int Health;
    [SerializeField, Range(50, 250)] private protected int HEALTH_MAX = 100;
    [SerializeField] protected NavPoints[] NavPoints;

    NavMeshAgent ThisNavMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private protected virtual void Start()
    {
        START_Connections();

        if(NavPoints.Length > 0)
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

        ThisNavMeshAgent.SetDestination( NavPoints[ PatrolLocation ].transform.position );
    }

    #region Health and Damage

    public virtual void Damage(int _damage)
    {
        print("TOOK DAMAGE: " + _damage);
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
    float PatrolHaltTimer;
    
    IEnumerator CyclePatrolLocations()
    {
        while( MyMoveState == EnemyMoveState.Patrolling || MyMoveState == EnemyMoveState.Patrolling_ShortHalt )
        {
            print("---");
            print(PatrolLocation);

            while (ThisNavMeshAgent.remainingDistance > 0.1f)
                yield return new WaitForEndOfFrame();

            ThisNavMeshAgent.isStopped = true;

            PatrolLocation += 1;
            //PatrolLocation = ++PatrolLocation % NavigationLocations.Length;

            print(PatrolLocation);

            if(MyMoveState == EnemyMoveState.Patrolling_ShortHalt)
            {

            }

            // yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
            // yield return new WaitForSeconds(0.1f);

            ThisNavMeshAgent.isStopped = false;

            yield return new WaitForEndOfFrame();

            ThisNavMeshAgent.SetDestination(NavPoints[PatrolLocation].transform.position);
        }

        yield return null;
    }

    #endregion Movement Functions

    // Update is called once per frame
    private protected virtual void Update()
    {
        if(MyMoveState == EnemyMoveState.Patrolling || MyMoveState == EnemyMoveState.Patrolling_ShortHalt)
        {
            if(PatrolHaltTimer > 0f)
            {
                PatrolHaltTimer -= Time.deltaTime;

                if(PatrolHaltTimer > 0f)
                    return;

                ThisNavMeshAgent.isStopped = false;
            }

            if (ThisNavMeshAgent.remainingDistance < 0.1f)
            {
                PatrolLocation = ++PatrolLocation % NavPoints.Length;

                ThisNavMeshAgent.isStopped = false;

                ThisNavMeshAgent.SetDestination(NavPoints[PatrolLocation].transform.position);

                int nextLocationNum = ((PatrolLocation - 1) + NavPoints.Length) % NavPoints.Length;

                if (NavPoints[nextLocationNum].moveState == EnemyMoveState.Patrolling_ShortHalt)
                {
                    ThisNavMeshAgent.isStopped = true;

                    PatrolHaltTimer = Random.Range(2.0f, 4.0f);

                    print("Waiting Time: " + PatrolHaltTimer);

                    SetEnemyState(EnemyMoveState.Patrolling);
                }
            }
        }

    }
}
