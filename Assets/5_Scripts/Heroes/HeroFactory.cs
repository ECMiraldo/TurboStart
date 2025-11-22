using System.Linq;

public class HeroFactory
{
    public static HeroData CreateRandomHero(int id)
    {
        HeroTemplateSO template = Utils.GetRandomFromList(Database.heroTemplates.Values.ToList());
        string spriteAddress = Utils.GetRandomFromList(template.possibleIcons).name;
        return new HeroData(id, template, GetRandomName(), spriteAddress);
    }

    private static string GetRandomName()
    {
        return "hero";
    }

}
