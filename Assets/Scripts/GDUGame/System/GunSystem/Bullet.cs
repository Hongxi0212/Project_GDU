using UnityEngine;

namespace GDUGame {

   public class Bullet: GDUController {
      private Rigidbody rb;

      public float speed;
      public float damage;

      private void Awake() {
         rb = GetComponent<Rigidbody>();

         Destroy(gameObject, 5f);
      }

      public void Trigger(Transform motion) {
         rb.transform.position = motion.position;
         rb.transform.rotation = motion.rotation;

         //to be test
         rb.velocity = transform.up * speed;
         Debug.Log(rb.velocity);
      }

      private void OnCollisionEnter(Collision collision) {
         if(collision.gameObject.CompareTag("Enemy")) {
            //Send Damage Command

            Destroy(gameObject);
         }
      }
   }
}