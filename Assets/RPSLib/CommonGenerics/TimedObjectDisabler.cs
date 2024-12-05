/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class TimedObjectDisabler : MonoBehaviour {

        public bool ShouldDestroy = false;
        public bool WaitForAudioSourceToFinish = true;
        public AudioSource AudioSource;

        public DisableMethod disableMethod;
        public enum DisableMethod {
            Time, Distance, TimeAndDistance
        }

        public float timeToDisable;

        private Transform _transform;
        private Vector3 _originPoint;
        private float _disableDistance;


        public void SetDisableDistance(float dist) {
            _disableDistance = dist;
        }

        private void OnEnable() {
            CancelInvoke();

            if (disableMethod == DisableMethod.Time || disableMethod == DisableMethod.TimeAndDistance) {
                if (AudioSource != null && WaitForAudioSourceToFinish) {
                    timeToDisable = AudioSource.clip.length;
                }
                Invoke(nameof(DisableObject), timeToDisable);
            }

            if (disableMethod == DisableMethod.Distance || disableMethod == DisableMethod.TimeAndDistance) {
                if (_transform == null)
                    _transform = transform;
                _originPoint = _transform.position;
            }

        }

        private void Update() {
            if (disableMethod == DisableMethod.Distance || disableMethod == DisableMethod.TimeAndDistance) {
                if (Vector3.Distance(_originPoint, _transform.position) > _disableDistance) {
                    DisableObject();
                }
            }
        }

        private void DisableObject() {
            if (ShouldDestroy) {
                Destroy(gameObject);
                return;
            }

            gameObject.SetActive(false);
        }

    }

}