using System.Collections.Generic;
using UnityEngine;

public static class Database
{
    public static readonly Dictionary<string, HeroTemplateSO> heroTemplates = new();
    public static readonly Dictionary<string, ItemSO> items = new();
    
    static Database()
    {
        LoadHeroTemplates();
        LoadItems();
    }

    static void LoadHeroTemplates()
    {
        var allTemplates = Resources.LoadAll<HeroTemplateSO>("Heroes");
        foreach (var template in allTemplates)
        {
            heroTemplates.Add(template.id, template);
        }
    }

    static void LoadItems()
    {
        var allItems = Resources.LoadAll<ItemSO>("Items");
        foreach (var item in allItems)
        {
            items.Add(item.id, item);
        }

    }           
   
}
