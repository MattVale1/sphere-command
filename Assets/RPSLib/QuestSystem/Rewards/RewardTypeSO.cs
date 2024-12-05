/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public abstract class RewardTypeSO : ScriptableObject {

        public abstract Sprite GetIcon();

        public abstract Color GetRarityColor();
        public abstract Color GetRarityBorderColor();

    }

}