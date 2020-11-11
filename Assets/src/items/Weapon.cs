using Assets.src.system;
using UnityEditor;
using UnityEngine;

namespace Assets.src.items
{
    public class Weapon : ScriptableObject, IWeapon, IItem
    {
        public string Name;
        public int AttackRange = 1;
        public WeaponType WeaponType;
        public DiceSides DiceSides;

        public int GetAttackRange()
        {
            return AttackRange;
        }

        public DiceSides GetDiceSides()
        {
            return DiceSides;
        }

        public WeaponType GetWeaponType()
        {
            return WeaponType;
        }

        [MenuItem("Assets/Create/Custom/Weapon")]
        public static void CreateWeaponTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Weapon", "New Weapon", "Asset", "Save Weapon", "Assets");

            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Weapon>(), path);
        }

        public string GetWeaponName()
        {
            return Name;
        }
    }
}