// Samurai should have a default health of 200
//  Samurai should have a method called death_blow, which when 
//      invoked should attack an object and decreases its health to 0 if it has less than 50 health
//  Samurai should have a method called meditate, which when invoked, 
//      heals the Samurai back to full health
//  (optional) Samurai should have a class method called how_many, which when invoked, 
//      displays how many Samurai's there are. Hint you may have to use the static keyword

using System;

namespace wiz_nin_sam
{
    public class Samurai : Human
    {
        static int count = 0;
        public Samurai(string person) : base(person)
        {
            count++;
            health = 200;
        }
        public Samurai(string person, int str, int intel, int dex, int hp) : base(person, str, intel, dex, hp)
        {
            count++;
        }
        public void death_blow(object obj)
        {
            Random rand = new Random();
            Human enemy = obj as Human;
            if((enemy == null) || (enemy.health >= 50))
            {
                Console.WriteLine("Failed Attack");
            }
            else
            {
                
                enemy.health = 0;
            }
        }
        public void meditate()
        {
            health = 200;
        }
        public int how_many()
        {
            return count;
        } 
    }
}