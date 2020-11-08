using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;

namespace Models
{
    public class PersonnageModel
    {
        public Personnage Personnage { get; set; }
    }

    public class Personnage
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public int ActionPoints { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Vigor { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }
        public int Charisma { get; set; }
        public string History { get; set; }
        public List<Sorts> Sorts { get; set; }
    }

    public class Sorts
    {
        public string Name { get; set; }
        public string Animation { get; set; }
        public string PersonnageAnimation { get; set; }
        public string Icone { get; set; }
        public int ActionPoints { get; set; }
        public int Damage { get; set; }
        public int MinimumScope { get; set; }
        public int MaximumScope { get; set; }
        public int[][] Cases { get; set; }
        public List<Effects> Effects { get; set; }
    }

    public class Effects
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int[][] Cases { get; set; }
    }

    //Passiv Bonus
}
