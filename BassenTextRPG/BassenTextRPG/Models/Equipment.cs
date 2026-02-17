namespace BassenTextRPG.Models;

public class Equipment : Item
{
    #region 프로퍼티

    public EquipmentSlot Slot { get; private set; }
    
    //공격력 보너스
    public int AttackBonus { get; private set; }

    //방어력 보너스
    public int DefenseBonus { get; private set; }
    
    #endregion
    
    #region 생성자

    public Equipment(string name, string description, int price,EquipmentSlot slot,int attackBonus=0,int defenseBonus=0) 
        : base(name, description, price, slot == EquipmentSlot.Armor? ItemType.Armor : ItemType.Weapon )
    {
        Slot = slot;
        AttackBonus = attackBonus;
        DefenseBonus = defenseBonus;
    }

    #endregion
   

    public override bool Use(Player player)
    { 
        //장비 착용 로직 구현
        player.EquipItem(this);
        return true;
    }

    #region 장착아이템 생성 메서드

    public static Equipment CreateWeapon(string WeaponType)
    {
        switch (WeaponType)
        {
            case"목검":
                return new Equipment("목검", "기본 무기", 100, EquipmentSlot.Weapon, 5);
            case"철검":
                return new Equipment("철검", "강력한 무기", 500, EquipmentSlot.Weapon, attackBonus: 15);
            case"전설검":
                return new Equipment("전설검", "최고급 무기", 1000, EquipmentSlot.Weapon, attackBonus: 25);
            default:
                return null;
        }
    }

    //방어구 생성메서드
    public static Equipment CreateArmor(string armorType)
    {
        switch (armorType)
        {
            case "천갑옷":
                return new Equipment("천갑옷", "기본 방어구",100, EquipmentSlot.Armor, 0,5);
            case "철갑옷":
                return new Equipment("철갑옷","강력한 방어구",500, EquipmentSlot.Armor, 0,20);
            case "전설갑옷":
                return new Equipment("전설갑옷","최고급 방어구",100, EquipmentSlot.Armor, 0,40);
            default:
                return null;
        }
    }
    
    #endregion
}