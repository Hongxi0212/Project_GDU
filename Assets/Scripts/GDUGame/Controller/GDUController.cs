using QPFramework;
using UnityEngine;

namespace GDUGame {

   public class GDUController: MonoBehaviour, IController {

      public IFactory GetFactory() {
         return GDUGame.Line;
      }
   }
}