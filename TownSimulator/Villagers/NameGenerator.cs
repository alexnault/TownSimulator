using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TownSimulator.Villagers
{
    public static class NameGenerator
    {
        static private Random _rand = new Random();

        static private string[] firstnames = new string[]{
            "Bob",
            "Robert",
            "Jean",
            "Philippe",
            "Alex",
            "Roger",
            "Paul",
            "Richard",
            "Alfalfa",
            "Don",
            "Stephane",
            "Ginette",
            "Olga"
        };

        static string[] lastnames = new string[]{
            "Gratton",
            "Nault",
            "Goulet",
            "Doe",
            "Joe",
            "Johns",
            "Schweizer",
            "De la Fontaine",
            "Juan",
            "Poitras"
        };

        public static string GetFirstname()
        {
            return firstnames[_rand.Next(0, firstnames.Length - 1)];
        }

        public static string GetLastname()
        {
            return lastnames[_rand.Next(0, lastnames.Length - 1)];
        }
    }
}
