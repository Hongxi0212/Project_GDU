using QPFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QPFramework {
   public interface IGunModel: IModel {
      GunInfo GetGunInfoByName(string name);

      List<GunInfo> GetAllGunInfos();
   }
}