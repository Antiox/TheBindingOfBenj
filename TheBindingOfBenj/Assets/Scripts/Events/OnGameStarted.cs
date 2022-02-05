namespace GameLibrary
{
    public class OnGameStarted : IGameEvent
    {
        public GameStatus Status { get; set; }

        public OnGameStarted(GameStatus status)
        {
            Status = status;
        }
    }
}
