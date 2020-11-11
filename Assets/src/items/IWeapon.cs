
using Assets.src.system;

namespace Assets.src.items
{
    public interface IWeapon
    {
        string GetWeaponName();
        int GetAttackRange();
        WeaponType GetWeaponType();
        DiceSides GetDiceSides();
    }
}
