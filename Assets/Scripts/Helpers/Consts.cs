using System.Collections.Generic;

public static class Consts
{
    public static readonly Dictionary<WeaponType, string> weaponTypeToName = new Dictionary<WeaponType, string>()
    {
        { WeaponType.Pistol, "Pistol" },
        { WeaponType.Uzi, "UZI" },
        { WeaponType.Shotgun, "Shotgun" },
        { WeaponType.RocketLauncher, "Rockets" },
        { WeaponType.Grenade, "Grenades" }
    };
}
