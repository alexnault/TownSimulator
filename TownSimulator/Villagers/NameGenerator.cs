using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TownSimulator.Villagers
{
    public static class NameGenerator
    {

        public static string GetFirstname()
        {
            string[] fNames = new string[]{
                "Bob",
                "Robert",
                "Jean",
                "Philippe",
                "Alex",
                "Roger",
                "Paul",
                "Richard",
                "Alfalfa"
            };

            return GetRandomName(fNames);
        }


        public static string GetLastname()
        {
            string[] lNames = new string[]{
                "Gratton",
                "Nault",
                "Goulet",
                "Doe",
                "Joe",
                "Johns",
                "Schweizer"
            };

            return GetRandomName(lNames);
        }


        private static string GetRandomName(string[] names)
        {
            Random rand = new Random();

            return names[rand.Next(0, names.Length - 1)];
        }





    }
}
