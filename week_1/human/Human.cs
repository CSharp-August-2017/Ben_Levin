namespace human
{
    public class Human
    {
        public string humanName;
        public int strength = 3;
        public int intelligence = 3;
        public int dexterity = 3;
        public int health = 100;
        public Human(string name)
        {
            humanName = name; 
        }

        public Human(string name, int sVal, int iVal, int dVal, int hVal){
            humanName = name;
            strength = sVal;
            intelligence = iVal;
            dexterity = dVal;
            health = hVal;
        }

        public void attack(object other){
            Human enemy = other as Human;
            if(enemy != null){
                enemy.health = enemy.health - (strength*5);
            }
        }
    }
}

//SOLUTION

// namespace ConsoleApplication {
//     public class Human {
//         public string name;
//         public int strength = 3;
//         public int intelligence = 3;
//         public int dexterity = 3;
//         public int health = 100;

//         public Human(string n) {
//             name = n;
//         }

//         public Human(string n, int str, int intl, int dex, int hp){
//             name = n;
//             strength = str;
//             intelligence = intl;
//             dexterity = dex;
//             health = hp;
//         }

//         // public void Attack(Human enemy){
//         //     enemy.health -= 5 * strength;
//         // }
//         public void Attack(object target){
//             Human enemy = target as Human;
//             if(enemy != null) {
//                 enemy.health -= 5 * strength;
//             }
//         }
//     }
// }

// public class Human
// {
//     public string name;

//     //The { get; set; } format creates accessor methods for the field specified
//     //This is done to allow flexibility
//     public int health { get; set; }
//     public int strength { get; set; }
//     public int intelligence { get; set; }
//     public int dexterity { get; set; }

//     public Human(string person)
//     {
//         name = person;
//         strength = 3;
//         intelligence = 3;
//         dexterity = 3;
//         health = 100;
//     }
//     public Human(string person, int str, int intel, int dex, int hp)
//     {
//         name = person;
//         strength = str;
//         intelligence = intel;
//         dexterity = dex;
//         health = hp;
//     }
//     public void attack(object obj)
//     {
//         Human enemy = obj as Human;
//         if(enemy == null)
//         {
//             Console.WriteLine("Failed Attack");
//         }
//         else
//         {
//             enemy.health -= strength * 5;
//         }
//     }
// }