using System;
// using System.Collections.Generic;
using System.Linq;

namespace puzzles
{
    class Program
    {
        //Random Array
        public static void RandomArray(){
            int[] arr = new int[10];
            Random rand = new Random();
            for (int a = 0; a < 10; a++)
                {
                    int b = rand.Next(6,25);
                    arr[a] = b;
                }
            int minVal = arr.Min();
            int maxVal = arr.Max();
            int sumVal = arr.Sum();
            Console.WriteLine("min: " + minVal);
            Console.WriteLine("max: " + maxVal);
            Console.WriteLine("sum: " + sumVal);
        }

        //Coin Flip
        public static int TossCoin(){
            Console.WriteLine("Tossing a Coin!");
            Random rand = new Random();
            int a = rand.Next(1,3);
            if (a == 1){
                Console.WriteLine("Heads");
            }
            else if (a == 2){
                Console.WriteLine("Tails");
            }
            return a;
        }
        public static double TossMultipleCoins(int num){
            int[] arr = new int[num];
            for (int a = 0; a < num; a++){
               arr[a] = TossCoin();
            }
            double headsCount = 0;
            double tailsCount = 0;
            for (int b = 0; b < arr.Length; b++){
                if (arr[b] == 1){
                    headsCount++;
                }
                else if (arr[b] == 2){
                    tailsCount++;
                }
            }
            double headsRatio = (headsCount/(headsCount + tailsCount));
            Console.WriteLine("Heads/Total Toss Ratio: " + string.Format("{0:0.00}", headsRatio));   
            return headsRatio;                
        }
        //Names
        public static string[] Names(){
            string[] names = {"Todd", "Tiffany", "Charlie", "Geneva", "Sydney"};
            string[] namesRef = {"Todd", "Tiffany", "Charlie", "Geneva", "Sydney"};
            string[] namesCheck = new string[5];            
            Random rand = new Random();            
            for (int a = 0; a < names.Length; a++){
                int repCheck = a;
                int b = rand.Next(0,namesRef.Length);
                string nameVal = namesRef[b];
                for (int c = 0; c < namesCheck.Length; c++){
                    if (namesCheck[c] == nameVal){
                        a--;
                    }
                    else{
                        continue;
                    }
                }
                if (repCheck == a){
                    names[a] = nameVal;
                    namesCheck[a] = nameVal;
                }
                else{
                    continue;
                }

            }
            int fiveArrCount = 0;
            Console.WriteLine("New Array Order: ");
            for (int d = 0; d < names.Length; d++){
                if (names[d].Length > 5){
                Console.WriteLine(names[d]);
                fiveArrCount++;  
                }
                else{
                Console.WriteLine(names[d]);           
                }
            }
            string[] fiveArr = new string[fiveArrCount];
            int f = 0;      
            for (int e = 0; e < names.Length; e++){
                if (names[e].Length > 5){
                    fiveArr[f] = names[e];
                    f++;
                }
                else{
                    continue;
                }

            }
            return fiveArr;
        }    
        
        static void Main(string[] args)
        {
            RandomArray();
            TossCoin();
            TossMultipleCoins(100);
            Names();
         
        }
    }
}


//SOLUTION

// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace ConsoleApplication
// {
//     public class Program
//     {
//         public int[] RandomArray() {
//             Random rand = new Random();
//             int[] randArr = new int[10];
//             int sum = 0;
//             for(int idx = 0; idx < randArr.Length; idx++) {
//                 //Up to 26 since .Next is non-inclusive
//                 int val = rand.Next(5,26);
//                 randArr[idx] = val;
//                 sum += val;
//             }
//             Console.WriteLine("The max value of the random array is {0}", randArr.Max());
//             Console.WriteLine("The min value of the random array is {0}", randArr.Min());
//             return randArr;
//         }

//         public string TossCoin(Random rand) {
//             Console.WriteLine("Tossing a Coin!");
//             string result = "Tails";
//             if(rand.Next() == 0) {
//                 result = "Heads";
//             }
//             Console.WriteLine(result);
//             return result;
//         }

//         public Double TossMultipleCoins(int num){
//             int numHeads = 0;
//             for(int reps = 0; reps < num; reps++){
//                 if(TossCoin(new Random()) == "Heads"){
//                     numHeads++;
//                 }
//             }
//             return (double)numHeads/(double)num;
//         }

//         public string[] Names() {
//             string[] names = new string[5] {"Todd","Tiffany","Charlie","Geneva","Sydney"};
//             //Fisher-Yates Shuffle
//             Random rand = new Random();
//             for(var idx = 0; idx < names.Length - 1; idx++){
//                 int randIdx = rand.Next(idx + 1, names.Length - 1);
//                 string temp = names[idx];
//                 names[idx] = names[randIdx];
//                 names[randIdx] = temp;
//                 //Print each name in it's new position
//                 Console.WriteLine(names[idx]);
//             }
//             //Don't forget the last value!
//             Console.WriteLine(names[names.Length - 1]);

//             //Return an array the only includes names longer than 5
//             List<string> nameList = new List<string>();
//             foreach(var name in names) {
//                 nameList.Add(name);
//             }
//             return nameList.ToArray();
//         }

//         public static void Main(string[] args)
//         {
//             //Invoke all functions here!
//         }
//     }
// }