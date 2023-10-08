using QPFramework;
using UnityEngine;

namespace GDUGame {

   public class InputController: GDUController {

      private void Update() {
         if(Input.GetMouseButton(0)) {
            this.SendEvent(InputInfo.Mouse_Left);
         }
      }
   }
}