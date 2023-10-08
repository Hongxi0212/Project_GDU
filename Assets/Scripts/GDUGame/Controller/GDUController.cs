using QPFramework;
using UnityEngine;

namespace GDUGame {
   /// <summary>
   /// Inherited from MonoBehaviour so that this class can be attached to GameObject in Scene
   /// Implement Interface IController leads this class to use Model, System, Query, Command and Event
   /// 
   /// All Scripts applied to GameObject in Scene should consider being inherited from this Class
   /// 
   /// TOBE: Considering Reduce ability of this Class and downgrade it to Viewer instead of Controller
   /// </summary>
   /// <seealso cref="UnityEngine.MonoBehaviour" />
   /// <seealso cref="QPFramework.IController" />
   public class GDUController: MonoBehaviour, IController {

      public IFactory GetFactory() {
         return GDUGame.Line;
      }
   }
}