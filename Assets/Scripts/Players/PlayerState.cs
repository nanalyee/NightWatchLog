public interface IPlayerState
{
    void Enter(PlayerController player);
    void Update(PlayerController player);
    void Exit(PlayerController player);
}
