using System;
using BassenTextRPG.Data;
using BassenTextRPG.Systems;
using BassenTextRPG.Utils;

namespace BassenTextRPG;

class Program
{
    static void Main(string[] args)
    {
        //콘솔 인코딩 설정(한글 지원)
         Console.OutputEncoding = System.Text.Encoding.UTF8;
        
      
         if (SaveLoadSystem.IsSaveFileExist())
         {
             ShowStartMenu();
         }
         else
         {
             GameManager.Instance.StartGame();
         }
        //저장된 게임 존재 여부 확인
    }

    static void ShowStartMenu()
    {
        Console.Clear();
        ConsoleUI.ShowTitle();
        Console.WriteLine("\n╔══════════════════════════════════════════╗");
        Console.WriteLine("║            게임 시작                      ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");
        
        Console.WriteLine("\n 1. 새게임");
        Console.WriteLine("2. 이어서하기");
        Console.WriteLine("0. 종료");

        while (true)
        {
            Console.WriteLine("\n선택 >");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    GameManager.Instance.StartGame();
                    break;
                case "2":
                    //이어서하기
                    if (GameManager.Instance.LoadGame())
                    {
                        GameManager.Instance.StartGame(true);
                    }
                    return;
                case "0":
                    Console.WriteLine("게임을 종료합니다");
                    return;
                default:
                    Console.WriteLine("잘못된 선택입니다.");
                    break;
            }

        }
    }
}