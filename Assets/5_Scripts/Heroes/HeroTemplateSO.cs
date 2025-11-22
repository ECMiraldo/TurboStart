using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Heroes/Hero Template")]
public class HeroTemplateSO : IDScriptableObject, IUnitData
{
    [field: SerializeField] public List<Sprite> possibleIcons { get; private set; }
    [field: SerializeField] public List<Sprite> possibleSprites { get; private set; }

    [field: SerializeField] public RuntimeAnimatorController animationController { get; private set; }
    [field: SerializeField] public GameObject prefab { get; private set; }


    public HeroBattleController InstantiateBattlePrefab(Transform parent)
    {
        GameObject newHero = Instantiate(prefab, parent);
        Transform sprite = newHero.transform.Find("Sprite");
        sprite.GetComponent<SpriteRenderer>().sprite = Utils.GetRandomFromList(possibleIcons);

        sprite.GetComponent<Animator>().runtimeAnimatorController = animationController;
        return newHero.GetComponent<HeroBattleController>();
    }

    public SerializedDictionary<UnitStat, CharacterAttribute> GetStatsMap()
    {
        SerializedDictionary<UnitStat, CharacterAttribute> dict = new();

        return dict;
    }

}


