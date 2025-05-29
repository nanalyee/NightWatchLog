using UnityEngine;

public class IdleState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        //Debug.Log("Entered Idle State");
    }

    public void Update(PlayerController player)
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            player.SetState(new WalkingState());
        }
    }

    public void Exit(PlayerController player)
    {
        //Debug.Log("Exiting Idle State");
    }
}
