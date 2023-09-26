using System.Threading.Tasks;
using UnityEngine;

public class StateMachine:MonoBehaviour
{
    public IBaseState CurrentState { get; private set; }

    public void Initialize(IBaseState startingState)
    {
        CurrentState = startingState;
        startingState.OnEnter();
    }

    public void ChangeState(IBaseState newState)
    {
        CurrentState.OnExit();
        CurrentState = newState;
        newState.OnEnter();
    }

    public async void Update()
    {
        if(CurrentState.IsWaiting)
            return;
        
        CurrentState.Tick();
        await Task.Delay(CurrentState.UpdateRate);
    }
}
