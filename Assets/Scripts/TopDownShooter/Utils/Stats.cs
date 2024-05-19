namespace TDS
{
    public class Stats
    {
        public int HP;
        public float Speed;
        public float AttackSpeed;

        public void Initialize(int hp, float spd, float atkspd)
        {
            HP = hp;
            Speed = spd;
            AttackSpeed = atkspd;
        }
    }
}
