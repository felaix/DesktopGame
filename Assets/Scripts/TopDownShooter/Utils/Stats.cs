namespace TDS
{
    public class Stats
    {
        public int HP;
        public int MaxHP;
        public float Speed;
        public float AttackSpeed;
        public int NumOfBullets;

        public void Initialize(int hp, float spd, float atkspd, int maxhp, int numOfBullets = 1)
        {
            HP = hp;
            MaxHP = maxhp;
            Speed = spd;
            AttackSpeed = atkspd;
            NumOfBullets = numOfBullets;
        }
    }
}
