using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureMapHeroChoosingCard : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    public void SetHero(HeroData hero)
    {
        image.sprite = hero.Sprite;
        nameText.text = hero.name;
        levelText.text = hero.level.ToString();
        
    }
}
