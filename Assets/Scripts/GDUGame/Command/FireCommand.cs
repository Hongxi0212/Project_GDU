using QPFramework;

namespace GDUGame {

   public class FireCommand: AbstractCommand {
      public static FireCommand Singleton = new FireCommand();

      protected override void OnExecute() {
         var gunSystem = this.GetSystem<IGunSystem>();

         gunSystem.CurrentGunData.BulletCount.Value--;
      }
   }
}