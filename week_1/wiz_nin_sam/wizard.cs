using System;

namespace wiz_nin_sam
{
    public class Wizard : Human
    {
        public Wizard(string person) : base(person)
        {
            intelligence = 25;
            health = 50;
        }
        public Wizard(string person, int str, int intel, int dex, int hp) : base(person, str, intel, dex, hp)
        {
        }
        public void heal()
        {
            health =  health + (10 * intelligence);
        }
        public void fireball(object obj)
        {
            Random rand = new Random();
            Human enemy = obj as Human;
            if(enemy == null)
            {
                Console.WriteLine("Failed Attack");
            }
            else
            {
                enemy.health -= rand.Next(20,50);
            }
        }
    }
}