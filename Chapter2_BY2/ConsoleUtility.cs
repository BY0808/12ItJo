﻿


namespace Chapter2_BY2
{
    internal class ConsoleUtility
    {
        //클래스에 직접 붙는 함수이기에 static으로 정의 > 그래서 인스턴스 없이 바로 호출 할 수 있음
        public static void PrintGameHeader() // 게임 시작 화면 출력
        {
            Console.WriteLine("------------------------------------------------ ");
            Console.WriteLine(" +-+-+-+-+-+-+-+ +-+-+ +-+-+-+-+-+-+ +-+-+-+-+-+ ");
            Console.WriteLine(" |W|E|L|C|O|M|E| |T|O| |S|U|M|M|O|N| |P|L|A|C|E| ");
            Console.WriteLine(" +-+-+-+-+-+-+-+ +-+-+ +-+-+-+-+-+-+ +-+-+-+-+-+ ");
            Console.WriteLine("               PRESS ANY KEY");
            Console.WriteLine("------------------------------------------------");
            Console.ReadKey();

        }
        //선택을 읽어오는 함수 + 검증의 기능
        public static int PromptMenuChoice(int min, int max) // 주어진 영역 안의 입력을 받아오는 메서드
        {
            while (true)
            {
                Console.Write("원하는 활동을 입력해주세요 >> ");
                // TryParse 는 성공여부를 반환(bool), 성공시 && 로 넘어감 그리고 out 두번째 return 을 함
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
            }
        }
        public static void PromptMenuChoice(int input) // 정해진 키만 입력으로 받아오는 메서드
        {
            while (true)
            {
                Console.Write("원하는 활동을 입력해주세요 >> ");
                // TryParse 는 성공여부를 반환(bool), 성공시 && 로 넘어감 그리고 out 두번째 return 을 함
                if (int.TryParse(Console.ReadLine(), out int choice) && choice == input)
                {
                    return;
                }
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
            }
        }

        //타이틀 보여주기 메소드
        internal static void ShowTitle(string title) // 제목을 표시하는 메서드
        {
            Console.ForegroundColor = ConsoleColor.Cyan; // 글자색을 시안색으로 변경
            Console.WriteLine(title);
            Console.ResetColor(); // 글자색 초기화
        }
        
        //텍스트 강조하기, s3 에 빈칸을 넣으면서, 메소드 입력시 생략이 가능해짐
        public static void PrintTextHighlights(string s1, string s2, string s3 = "") //s2만 강조해서 출력하는 메서드
        {
            Console.Write(s1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(s2); //중간 글자만 강조
            Console.ResetColor();
            Console.WriteLine(s3);
        }
        //글자 크기 맞추어서 전체를 숫자를 셈 > 길이를 알아냄
        private static int GetPrintableLength(string str) // 글자 길이을 계산하는 메서드
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2; // 한글과 같은 넓은 문자에 대해 길이 2 취급
                }
                else
                {
                    length += 1; // 나머지 문자에 대해 길이 1 취급
                }
            }
            return length;
        }

        //글자 수 맞추기
        internal static string PadRightForMixedText(string str, int totalLength) // 글자 출력 & 커서를 정렬해주는 메서드
        {
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength; // 빈공간을 셈
            return str.PadRight(str.Length + padding); // 빈공간을 채워줌
        }

        //게임 끝내기시 호출
        public static void PrintGameEnd()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(" +-+-+-+-+-+-+ +-+-+ +-+-+-+-+ +-+-+-+ ");
            Console.WriteLine(" |T|H|A|N|K|S| |T|O| |P|L|A|Y| |B|Y|E| ");
            Console.WriteLine(" +-+-+-+-+-+-+ +-+-+ +-+-+-+-+ +-+-+-+ ");
            Console.WriteLine("               THE END");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("      종료를 원하면 0을 눌러주세요");
        }
    }
}