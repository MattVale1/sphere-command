/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using UnityEngine;

namespace RPSCore {

    [Obsolete("Not used anywhere. Only handles health. Could possibly be an Interface instead like IEnemy.", true)]
    public class Enemy : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region CACHE
        [HideInInspector]
        public Transform mTransform;
        #endregion
        #region HEALTH
        public float maxHealth;
        private Health mHealth;
        #endregion
        #endregion


        #region SETUP
        protected void Start() {
            Init();
        }

        private void Init() {
            CacheVars();
            Init_Health();
        }

        protected void OnEnable() {
            Init_Health();
        }

        private void CacheVars() {
            mTransform = GetComponent<Transform>();
        }

        public void Init_Health() {
            mHealth ??= new Health();
            mHealth.SetHealth(maxHealth);
        }
        #endregion

        #region HEALTH
        private void ReduceHealth(float damage) {
            mHealth.SetHealth(mHealth.GetHealth() - damage);
            CheckHealth();
        }

        public void CheckHealth() {
            if (mHealth.IsDead()) {

            }
        }
        #endregion

        #region PUBLIC METHODS
        public void DoDamage(float amountToApply) {
            ReduceHealth(amountToApply);
        }
        #endregion

    }

}