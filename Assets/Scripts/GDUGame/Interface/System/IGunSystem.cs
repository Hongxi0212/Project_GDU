using QPFramework;
using System.Collections.Generic;

namespace GDUGame {

   public interface IGunSystem: ISystem {
      Gun CurrentGun { get; set; }

      GunData CurrentGunData { get; set; }

      Dictionary<Gun, GunData> AllGunwithDatas { get; }

      void OptInGun(Gun gun);

      void SwitchGun(int slotNum);
   }
}