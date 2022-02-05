namespace GameLibrary
{
    class OnPlayerHurted : IGameEvent
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }


        public OnPlayerHurted(float maxHealth, float currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
    }
}
