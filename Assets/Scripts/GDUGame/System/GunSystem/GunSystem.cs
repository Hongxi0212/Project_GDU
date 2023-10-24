using QPFramework;
using System.Collections.Generic;

namespace GDUGame {
   /// <summary>
   /// This is a system used to manage GunData when playing Game
   /// Notice: the "GunData" Gundata specifically refers to
   ///         the dynamic game data in the level
   ///         not static, game configuration data outside the level
   /// </summary>
   public class GunSystem: AbstractSystem, IGunSystem {
      private Gun mCurrentGun;

      public Gun CurrentGun {
         get {
            return mCurrentGun;
         }
         set {
            mCurrentGun = value;
         }
      }

      private GunData mCurrentGunData;

      public GunData CurrentGunData {
         get {
            return mCurrentGunData;
         }
         set {
            mCurrentGunData = value;
         }
      }

      private List<Gun> mAllGuns;

      public List<Gun> AllGuns {
         get {
            return mAllGuns;
         }
         set {
            mAllGuns = value;
         }
      }

      public void RegisterGun(Gun gun) {
         AllGuns.Add(gun);
      }

      /// <summary>
      /// Change current GunData to specific Gun
      /// parameter slotNum refers to the index of the four guns
      /// owned by the player in the equipment slots
      /// 
      /// NOTICE: The Parameter SlotNum Should be in Range of 0-3
      /// </summary>
      /// <param name="slotNum"></param>
      public void SwitchGun(int slotNum) {
         if(AllGuns.Count > 0) {
            CurrentGun = AllGuns[slotNum];
            CurrentGunData = CurrentGun.GunData;
         }
      }

      protected override void OnInit() {
         //To be changed when start from level
         //Implement by Model System, loadPathString and fileName should be read by json file

         if(mAllGuns != null) {
            SwitchGun(0);
         }
      }
   }
}