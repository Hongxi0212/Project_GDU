using System.Collections.Generic;

namespace QPFramework {
   public class GunModel: AbstractModel, IGunModel {
      private Dictionary<string, GunInfo> allGunInfos = new Dictionary<string, GunInfo>() {
            { "AK-47", new GunInfo("AK-47", "Test Weapon", true, 30, 50f, 0.2f, 100f, 2f) },
        };

      public GunInfo GetGunInfoByName(string name) {
         var info = new GunInfo();
         if(allGunInfos.TryGetValue(name, out info)) {
            return info;
         }
         else {
            //think about implement Exception System in future
            return null;
         }
      }

      public List<GunInfo> GetAllGunInfos() {
         var list= new List<GunInfo>();

         foreach(var info in allGunInfos) {
            list.Add(info.Value);
         }
        
         return list;
      }

      protected override void OnInit() {

      }
   }
}