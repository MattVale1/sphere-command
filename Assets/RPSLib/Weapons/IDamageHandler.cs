/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
namespace RPSCore {

    public interface IDamageHandler {

        public void DoDamage(float amount, out bool destroySource);

    }

}