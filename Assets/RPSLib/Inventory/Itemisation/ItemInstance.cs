/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
namespace RPSCore {

    [System.Serializable]
    public class ItemInstance {

        public ItemSO Data;

        public ItemInstance(ItemSO item) {
            Data = item;
        }

    }

}