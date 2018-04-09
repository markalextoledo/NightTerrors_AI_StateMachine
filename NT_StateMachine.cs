/*
  The code architecture, for the enemy AI for Night Terrors

  @Author Mark Toledo
*/

//All the possible states needed for all kinds of enemy ai
public enum BoogeyStates
{
    //general states
    FindTarget,
    MoveToTarget,
    Attack,
    Stun,
    Die,
    //sploder specific states
    Stop_Moving,
    //snatcher specific states
    Idle,
    Charge,
    ChargeDelay,
    //snuffer specific states
    Spawn
}
//Used to create a states for enemies
public abstract class State
{
    protected BaseEnemy boogeyModel;

    //Initialize all the things the state might need from the enemey controller
    protected State(BaseEnemy paramBoogey)
    {
        boogeyModel = paramBoogey;
    }

    //will be overridden and the state's action will be placed here
    public abstract IEnumerator StateAction();
}
/*The BaseEnemy is the class that all enemy AI will inherit from*/
public class BaseEnemy : BaseUnit
{
    //List of states needed for the state machine
    protected Dictionary<BoogeyStates, State> dBoogeyState;

    //Sets the state in the state machine
    public void SetState(BoogeyStates paramNewState)
    {
        if (stateCurrent != null)
        {
            StopAllCoroutines();
        }

        bool bStateFound = dBoogeyState.TryGetValue(paramNewState, out stateCurrent);

        StartCoroutine(stateCurrent.StateAction());
    }

    //sets up all the states needed for the AI to function.  Child classes will override this
    protected virtual void InitializeStates() { }
}
//the snuffer class houses all states needed for the state machine and any related functions and variables needed
public class Snuffer : BaseEnemy {
    //intialize all the states for the snuffer AI
    protected override void InitializeStates()
    {
        dBoogeyState = new Dictionary<BoogeyStates, AIStateMachine.State>()
        {
            {BoogeyStates.FindTarget, new StateBaseFindTarget(this)},
            {BoogeyStates.MoveToTarget, new StateSnufferMoveToTarget(this)},
            {BoogeyStates.Attack, new StateSnufferAttack(this)},
            {BoogeyStates.Stun, new StateSnufferStun(this)},
            {BoogeyStates.Die, new StateSnufferDie(this)}
        };
    }
}
