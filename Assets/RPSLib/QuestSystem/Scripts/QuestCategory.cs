/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [System.Serializable]
    public class QuestCategory {

        public QuestCategories category;
        public Sprite categoryIcon;

    }

    public enum QuestCategories {
        MAIN,
        CORPO,
        BOUNTY,
        RESCUE,
        DELIVERY,
        EXPLORATION,
        MINING,
        ESCORT
    }

}