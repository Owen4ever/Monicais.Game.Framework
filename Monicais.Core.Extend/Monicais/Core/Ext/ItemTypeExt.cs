
namespace Monicais.Core
{
    public static class ItemTypeExt
    {

        public static readonly ItemType EQUIPMENT = new ItemType("Equipment");

        public static readonly ItemType WEAPON = new ItemType("Weapon", EQUIPMENT);

        public static readonly ItemType ARMOR = new ItemType("Armor", EQUIPMENT);

        public static readonly ItemType SHOES = new ItemType("Shoes", EQUIPMENT);

        public static readonly ItemType HELMET = new ItemType("Helmet", EQUIPMENT);

        public static readonly ItemType MATERIAL = new ItemType("Material");

        public static readonly ItemType TOOL = new ItemType("Tool");

        public static readonly ItemType BAG = new ItemType("Bag");
    }
}
