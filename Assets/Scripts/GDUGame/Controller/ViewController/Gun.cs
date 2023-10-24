using QPFramework;
using System.Numerics;

namespace GDUGame {
   /// <summary>
   /// Represent Current State of Gun
   /// </summary>
   public enum GunState {
      Idle = 0,
      Shooting = 1,
      Reloading = 10,
      Depleted = 11,
      Cooling = 20
   }

   /// <summary>
   /// Implement logic of Gameplay about Gun in Scene 
   /// </summary>
   /// <seealso cref="GDUGame.GDUController" />
   public class Gun: GDUController {
      private Bullet bullet;

      private int capacity;

      public GunData GunData;

      public BindableProperty<string> Name;

      public BindableProperty<GunState> State;

      private void Awake() {
         bullet = transform.Find("Bullet").GetComponent<Bullet>();

         Name.Value = name.Split('(')[0];

         //不对，Controller不应该直接获得Model
         var info= this.GetModel<IGunModel>().GetGunInfoByName(Name.Value);

         GunData = new GunData() {
            BulletCount = new BindableProperty<int>() {
               Value = info.BulletMaxCount
            },
            SpareRoundsCount = new BindableProperty<int>() {
               Value = 9999
            }
         };

         State = new BindableProperty<GunState>() {
            Value = GunState.Idle,
         };

         this.GetSystem<IGunSystem>().RegisterGun(this);
      }

      /// <summary>
      /// Gun Shoot, implement by Inst Bullet Instance GameObject
      /// </summary>
      public void Shoot() {
         if(GunData.BulletCount.Value > 0 && State.Value == GunState.Idle) {
            //State Changing should be instead of using TimeSystem with Fire Frequency
            State.Value = GunState.Shooting;
            var b = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation);
            b.gameObject.SetActive(true);

            b.transform.localScale = bullet.transform.localScale;
            b.Trigger(b.transform);
         }
      }

      /// <summary>
      /// Gun Reload, Checking bullet count in gun and spare rounds count
      /// </summary>
      public void Reload() {
         if(GunData.BulletCount.Value < capacity && State.Value == GunState.Idle) {
            if(GunData.SpareRoundsCount.Value > 0) {
               var needBulletCount = capacity - GunData.BulletCount.Value;

               if(needBulletCount > 0) {
                  //Using TimeSystem to implement reload

                  //Remember to change state in ReloadCommand
               }
            }
         }
      }

      public void CoolDown() {
         State.Value = GunState.Idle;
      }
   }
}