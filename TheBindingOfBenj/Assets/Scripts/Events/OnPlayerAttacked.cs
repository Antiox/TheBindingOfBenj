namespace GameLibrary
{
    class OnPlayerAttacked : IGameEvent
    {
        public bool Attack { get; set; }


        public OnPlayerAttacked(bool attack)
        {
            Attack = attack;
        }
    }
}
