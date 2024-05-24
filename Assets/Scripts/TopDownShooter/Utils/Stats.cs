using System;

namespace TDS
{
    [Serializable]
    public class Stats
    {
        public int HP;
        public int MaxHP;
        public float Speed;
        public float AttackSpeed;
        public int NumOfBullets;
        public int Luck;
        public int Coins;

        public void Initialize(int hp, float spd, float atkspd, int maxhp, int numOfBullets = 1, int luck = 0, int coins = 0)
        {
            HP = hp;
            MaxHP = maxhp;
            Speed = spd;
            AttackSpeed = atkspd;
            NumOfBullets = numOfBullets;
            Luck = luck;
            Coins = coins;
        }
    }
}
