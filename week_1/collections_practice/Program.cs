using System;
using System.Collections.Generic;

namespace collections_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            // ARRAY TO HOLD VALUES 0 THRU 9
            int[] array1 = {0,1,2,3,4,5,6,7,8,9};

            //ARRAY OF NAMES
            string[] array2 = {"Tim", "Martin", "Nikki", "Sara"};

            //ARRAY OF TRUE/FALSE
            bool[] array3 = new bool[10];
            for (int i = 0; i < array3.Length; i += 2)
            {
                array3[i] = true;
                array3[i+1] = false;
            }
            //MULTIPLICATION TABLE ARRAY
            int[][] array4 = new int[10][];                 
            for (int a = 1; a < 11; a += 1)
            {
                array4[a-1] = new int[] {(1*a),(2*a),(3*a),(4*a),(5*a),(6*a),(7*a),(8*a),(9*a),(10*a)};
            }
            //LIST OF FLAVORS

            List<string> flavors = new List<string>();
            flavors.Add("Vanilla");
            flavors.Add("Chocolate");
            flavors.Add("Strawberry");
            flavors.Add("Coffee");
            flavors.Add("Pistachio");
            Console.WriteLine(flavors.Count);
            Console.WriteLine(flavors[2]);
            flavors.RemoveAt(2);
            Console.WriteLine(flavors.Count);
            //USER INFO DICTIONARY
            //create dictionary, add names
            Dictionary<string,string> users = new Dictionary <string,string>();
            for (int b = 0; b < array2.Length; b++)
            {
                users.Add(array2[b], null);
            }
            //assign random flavors
            Random rand = new Random();
            for (int c = 0; c < array2.Length; c++)
            {
                users[array2[c]] = flavors[rand.Next(0, flavors.Count)];
            }
            //loop to print dictionary
            foreach (KeyValuePair<string,string> user in users)
            {
                Console.WriteLine(user.Key + " - " + user.Value);
            }            
        }
    }
}