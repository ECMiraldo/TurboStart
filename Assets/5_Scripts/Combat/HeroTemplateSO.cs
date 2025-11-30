using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Heroes/Hero Template")]
public class HeroTemplateSO : IDScriptableObject
{
    [field: SerializeField] public List<Sprite> possibleIcons { get; private set; }
    [field: SerializeField] public List<Sprite> possibleSprites { get; private set; }
    [field: SerializeField] public FighterData starterStats { get; private set; }
    [field: SerializeField] public RuntimeAnimatorController animationController { get; private set; }
    [field: SerializeField] public GameObject prefab { get; private set; }


    public HeroBattleController InstantiateBattlePrefab(Transform parent)
    {
        GameObject newHero = Instantiate(prefab, parent);
        newHero.name = name;

        newHero.GetComponentInChildren<SpriteRenderer>().sprite = Utils.GetRandomFromList(possibleIcons);
        newHero.GetComponentInChildren<Animator>().runtimeAnimatorController = animationController;

        return newHero.GetComponent<HeroBattleController>();
    }
}


