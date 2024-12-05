/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [CreateAssetMenu(fileName = "New Item", menuName = "Currencies/New Item")]
    public class CurrencySO : ScriptableObject {

        [Header("Data")]
        public string ItemID;
        [TextArea] public string ItemDescription;
        public RarityLevel ItemRarity;
        public Currency Currency;
        public Color CurrencyColor;
        public int ItemValue;
        public bool CanBeInDebt;

        [Header("Icons")]
        public Sprite CurrencyIcon;

    }

}