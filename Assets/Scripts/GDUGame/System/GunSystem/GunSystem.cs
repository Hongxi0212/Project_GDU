using QPFramework;
using System.Collections.Generic;

namespace GDUGame {

   public class GunSystem: AbstractSystem, IGunSystem {
      private GunData mCurrentGun;

      public GunData CurrentGunData {
         get {
            return mCurrentGun;
         }
         set {
            mCurrentGun = value;
         }
      }

      private List<GunData> mGunList;

      public List<GunData> AllGunsData {
         get {
            return mGunList;
         }
         set {
            mGunList = value;
         }
      }

      public void SwitchGun(int slotNum) {
         if(mGunList.Count > 0) {
            mCurrentGun = mGunList[slotNum];
         }
      }

      protected override void OnInit() {
         //To be changed when start from level
         //Impletment by Model System, loadPathString and fileName should be read by json file
         AllGunsData = new List<GunData> {
            new GunData() {
               BulletCount = new BindableProperty<int>() {
                  Value = 9999
               },
               SpareRoundsCount = new BindableProperty<int>() {
                  Value = 210
               }
            }
         };

         if(mGunList != null) {
            SwitchGun(0);
         }
      }
   }
}