/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public abstract class BulletBase : MonoBehaviour {

        public abstract float Damage { get; set; }

        public abstract void OnTriggerEnter(Collider collisionData);

        public abstract void OnCollisionEnter(Collision collisionData);

    }

}