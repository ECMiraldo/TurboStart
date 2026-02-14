using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Persistence 
{
    [Serializable]
    public class PersistentData
    {
        //anything you put in here will be saved to disk.
        //make sure object types(classes) or structs have the [Serializable] attribute
        //newtonsoft's Json documentation is right here: https://www.newtonsoft.com/json/help/html/Introduction.htm 
        [JsonProperty] private string hello = "Hello World";

        public List<HeroData> heroes = new();

        public Inventory inventory;

        public PersistentData()
        {
            inventory = new();
            heroes.Clear();
            heroes = new List<HeroData>
            {
                new HeroData("4b3f15f1-5416-4150-8558-648141f34956"),
                new HeroData("4b3f15f1-5416-4150-8558-648141f34956"),
                new HeroData("4b3f15f1-5416-4150-8558-648141f34956"),

            };
        }

    }

}


