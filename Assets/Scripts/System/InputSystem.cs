using QFramework;
using UnityEngine;

namespace GDUGame {
   public interface IInputSystem: ISystem {

   }

   public class InputSystem: AbstractSystem, IInputSystem {
      protected override void OnInit() {

      }

   }
}