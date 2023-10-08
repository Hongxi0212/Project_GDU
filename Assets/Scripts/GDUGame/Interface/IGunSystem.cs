using QPFramework;
using System.Collections.Generic;

namespace GDUGame {

   public interface IGunSystem: ISystem {
      GunData CurrentGunData { get; set; }

      List<GunData> AllGunsData { get; }

      void SwitchGun(int slotNum);
   }
}