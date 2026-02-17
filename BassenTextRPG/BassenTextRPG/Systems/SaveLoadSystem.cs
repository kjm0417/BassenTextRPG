using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using BassenTextRPG.Models;
namespace BassenTextRPG.Systems;
using System.Text.Json;
using BassenTextRPG.Data;

public class SaveLoadSystem
{
    //저장 경로 및 파일명
    private const string SaveFilePath = "savegame.json";

    //JSON 직렬화 옵션
    //직렬화 의미 : 객체 -> 문자열
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping //한글 지원 UTF

    };

    #region 저장 기능

    public static bool SaveGame(Player player, InventorySystem inventory)
    {
        try
        {
            //1. 게임 객체 (클래스) -> DtO(Data Transfer object) 반환
            var saveData = new GameSaveData
            {
                Player = ConvertToPlayerData(player),
                Inventory = ConvertToItemData(inventory),
            };
            
            //2. DTO 객체 -> 문자열로 변환 JSon
            string jsonString =  JsonSerializer.Serialize(saveData, jsonOptions);
            
            //3. Json 문자열 -> 파일로 저장
            File.WriteAllText(SaveFilePath, jsonString);
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    //palyer -> playerData로 변환
    private static PlayerData ConvertToPlayerData(Player player)
    {
        return new PlayerData
        {
            Name = player.Name,
            Job = player.Job.ToString(),
            Level = player.Level,
            CureentHp = player.CurrentHp,
            MaxHp = player.MaxHp,
            CureentMp = player.CurrentMp,
            MaxMp = player.MaxMp,
            AttackPower = player.AttackPower,
            DefensePower = player.Defense,
            Gold = player.Gold,
            EquipedWeaponName = player.EquipmentWeapon?.Name,
            EquipedArmorName = player.EquipmentArmor?.Name,
        };

    }
    
    //Inventoory -> ItemData로 변환
    private static List<ItemData> ConvertToItemData(InventorySystem inventory)
    {
        var itemDataList = new List<ItemData>();

        for (int i = 0; i < inventory.Count; i++)
        {
            var item = inventory.GetItem(i);
            if (item == null)
            {
                continue;
            }

            var itemData = new ItemData
            {
                Name = item.Name,
            };

            if (item is Equipment equipment)
            {
                itemData.ItemType = "Equipment";
                itemData.Slot = equipment.Slot.ToString();
            }
            else if (item is Consumable consumable)
            {
                itemData.ItemType = "Consumable";
            }
            itemDataList.Add(itemData);
        }
        return itemDataList;
        
    }
    
    
    #endregion

    #region  불러오기

    //저장 파일 여부 확인
    public static bool IsSaveFileExist()
    {
        return File.Exists(SaveFilePath);
    }
    public static GameSaveData? LoadGame()
    {
        try
        {
            //1. Json파일 문자열 일긱
            string jsonString = File.ReadAllText(SaveFilePath);
            
            //2. json 문자열 -> DTO변환 (역 직렬화)
            var saveData = JsonSerializer.Deserialize<GameSaveData>(jsonString, jsonOptions);
            Console.WriteLine("\n 게임데이터가 로드되었습니다");
            return saveData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    //PlayerData DTO를 Player 클래스로 변환 메서드
    public static Player? LoadPlayer(PlayerData data)
    {
        //Jobtype문자열 -> 열거형 (Enum)
        var job = Enum.Parse<JobType>(data.Job); //data.Job) -> Jobtype으로 변경한다 string을 Enum으로
        //Player 객체 생성
        var player = new Player(data.Name, job);
        
        //스텟 설정
        player.Level = data.Level;
        player.CurrentHp = data.CureentHp;
        player.MaxHp = data.CureentMp;
        player.CurrentMp = data.CureentMp;
        player.MaxMp = data.CureentMp;
        player.AttackPower = data.AttackPower;
        player.Defense = data.DefensePower;
        player.Gold =  data.Gold;
        
        return player;
        

    }
    
    //ItemData DTO를 Inventory 클래스로 변환 메서드
    public static InventorySystem LoadInventorySystem(List<ItemData> itemDataList,Player player)
    {
        var inventory = new  InventorySystem();

        foreach (var itemData in itemDataList)
        {
            Item? item = null;

            if (itemData.ItemType == "Equipment")
            {
                //장착 슬롯 확인
                var slot = Enum.Parse<EquipmentSlot>(itemData.Slot);
                if (slot == EquipmentSlot.Weapon)
                {
                    Equipment.CreateWeapon(itemData.Name);
                }
                else if (slot == EquipmentSlot.Armor)
                {
                    item = Equipment.CreateArmor(itemData.Name);
                }
            }
            else if (itemData.ItemType == "Consumable")
            {
                item = Consumable.CreatePotion(itemData.Name);
            }

            if (item != null)
            {
                inventory.AddItem(item); 
            }
        }
        return inventory;
    }
    
    
    //저장된 장착 아이템을 복원 메서드 (무기/방어구)
    public static void LoadEquippedItems(Player player, PlayerData data, InventorySystem inventory)
    {
        //무기 장착 복원
        if (!string.IsNullOrEmpty(data.EquipedWeaponName))
        {
            //인벤토리에서 같은 무기를 찾아서 
            for (int i = 0; i < inventory.Count; i++)
            {
                var item = inventory.GetItem(i);
                if (item is Equipment equipment && equipment.Slot == EquipmentSlot.Weapon &&
                    equipment.Name == data.EquipedWeaponName)
                {
                    player.EquipItem(equipment);
                    break;
                }
            }
        }
        
        if(!string.IsNullOrEmpty(data.EquipedArmorName))
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                var item = inventory.GetItem(i);
                if (item is Equipment equipment && equipment.Slot == EquipmentSlot.Armor &&
                    equipment.Name == data.EquipedArmorName)
                {
                   player.EquipItem(equipment); 
                }  
            }
        }
    }
    
   
    
    #endregion
}