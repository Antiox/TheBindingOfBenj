namespace GameLibrary
{
    class OnWeaponChanged : IGameEvent
    {
        public bool WeaponChange { get; set; }


        public OnWeaponChanged(bool weaponChanged)
        {
            WeaponChange = weaponChanged;
        }
    }
}
