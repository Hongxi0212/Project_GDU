using UnityEngine;

namespace GDUGame {
   /// <summary>
   /// Bullet shoot by Gun
   /// </summary>
   /// <seealso cref="GDUGame.GDUController" />
   public class Bullet: GDUController {
      private Rigidbody rb;

      public float speed;
      public float damage;

      private void Awake() {
         rb = GetComponent<Rigidbody>();

         Destroy(gameObject, 5f);
      }

      /// <summary>
      /// When Bullet Inst in Gun, it will be triggered
      /// parameter motion refers to the direction of moving of bullet (also including scale
      /// 
      /// NOTICE: The Parameter Motion Should be Assigned by trasform of Bullet Instance
      ///         Obtained from Gun Instance
      /// </summary>
      /// <param name="motion">The motion.</param>
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