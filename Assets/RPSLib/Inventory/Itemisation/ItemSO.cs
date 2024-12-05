/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
    public class ItemSO : ScriptableObject {

        [Header("Data")]
        public string ItemID;
        [TextArea] public string ItemDescription;
        public ItemType ItemType;
        public RarityLevel ItemRarity;
        public Currency CurrencyRequirement;
        public int ItemValue;
        public bool Illegal;
        public int MaxStack = 1000;

        [Header("Icons")]
        public Sprite ItemIcon;

        [Header("Audio")]
        public AudioClip ItemAudioClip;

        [Header("Spawnables")]
        public GameObject spawnable;


        public Color GetRarityPrimaryColor() {
            var color = ItemRarity switch {
                RarityLevel.COMMON => ItemRarityHandler.Instance.RarityColors.Common,
                RarityLevel.UNCOMMON => ItemRarityHandler.Instance.RarityColors.Uncommon,
                RarityLevel.RARE => ItemRarityHandler.Instance.RarityColors.Rare,
                RarityLevel.VERY_RARE => ItemRarityHandler.Instance.RarityColors.VeryRare,
                RarityLevel.LEGENDARY => ItemRarityHandler.Instance.RarityColors.Legendary,
                RarityLevel.COSMIC => ItemRarityHandler.Instance.RarityColors.Cosmic,
                RarityLevel.PROTOTYPE => ItemRarityHandler.Instance.RarityColors.Prototype,
                _ => ItemRarityHandler.Instance.RarityColors.Common,
            };
            return color;
        }

        public Color GetRarityColorBorder() {
            var color = ItemRarity switch {
                RarityLevel.COMMON => ItemRarityHandler.Instance.RarityColors.CommonBorder,
                RarityLevel.UNCOMMON => ItemRarityHandler.Instance.RarityColors.UncommonBorder,
                RarityLevel.RARE => ItemRarityHandler.Instance.RarityColors.RareBorder,
                RarityLevel.VERY_RARE => ItemRarityHandler.Instance.RarityColors.VeryRareBorder,
                RarityLevel.LEGENDARY => ItemRarityHandler.Instance.RarityColors.LegendaryBorder,
                RarityLevel.COSMIC => ItemRarityHandler.Instance.RarityColors.CosmicBorder,
                RarityLevel.PROTOTYPE => ItemRarityHandler.Instance.RarityColors.PrototypeBorder,
                _ => ItemRarityHandler.Instance.RarityColors.CommonBorder,
            };
            return color;
        }

        public Color GetRaritySecondaryColor() {
            var color = ItemRarity switch {
                RarityLevel.COMMON => ItemRarityHandler.Instance.RarityColors.CommonSecondary,
                RarityLevel.UNCOMMON => ItemRarityHandler.Instance.RarityColors.UncommonSecondary,
                RarityLevel.RARE => ItemRarityHandler.Instance.RarityColors.RareSecondary,
                RarityLevel.VERY_RARE => ItemRarityHandler.Instance.RarityColors.VeryRareSecondary,
                RarityLevel.LEGENDARY => ItemRarityHandler.Instance.RarityColors.LegendarySecondary,
                RarityLevel.COSMIC => ItemRarityHandler.Instance.RarityColors.CosmicSecondary,
                RarityLevel.PROTOTYPE => ItemRarityHandler.Instance.RarityColors.PrototypeSecondary,
                _ => ItemRarityHandler.Instance.RarityColors.CommonSecondary,
            };
            return color;
        }

    }

}