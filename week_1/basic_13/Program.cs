using System;
using System.Linq;

namespace basic_13
{
    class Program
    {
        static void Main(string[] args)
        {
        //Print 1-255    
        Console.WriteLine("1. Print 1-255");
        for (int a = 1; a < 256; a++)
            {
            Console.WriteLine(a);
            }
        //Print 1-255 Odd
        Console.WriteLine("2. Print 1-255, odd");
        for (int a = 1; a < 256; a++)
            {
                if((a % 2 == 0) == true)
                {
                    continue;
                }
                else
                {
                    Console.WriteLine(a);
                }
            }
        //Print Sum
        Console.WriteLine("3. Print 0-255, with sum");
        int b = 0;
        for (int a = 0; a < 256; a++)
            {
            b = a + b;
            Console.WriteLine("New number: " + a + ", sum: " + b);
            }       
        //Iterating thru an Array
        Console.WriteLine("4. Iterate thru Array");
        int[] arr = {1,3,5,7,9,13};
        for (int a = 0; a < arr.Length; a++)
            {
            Console.WriteLine(arr[a]);
            }    
        //Find max in Array
        Console.WriteLine("5. Find max in array");
        int[] arr2 = {-3, -5, -7};
        int maxValue = arr2.Max();
        Console.WriteLine(maxValue);
        //Find avg in Array
        Console.WriteLine("6. Find avg in array");
        double[] arr3 = {2, 10, 3};
        double avgValue = arr3.Average();
        Console.WriteLine(avgValue);
        //Array with Odd Numbers         
        Console.WriteLine("7. Array with Odd Numbers");
        int[] arr4 = new int[(256/2)];
        int c = 0;        
        for (int a = 1; a < 256; a++)
            {
                if((a % 2 == 0) == true)
                {
                    continue;
                }
                else
                {
                    arr4[c] = a;
                    c++;
                }
            }
        //Greater than Y
        Console.WriteLine("8. Greater than Y");
        int[] arr5 = {1, 3, 5, 7};
        int y = 3;
        int d = 0;
        for (int a = 0; a < arr5.Length; a++)
            {
            if(arr5[a] > y)
                {
                    d++;
                }
            else
                {
                    continue;
                }
            }
            Console.WriteLine(d);
        //Square the Values
        Console.WriteLine("9. Square the Values");
        int[] arr6 = {1, 5, 10, -2};
        for (int a = 0; a < arr6.Length; a++)
            {
            arr6[a] = arr6[a] * arr6[a];
            }
        //Eliminate Negative Numbers
        Console.WriteLine("10. Eliminate Negative Numbers");
        int[] arr7 = {1, 5, 10, -2};
        for (int a = 0; a < arr7.Length; a++)
            {
            if (arr7[a] < 0)
                {
                    arr7[a] = 0;
                }
            else
                {
                    continue;
                }
            }
        //Min, Max, Average
        Console.WriteLine("11. Min, Max, Average");
        int[] arr8 = {1, 5, 10, -2};
        int minValue11 = arr8.Min();
        int maxValue11 = arr8.Max();
        double avgValue11 = arr8.Average();
        Console.WriteLine("min: " + minValue11);
        Console.WriteLine("max: " + maxValue11);
        Console.WriteLine("avg: " + avgValue11);
        //Shifting the values in an array
        Console.WriteLine("12. Shifting the values in an array");
        int[] arr9 = {1, 5, 10, 7, -2};
        for (int a = 0; a < arr9.Length; a++)
            {
                if (a < (arr9.Length - 1))
                {
                    arr9[a] = arr9[a+1];
                }
                else
                {
                    arr9[a] = 0;
                }
            }
        //Number to String
        Console.WriteLine("13. Number to String");
        string[] arr10 = {"-1", "-3", "2"};
        for (int a = 0; a < arr10.Length; a++)
            {
            if (Convert.ToInt32(arr10[a]) < 0)
                {
                    arr10[a] = "Dojo";
                }
            else
                {
                    continue;
                }
            }        
        }
    }
}

// //SOLUTION

// using System;
// using System.Collections.Generic;

// namespace ConsoleApplication
// {
//     public class Program
//     {
//         //Prints values 1 through 255 to the console
//         public static void Print1to255() {
//             for(int val=1; val <= 255; val++) {
//                 Console.WriteLine(val);
//             }
//         }

//         //Print all odd values between 1 and 255 to the console
//         public static void PrintOdds() {
//             for(int val = 1; val < 256; val++) {
//                 if(val % 2 == 1) {
//                     Console.WriteLine(val);
//                 }
//             }
//         }

//         //Print Sum all number between 0 and 255
//         //New number: $ Sum: #
//         public static void PrintSum() {
//             int sum = 0;
//             for(int num = 0; num <= 255; num++) {
//                 sum += num; //sum = sum + num
//                 Console.WriteLine($"New Number: {num} Sum: {sum}");
//             }
//         }

//         //Iterate through a passed array
//         public static void ArrayIterate(int[] arr) {
//             string output = "[";
//             for(int idx = 0; idx < arr.Length; idx++) {
//                 output += arr[idx] + ", ";
//             }
//             output += "]";
//             Console.WriteLine(output);
//         }

//         //Find max value in an array
//         public static void MaxInArray(int[] arr) {
//             int max = arr[0];
//             foreach(int val in arr){
//                 if(val > max) {
//                     max = val;
//                 }
//             }
//             Console.WriteLine("The max value is {0}", max);
//         }

//         //Get average value of an array
//         public static void AvgOfArray(int[] arr) {
//             int sum = GetSum(arr);
//             Console.WriteLine("This average is " + (double)sum/(double)arr.Length);
//         }
//         public static int GetSum(int[] arr) {
//             int sum = 0;
//             for(int num = 0; num < arr.Length; num++) {
//                 sum += arr[num]; //sum = sum + num
//             }
//             return sum;
//         }

//         //Create arr of odd numbers between 1 and 255
//         public static int[] CreateOddArray() {
//             List<int> oddList = new List<int>();
//             for(int val = 1; val < 256; val++) {
//                 if(val % 2 == 1) {
//                     oddList.Add(val);
//                 }
//             }
//             return oddList.ToArray();
//         }

//         //Count all values greater than myArr
//         public static void GreaterThanY(int[] arr, int y) {
//             int count = 0;
//             foreach(int val in arr){
//                 if(val > y) {
//                     count++;
//                 }
//             }
//             Console.WriteLine($"There are {count} values greater than {y}");
//         }

//         //Square all values in an arr
//         public static void SquareArrayValues(int[] arr) {
//             for(int idx = 0; idx < arr.Length; idx++) {
//                 arr[idx] *= arr[idx];
//             }
//         }

//         //Elimate Negative Numbers in an array
//         public static void ReplaceNegatives(int[] arr) {
//             for(int idx = 0; idx < arr.Length; idx++) {
//                 if(arr[idx] < 0) {
//                     arr[idx] = 0;
//                 }
//             }
//         }

//         //Retrieve the min, max, and average values from an array
//         public static void MinMaxAvg(int[] arr) {
//             int sum = 0;
//             int min = arr[0];
//             int max = arr[0];
//             foreach(int val in arr) {
//                 sum += val;
//                 if(val < min) {
//                     min = val;
//                 }
//                 if(val > max) {
//                     max = val;
//                 }
//             }
//             Console.WriteLine("The max of the array is {0}, the min is {1}, and the average is {2}", max, min, (double)sum/(double)arr.Length);
//         }

//         //Shift an array to the front by one number and add 0 to the end
//         public static void ShiftLeft(int[] arr) {
//             for(int idx = 0; idx < arr.Length - 1; idx++){
//                 arr[idx] = arr[idx + 1];
//             }
//             arr[arr.Length - 1] = 0;
//         }

//         //replace negatives with "dojo"
//         public static object[] ReplaceNumberWithString(object[] arr) {
//             for(int idx = 0; idx < arr.Length; idx++) {
//                 if((int)arr[idx] < 0) {
//                     arr[idx] = "Dojo";
//                 }
//             }
//             return arr;
//         }
//         public static void Main(string[] args)
//         {
//             Print1to255();
//             PrintOdds();
//             PrintSum();
//             int[] myArr = new int[6] {1,3,5,7,9,13};
//             ArrayIterate(myArr);
//             MaxInArray(myArr);
//             AvgOfArray(myArr);
//             CreateOddArray();
//             GreaterThanY(myArr, 4);
//             SquareArrayValues(myArr); //Passed by reference
//             ReplaceNegatives(myArr); //Passed by reference
//             ShiftLeft(myArr);
//             MinMaxAvg(myArr);
//             ShiftLeft(myArr);
//             object[] boxedArray = new object[] {-1, 3, 2 -16};
//             ReplaceNumberWithString(boxedArray);
//         }
//     }
// }
