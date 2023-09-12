using UnityEngine;

namespace GDUGame {
   public class Player: MonoBehaviour {
      private const float gravity = -9.81f;

      private CharacterController cc;

      private GameObject head;
      private GameObject feet;

      [SerializeField]
      private Vector3 velocity;
      private Vector3 moveDirection;
      public LayerMask GroundMask;
      private PlayerView pv = new();

      [SerializeField]
      private float moveSpeed;
      [SerializeField]
      private float jumpHeight;
      [SerializeField]
      private float rotateSensititvity;
      [SerializeField]
      private float sprint_Factor;
      private float sprint_Threshold = 2f;
      [SerializeField]
      private float groundCheckDistance;
      [SerializeField]
      private float stamina;
      private float stamina_Max = 5f;
      private float stamina_Change = 1f;
      [SerializeField]
      private float dashDistance;

      [SerializeField]
      private bool isYInvert;
      [SerializeField]
      private bool isOnGround;
      [SerializeField]
      private bool hasGravity;


      private void Awake() {
         Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;

         cc = GetComponent<CharacterController>();

         head = transform.GetChild(0).gameObject;
         feet = transform.GetChild(1).gameObject;

         velocity = Vector3.zero;
         pv.set(head.transform.localEulerAngles);
      }

      private void Update() {
         isOnGround = checkOnGround();

         if(Input.GetButtonDown("Jump")) {
            jump();
         }

         if(Input.GetKeyDown(KeyCode.LeftControl)) {
            dash();
         }

         underGravity();

         lookAt(pv.get());
      }

      private void FixedUpdate() {
         move();
      }

      private void lookAt(Vector3 vector) {
         var mouseMovement = getInputViewRotation() * rotateSensititvity;

         pv.yaw += mouseMovement.x;
         pv.pitch += mouseMovement.y;

         vector = new Vector3(Mathf.Clamp(vector.x, -30, 60), vector.y, vector.z);

         head.transform.eulerAngles = new Vector3(vector.x, vector.y, vector.z);
         transform.eulerAngles = new Vector3(0f, vector.y, 0f);
      }

      private void move() {
         moveDirection = getInputMoveDirection();

         moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);

         if(Input.GetKey(KeyCode.LeftShift) && stamina > 0) {
            moveDirection *= sprint_Factor;

            stamina -= stamina_Change * Time.deltaTime;
         }
         else if(!Input.GetKey(KeyCode.LeftShift) && stamina < stamina_Max) {
            stamina += stamina_Change * Time.deltaTime;
         }

         if(stamina > stamina_Max) {
            stamina = stamina_Max;
         }

         cc.Move(moveDirection * moveSpeed);
      }

      private void underGravity() {
         if(hasGravity && !isOnGround) {
            velocity.y += gravity * Time.deltaTime;
            cc.Move(0.5f * velocity * Time.deltaTime);
         }
         else if(velocity.y < 0 && isOnGround) {
            velocity.y = -1f;
         }
      }

      private void jump() {
         if(isOnGround) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            cc.Move(0.5f * velocity * Time.deltaTime);
         }
      }

      private void dash() {
         cc.Move(moveDirection * dashDistance);
      }

      private bool checkOnGround() {
         return Physics.CheckSphere(feet.transform.position, groundCheckDistance, GroundMask);
      }

      private Vector3 getInputMoveDirection() {
         Vector3 direction = Vector3.zero;
         direction = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

         return direction;
      }

      private Vector2 getInputViewRotation() {
         if(!isYInvert) {
            return new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
         }
         else {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
         }
      }

      struct PlayerView {
         public float pitch;
         public float yaw;
         public float roll;

         public void set(Vector3 v) {
            pitch = v.x;
            yaw = v.y;
            roll = v.z;
         }

         public Vector3 get() {
            return new Vector3(pitch, yaw, roll);
         }
      }
   }
}