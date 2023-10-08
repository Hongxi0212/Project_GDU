using QPFramework;

namespace GDUGame {
   /// <summary>
   /// Command Used by Player to inform GunSystem of Fire
   /// 
   /// GunSystem will reduce one bullet count after recieving this command.
   /// </summary>
   /// <seealso cref="QPFramework.AbstractCommand" />
   public class FireCommand: AbstractCommand {
      public static FireCommand Singleton = new FireCommand();

      protected override void OnExecute() {
         var gunSystem = this.GetSystem<IGunSystem>();

         gunSystem.CurrentGunData.BulletCount.Value--;
      }
   }
}