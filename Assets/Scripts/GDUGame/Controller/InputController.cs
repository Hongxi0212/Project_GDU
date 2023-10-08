using QPFramework;
using UnityEngine;

namespace GDUGame {
   /// <summary>
   /// Collect All Input from Player and send Event to System
   /// 
   /// NOTICE: This Class should be applied to an GameObject in Scene
   /// </summary>
   public class InputController: GDUController {

      private void Update() {
         if(Input.GetMouseButton(0)) {
            this.SendEvent(InputInfo.Mouse_Left);
         }
      }
   }
}