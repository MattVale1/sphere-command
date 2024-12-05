/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections.Generic;
using UnityEngine;

namespace RPSCore {

    public class MineableEntity : MonoBehaviour, IDamageHandler {

        #region Public Properties       

        public List<DroppableItem> ResourcesContained;        
        public int ResourcesAvailable;
        public AudioClip AudioClip;

        public float MaxHealth = 50;
        #endregion

        #region Private Properties
        private Health _health;
        #endregion


        #region Unity Flow
        private void Awake() {
            _health = new Health();
            _health.SetHealth(MaxHealth);
        }
        #endregion

        #region Private Methods
        private void MineResource() {
            float rand = Random.Range(0, 100);

            for (int i = 0; i < ResourcesContained.Count; i++) {
                if (rand <= ResourcesContained[i].ChanceOfDrop) {
                    int amountToDrop = Random.Range(ResourcesContained[i].RewardAmountMin, ResourcesContained[i].RewardAmountMax);
                    ResourcesAvailable -= amountToDrop;

                    PlayerInventoryService.Instance.AddItem(ResourcesContained[i].RewardItem, TransactionType.ADD, amountToDrop);

                    if (ResourcesAvailable <= 0) {
                        KillAsteroid();
                    }
                }
            }
        }

        private void KillAsteroid() {
            // TODO Spawn VFX

            Destroy(gameObject);
        }
        #endregion

        #region Public Methods
        public void Mine() {
            MineResource();
        }

        public void DoDamage(float amount, out bool destroySource) {
            destroySource = true;
            _health.ReduceHealth(amount);
            if (_health.IsDead()) {
                KillAsteroid();
            }
        }
        #endregion

    }

}