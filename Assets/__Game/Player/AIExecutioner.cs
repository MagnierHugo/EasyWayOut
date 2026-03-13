using System.Globalization;
using System.Runtime.ExceptionServices;
using UnityEngine;

public static class AIExecutioner
{
    public static void ExecuteAI(AIPersonality personality, Player opponent, bool canUseSpecial, Gun gun)
    {
        Mag mag = gun.GetMag();
        float chanceToDie = (mag.GetNumberOfBullets() / mag.GetNumberOfChamberLeft());

        switch (personality)
        {

            case AIPersonality.Maniac:
                if (Random.value > 0.5f) opponent.ShootSelf();
                else opponent.ShootOpponent();
                break;

            case AIPersonality.Thug:
                if (canUseSpecial) opponent.UseSpecial();
                else opponent.ShootOpponent();
                break;

            case AIPersonality.Coward:
                if (chanceToDie < 0.4f) opponent.ShootSelf();
                else if (chanceToDie < 0.7f && canUseSpecial) opponent.UseSpecial();
                else opponent.ShootOpponent();
                break;


            case AIPersonality.Gambler:
                if (chanceToDie < 0.75f) opponent.ShootSelf(); else opponent.ShootOpponent();
                break;

            case AIPersonality.Calculator:
                float[] scores = new float[3];
                scores = CalculateBestOption(gun, canUseSpecial, mag.GetNumberOfBullets(), mag.GetNumberOfChamberLeft());

                int bestActionIndex = 0;
                float highestScore = scores[0];

                for (int i = 1; i < scores.Length; i++)
                {
                    if (scores[i] > highestScore)
                    {
                        highestScore = scores[i];
                        bestActionIndex = i;
                    }
                }

                switch (bestActionIndex)
                {
                    case 0:
                        opponent.ShootSelf();
                        break;
                    case 1:
                        opponent.ShootOpponent();
                        break;
                    case 2:
                        opponent.UseSpecial();
                        break;
                }
                break;
        }
    }

    private static float[] CalculateBestOption(Gun gun, bool canUseSpecial, float bullets, float chamberLeft)
    {
        float[] scores = new float[3];

        float pDeath = bullets / chamberLeft;

        if (!canUseSpecial)
        {
            scores[0] = 1f - pDeath;
            scores[1] = pDeath;
            scores[2] = 0f;
            return scores;
        }

        if (gun.GetComponent<Burst>() != null)
        {
            scores[0] = 1f;
            scores[1] = 0f;
            scores[2] = 0f;
        }

        else if (gun.GetComponent<ShotGun>() != null)
        {
            scores[0] = 1f - pDeath;
            scores[1] = pDeath;
            scores[2] = 0f;
        }

        else if (gun.GetComponent<Nailgun>() != null)
        {
            scores[0] = 1f - pDeath;
            scores[1] = 0f;
            scores[2] = pDeath - 0.25f;
        }

        else if (gun.TryGetComponent<DoubleBarrel>(out DoubleBarrel barrel))
        {
            float pDeathOther = (float)bullets / barrel.GetOtherMag().GetNumberOfChamberLeft();

            if (pDeath > pDeathOther && canUseSpecial)
            {
                scores[0] = 0f;
                scores[1] = 0f;
                scores[2] = 1f;
            }
            else
            {
                scores[0] = 1f - pDeath;
                scores[1] = pDeath;
                scores[2] = 0f;
            }
        }

        return scores;
    }
}
