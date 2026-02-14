using UnityEngine;

[CreateAssetMenu(menuName = "HeroTemplate")]
public class HeroTemplateSO : IDScriptableObject
{
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public Sprite mainSprite { get; private set; }
    [field: SerializeField] public RuntimeAnimatorController animationController { get; private set; }
    [field: SerializeField] public GameObject battlePrefab { get; private set; }
    [field: SerializeField] public Vector2Int[] footprint { get; private set; }

}
