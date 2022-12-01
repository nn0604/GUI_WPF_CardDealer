using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace W1002WPFCardDealer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void OnDeal(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            int[] num = new int[5];

            for (int i = 0; i < num.Length; i++)
            {
                int n = 0;
                do
                {
                    n = r.Next(52);
                } while (num.Contains(n));
                num[i] = n;
            }

            Array.Sort(num);

            BitmapImage[] b = new BitmapImage[num.Length];
            for (int i=0; i<b.Length; i++)
                b[i] = new BitmapImage(new Uri($"Images/{GetCardName(num[i])}", UriKind.RelativeOrAbsolute));

            Card1.Source = b[0];
            Card2.Source = b[1];
            Card3.Source = b[2];
            Card4.Source = b[3];
            Card5.Source = b[4];

            Hand.Text = "족보 : " + ReadHand(num);
            
        }

        private string GetCardName(int c)
        {
            string shape = "";
            switch (c / 13)
            {
                case 0: shape = "spades"; break;
                case 1: shape = "diamonds"; break;
                case 2: shape = "hearts"; break;
                case 3: shape = "clubs"; break;
            }

            string number = "";
            switch (c % 13)
            {
                case 0: number = "ace"; break;
                case int n when (n > 0 && n < 10):
                    number = (c % 13 + 1).ToString(); break;
                case 10: number = "jack"; shape += "2"; break;
                case 11: number = "queen"; shape += "2"; break;
                case 12: number = "king"; shape += "2"; break;
            }
            return string.Format("{0}_of_{1}.png", number, shape);
        }

      

        private string ReadHand(int[] hand)
        {
            string handname = "";
            int rankcount = 0;
            int paircount = 0;
            int straightcount = 0;
            int[] handnum = new int[5];
            int[] handsuit = new int[5];
            bool isSuitAllMatch = false;
            bool isBackStraight = false;
            bool isMountain = false;
     

            for (int i = 0; i < 5; i++)
            {
                handnum[i] = hand[i] % 13;
                handsuit[i] = hand[i] / 13;
            }

            Array.Sort(handnum);

            for (int i = 0; i < hand.Length-1; i++) // 스트레이트 확인
            {
                if (handnum[i]+1 == handnum[i+1]) straightcount++;
            }
            if(straightcount == 4) // 백 스트레이트 확인
            {
                if (handnum[0] == 0) isBackStraight = true;
            }
            if(straightcount == 3)
                if (handnum[0] == 0 && handnum[1] == 9) isMountain = true; // 마운틴 확인


            if (handnum.Distinct().Count() == 4) paircount = 1;
            else if (handnum.Distinct().Count() == 3) paircount = 2;
            else if (handnum.Distinct().Count() == 2) paircount = 3;

            if (handsuit.Distinct().Count() == 1) isSuitAllMatch = true; // 플러시 확인

            for (int i = 0; i < hand.Length; i++) // 
            {
                for (int j = 0; j < hand.Length; j++)
                {
                    if (i == j) continue;
                    if (handnum[i] == handnum[j])
                    {
                        rankcount++;
                    }
                }
            }

            if (paircount == 0) handname = "탑";
            if (paircount == 1) handname = "원페어";
            if (paircount == 2 && rankcount == 4) handname = "투페어";
            if (paircount == 2 && rankcount == 6) handname = "트리플";
            if (straightcount == 4) handname = "스트레이트";
            if (isBackStraight == true) handname = "백 스트레이트";
            if (isMountain == true) handname = "마운틴";
            if (isSuitAllMatch == true) handname = "플러시";
            if (paircount == 3) handname = "포카드";
            if (paircount == 3 && rankcount == 8) handname = "풀하우스";
            if (isSuitAllMatch == true && straightcount == 4) handname = "스트레이트 플러시";
            if (isSuitAllMatch == true && isBackStraight == true) handname = "백 스트레이트 플러시";
            if (isSuitAllMatch == true && isMountain == true) handname = "로열 스트레이트 플러시";

            return handname;
        }

        private void Simulator(object sender, RoutedEventArgs e)
        {
            string[] simulation = new string[10000];
            Random r = new Random();
            int[] num = new int[5];
            
            for(int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < num.Length; j++)
                {
                    int n = 0;
                    do
                    {
                        n = r.Next(52);
                    } while (num.Contains(n));
                    num[j] = n;
                }

                Array.Sort(num);
                simulation[i] = ReadHand(num);
            }
            
            MessageBox.Show("탑 :" + simulation.Count(x => x == "탑") +
                            "\n원페어 :" + simulation.Count(x => x == "원페어") +
                            "\n투페어 :" + simulation.Count(x => x == "투페어") +
                            "\n트리플 :" + simulation.Count(x => x == "트리플") +
                            "\n스트레이트 :" + simulation.Count(x => x == "스트레이트") +
                            "\n백 스트레이트 :" + simulation.Count(x => x == "백 스트레이트") +
                            "\n마운틴 :" + simulation.Count(x => x == "마운틴") +
                            "\n플러시 :" + simulation.Count(x => x == "플러시") +
                            "\n풀하우스 :" + simulation.Count(x => x == "풀하우스") +
                            "\n포카드 :" + simulation.Count(x => x == "포카드") +
                            "\n스트레이트 플러시 :" + simulation.Count(x => x == "스트레이트 플러시") +
                            "\n백 스트레이트 플러시 :" + simulation.Count(x => x == "백 스트레이트 플러시") +
                            "\n로열 스트레이트 플러시 :" + simulation.Count(x => x == "로열 스트레이트 플러시")
                );
        }

        private void HighHand(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            int[] num = new int[5];
            string handname;

            while (true)
            {
                for (int i = 0; i < num.Length; i++)
                {
                    int n = 0;
                    do
                    {
                        n = r.Next(52);
                    } while (num.Contains(n));
                    num[i] = n;
                }

                Array.Sort(num);
                handname = ReadHand(num);
                if (handname == "로열 스트레이트 플러시") break;
            }
                

            BitmapImage[] b = new BitmapImage[num.Length];
            for (int i = 0; i < b.Length; i++)
                b[i] = new BitmapImage(new Uri($"Images/{GetCardName(num[i])}", UriKind.RelativeOrAbsolute));

            Card1.Source = b[0];
            Card2.Source = b[1];
            Card3.Source = b[2];
            Card4.Source = b[3];
            Card5.Source = b[4];

            Hand.Text = "족보 : " + ReadHand(num);
        }
    }
}
