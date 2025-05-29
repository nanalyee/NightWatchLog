using UnityEngine;

public class WalkingState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        //Debug.Log("Entered Walking State");
    }

    public void Update(PlayerController player)
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            player.SetState(new IdleState());
        }
    }

    public void Exit(PlayerController player)
    {
        //Debug.Log("Exiting Walking State");
    }
}
