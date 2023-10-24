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

         gunSystem.CurrentGun.Shoot();
         gunSystem.CurrentGun.GunData.BulletCount.Value--;

         var gunInfo = this.GetModel<IGunModel>().GetGunInfoByName(gunSystem.CurrentGun.Name.Value);

         this.GetSystem<ITimeSystem>().AddDelayTask(1f / gunInfo.Frequency,
            () => {
               gunSystem.CurrentGun.CoolDown();
            });
      }
   }
}