using System;
namespace BassenTextRPG.Models;


public abstract class Item
{
    #region 프로퍼티

    //아이템 이름
    public string Name { get; protected set; }
    //아이템 설명
    public string Description { get; protected set; }
    //아이템 가겨
    public int Price { get; protected set; }
    //아이템 타입
    public ItemType Type { get; protected set; }
    
    #endregion

    #region 생성자

    protected Item(string name, string description, int price, ItemType type)
    {
        Name = name;
        Description = description;
        Price = price;
        Type = type;
    }

    #endregion

    #region 메서드

    //아이템을 사용 메서드(추상 메서드)
    public abstract bool Use(Player player);
    
    //아이템 정보표시 메서드
    public virtual void DisPlayInfo()
    {
        Console.WriteLine($"[{Name}] {Description} (가격: {Price} 골드)");
    }

    #endregion
}