/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [CreateAssetMenu(fileName = "New Item", menuName = "Itemisation/New Rarity Color Container")]
    public class RarityColorSO : ScriptableObject {

        [Header("Common")]
        public Color Common;
        public Color CommonBorder;
        public Color CommonSecondary;

        [Header("Uncommon")]
        public Color Uncommon;
        public Color UncommonBorder;
        public Color UncommonSecondary;

        [Header("Rare")]
        public Color Rare;
        public Color RareBorder;
        public Color RareSecondary;

        [Header("VeryRare")]
        public Color VeryRare;
        public Color VeryRareBorder;
        public Color VeryRareSecondary;

        [Header("Legendary")]
        public Color Legendary;
        public Color LegendaryBorder;
        public Color LegendarySecondary;

        [Header("Cosmic")]
        public Color Cosmic;
        public Color CosmicBorder;
        public Color CosmicSecondary;

        [Header("Prototype")]
        public Color Prototype;
        public Color PrototypeBorder;
        public Color PrototypeSecondary;

        [Header("Illegal")]
        public Color IllegalBorder;

    }

}