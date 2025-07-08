using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace Persistence 
{
    [Serializable]
    public class ProfileData
    {
        //anything you put in here will be saved to disk.
        //make sure object types(classes) or structs have the [Serializable] attribute
        //newtonsoft's Json documentation is right here: https://www.newtonsoft.com/json/help/html/Introduction.htm 
        public List<Hero> heroes;

        public TavernData tavernData;
        public long lastTickTime;
        public ProfileData()
        {
            heroes = new List<Hero>();
            tavernData = new TavernData();
          
         }
    }

}


