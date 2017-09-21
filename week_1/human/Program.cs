using System;

namespace human
{
    class Program
    {
        static void Main(string[] args)
        {
            //test Human(s)
            Human person1 = new Human("Chuck", 1, 1, 1, 190);
            Human person2 = new Human("Barry", 10, 10, 10, 150);
            Console.WriteLine(person2.health);
            person1.attack(person2);
            Console.WriteLine(person2.health);
        }
    }
}
