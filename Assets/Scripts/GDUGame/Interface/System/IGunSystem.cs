using QPFramework;
using System.Collections.Generic;

namespace GDUGame {

   public interface IGunSystem: ISystem {
      Gun CurrentGun { get; set; }

      GunData CurrentGunData { get; set; }

      List<Gun> AllGuns { get; }

      void RegisterGun(Gun gun);

      void SwitchGun(int slotNum);
   }
}