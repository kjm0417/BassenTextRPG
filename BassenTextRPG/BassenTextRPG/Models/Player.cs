using System;

namespace BassenTextRPG.Models;

public class Player : Character
{
    #region 프로퍼티

    public JobType Job { get; private set; }
    //골드
    public int Gold { get; set; }

    //장착할 무기
    public Equipment? EquipmentWeapon { get; private set; }
    
    //장착 방어구
    public Equipment? EquipmentArmor { get; private set; }

    #endregion

    #region 생성자

    public Player(string name,  JobType job) : base(
        name:name,
        maxHp:GetinitHp(job),
        maxMp:GetinitMp(job), 
        attackPower:GetinitAttack(job),
        defense:GetInitDefense(job),
        level:1 )
    {
        Job = job;
        Gold = 1000;
    }

    #endregion

    #region 직업별 초기 스탯

    //Static으로하는 이유 생성장에서 바로 접근할 수 있도록 하기 위해
    private static int GetinitHp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior:
                return 150;
            case JobType.Archer:
                return 100;
            case JobType.Wizard:
                return 50;
            default:
                return 100;
        }
    }

    private static int GetinitMp(JobType job)
    {
        switch (job)
        {
            case JobType.Warrior: return 30;
            case JobType.Archer: return 50;
            case JobType.Wizard: return 100;
            default: return 30;
            
        }
       
    }

    private static int GetinitAttack(JobType job) =>
        job switch
        {
            JobType.Warrior => 20,
            JobType.Archer => 30,
            JobType.Wizard => 40,
            _ => 20 //defult
        };

    private static int GetInitDefense(JobType job) =>
        job switch
        {
            JobType.Warrior => 15,
            JobType.Archer => 10,
            JobType.Wizard => 5,
            _ => 20
        };

    #endregion

    #region 메서드
    //플레이어 정보 출력
    public override void DisPlayInfo()
    {
        //base.DisPlayInfo();
        Console.Clear();
        Console.WriteLine($"====={Name} 정보 ====");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($".HP: {CurrentHp/MaxHp}");
        Console.WriteLine($".MP: {CurrentMp/MaxMp}");

        int attackBonus = EquipmentWeapon != null ? EquipmentWeapon.AttackBonus : 0;
        int defenseBonus = EquipmentArmor != null ? EquipmentArmor.DefenseBonus : 0;
        
        Console.WriteLine($"ATK : {AttackPower} (+{attackBonus})");
        Console.WriteLine($"DEF : {Defense} (+{defenseBonus})");
        Console.WriteLine($"골드: {Gold}");
        
        //장착 아이템 목록
        if (EquipmentWeapon != null || EquipmentArmor != null)
        {
            Console.WriteLine("\n[장착 중인 장비 목록]");
            if (EquipmentWeapon != null)
            {
                Console.WriteLine($"무기 : {EquipmentWeapon.Name}");
            }

            if (EquipmentArmor != null)
            {
                Console.WriteLine($"방어구 : {EquipmentArmor.Name}");
            }
        }
    }
    
    //기본 공격 메서드
    public override int Attack(Character target)
    {
        //장착무기 또는 방어구에 따른 추가 데미지
        int attackDamage = AttackPower;

        //null 병합 연산자 : ??
       attackDamage += EquipmentWeapon?.AttackBonus ?? 0;

       // if (EquipmentWeapon != null)
       // {
       //     attackDamage += EquipmentWeapon.AttackBonus;
       // }
        
        return target.TakeDamage(attackDamage);
    }

    //플레이어 전용 메서드 (스킬 공격)
    public int SkillAttack(Character target)
    {
        int mpCost = 15;
        
        // 스킬 공격 = 기본 공격 1.5 데미지
        int totalDamage = AttackPower;
        totalDamage += EquipmentWeapon?.AttackBonus ?? 0;
        totalDamage = (int)(totalDamage * 1.5f);
        
        //MP 소모
        CurrentMp-=mpCost;
        
        //데미지 전달
        return target.TakeDamage(totalDamage);
    }

    //골드 획득 메서드
    public void GainGold(int amount)
    {
        Gold += amount;
        Console.WriteLine($"골드 +{amount}획득! 현재 골드: {Gold}");
    }
    
    //골드 차감 메서드
    public void SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
        }
        
    }
    //장비 착용
    public void EquipItem(Equipment newEquipment)
    {
        Equipment? provEquipment = null;

        switch (newEquipment.Slot)
        {
            case EquipmentSlot.Weapon:
                provEquipment = EquipmentWeapon;
                EquipmentWeapon = newEquipment;
                break;
            case EquipmentSlot.Armor:
                provEquipment = EquipmentArmor;
                EquipmentArmor = newEquipment;
                break;
        }

        //이전 장비 해제 메시지
        if (provEquipment != null)
        {
            Console.WriteLine($"{provEquipment} 장착 해제");
        }
        Console.WriteLine($"{newEquipment.Name} 장착 완료");
        
        
    }
    
    //장비 해제
    public Equipment? UnequipItem(EquipmentSlot slot)
    {
        Equipment? equipment = null;
        switch (slot)
        {
            case EquipmentSlot.Weapon:
                equipment = EquipmentWeapon;
                EquipmentWeapon =  null;
                break;
            case EquipmentSlot.Armor:
                equipment = EquipmentArmor;
                EquipmentArmor =  null;
                break;
            
        }

        if (equipment != null)
        {
            Console.WriteLine($"{equipment.Name}장착 해제");
        }
        return equipment;
    }
    
    #endregion
}