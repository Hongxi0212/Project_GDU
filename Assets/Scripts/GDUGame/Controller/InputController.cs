using QPFramework;
using System.Collections;
using UnityEngine;

namespace GDUGame {
   /// <summary>
   /// Collect All Input from Player and send Event to System
   /// 
   /// NOTICE: This Class should be applied to an GameObject in Scene
   /// </summary>
   public class InputController: GDUController {
      private float mouseInputRate = 0.01f;
      private float mouseInputTime = 0f;

      private void Update() {
         if(Input.GetMouseButton(0)) {
            if(Time.time > mouseInputTime) {
               this.SendEvent(InputInfo.Mouse_Left);
               mouseInputTime = Time.time + mouseInputRate;
            }
         }
      }

   }
}