/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
namespace RPSCore {

    public class Health {

        private float CurrentHealth {
            get; set;
        }


        /// <summary>
        /// Set the Health of this object.
        /// </summary>
        /// <param name="value">New health value.</param>
        public void SetHealth(float value) {
            CurrentHealth = value;
        }

        /// <summary>
        /// Get the Health of this object.
        /// </summary>
        /// <returns>Health value as float.</returns>
        public float GetHealth() {
            return CurrentHealth;
        }

        /// <summary>
        /// Reduce the health of this object.
        /// </summary>
        /// <param name="amount">Amount to remove from current health.</param>
        public void ReduceHealth(float amount) {
            SetHealth(GetHealth() - amount);
        }

        /// <summary>
        /// Is this object dead?
        /// </summary>
        /// <returns>bool value representing dead state.</returns>
        public bool IsDead() {
            if (CurrentHealth <= 0) {
                return true;
            } else {
                return false;
            }
        }

    }

}