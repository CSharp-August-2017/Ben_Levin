using System;

namespace wiz_nin_sam
{
    public class Ninja : Human
    {
        public Ninja(string person) : base(person)
        {
            dexterity = 175;
        }
        public Ninja(string person, int str, int intel, int dex, int hp) : base(person, str, intel, dex, hp)
        {
        }
        public void steal(object obj)
        {
            Random rand = new Random();
            Human enemy = obj as Human;
            if(enemy == null)
            {
                Console.WriteLine("Failed Attack");
            }
            else
            {
                enemy.health -= 10;
                health += 10;
            }
        }
        public void get_away()
        {
            health -= 15;
        }
    }
}