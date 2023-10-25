using QPFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

      private Dictionary<Gun, GunData> mAllGunwithDatas = new Dictionary<Gun, GunData>();

      public Dictionary<Gun, GunData> AllGunwithDatas {
         get {
            return mAllGunwithDatas;
         }
      }

      public void OptInGun(Gun gun) {
         var info=this.GetModel<IGunModel>().GetGunInfoByName(gun.Name.Value);

         AllGunwithDatas.Add(gun, new GunData() {
            BulletCount=new BindableProperty<int>() {
               //Value=info.BulletMaxCount
               Value=99999
            },
            SpareRoundsCount=new BindableProperty<int>() {
               Value=9999
            }
         });

         SwitchGun(0);
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
         if(AllGunwithDatas.Count > 0) {
            CurrentGun = AllGunwithDatas.ElementAt(0).Key;
            CurrentGunData = AllGunwithDatas.ElementAt(0).Value;
         }
      }

      protected override void OnInit() {
         //To be changed when start from level
         //Implement by Model System, loadPathString and fileName should be read by json file

         if(AllGunwithDatas != null) {
            SwitchGun(0);
         }
      }
   }
}