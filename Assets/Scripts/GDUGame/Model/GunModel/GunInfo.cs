namespace QPFramework {
   public class GunInfo {
      public string Name { get; set; }

      public int BulletMaxCount { get; set; }

      public float Attack { get; set; }

      public float Frequency { get; set; }

      public float ShootDistance { get; set; }

      public bool NeedBullet { get; set; }

      public float ReloadSeconds { get; set; }

      public string Description { get; set; }

      public GunInfo() { 

      }

      public GunInfo(string name, string description, bool needBullet, int bulletMaxCount,
         float attack, float frequency, float shootDistance, float reloadSeconds) {

         Name = name;
         Description = description;
         NeedBullet = needBullet;
         BulletMaxCount = bulletMaxCount;
         Attack = attack;
         Frequency = frequency;
         ShootDistance = shootDistance;
         ReloadSeconds = reloadSeconds;
      }

      public override string ToString() {
         return "This Gun (" + Name + ") has at most " + BulletMaxCount + " bullets, " +
            "its attack value is " + Attack + ", frequency is " + Frequency + 
            ", shoot distance is " + ShootDistance + ", reload seconds is " + ReloadSeconds + 
            ". Its description as follows: " + Description;
      }
   }
}