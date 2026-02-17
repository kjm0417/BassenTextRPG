using System;

namespace BassenTextRPG.Models;

//캐릭터 기본 추상 클래스 :공통적인 부분 모아두기
public abstract class Character
{
    #region 속성 프로퍼티

    public string Name { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    
    public int CurrentMp { get; set; }
    
    public int MaxMp { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }
    public int Level { get; set; }
    
    //생존 여부
    public bool isAlive =>CurrentHp > 0;

    #endregion

    #region 생성자

    protected Character(string name, int maxHp,int maxMp, int attackPower, int defense, int level)
    {
        Name = name;
        MaxHp = maxHp;
        CurrentHp = maxHp;
        MaxMp = maxMp;
        CurrentMp = maxMp;
        AttackPower = attackPower;
        Defense = defense;
        Level = level;
    }

    #endregion

    #region 메서드

    //공통으로 사용할 메서드
    public abstract int Attack(Character target);
    
    //데미지 처리 메서드
    //가상메서드(virtual methed)
    public virtual int TakeDamage(int damage)
    {
        //방어력 적용
        int actualDamage = Math.Max(1,damage-Defense);
        CurrentHp -=Math.Max(0,CurrentHp - actualDamage);
        return actualDamage; 
    }
    
    //캐릭터 스텟 출력
    public virtual void DisPlayInfo()
    {
        Console.Clear();
        Console.WriteLine($"========={Name}정보========");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"체력: {CurrentHp}/{MaxHp}");
        Console.WriteLine($"마나: {CurrentMp}/{MaxMp}");
        Console.WriteLine($"공격력: {AttackPower}");
        Console.WriteLine($"방어력: {Defense}");
        Console.WriteLine($"===========================");
    }
    
    //HP 회복 메서드
    public int HealHp(int amount)
    {
        int beforeHp = CurrentHp;
        //회복 후 현재 hp 최대 hp 넘지 않도록
        CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
        return CurrentHp-beforeHp; //실제로 회복된 양
    }
    //MP 회복 메서드
    public int HealMp(int amount)
    {
        int  beforeMp = CurrentMp;
        CurrentMp = Math.Min(MaxMp, CurrentMp + amount);
        return CurrentMp-beforeMp;
    }
    #endregion
}