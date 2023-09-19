using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goinCoin, healthCoin, staminaGlobe;

    public void DropItem()
    {
        int randomNum = Random.Range(1, 10);
        if (randomNum == 1)
        {
            ObjectPool.Instance.GetObject(healthCoin, transform.position, Quaternion.identity);

            //Instantiate(healthCoin, transform.position, Quaternion.identity);
        }
        if (randomNum == 2 || randomNum == 3)
        {
            ObjectPool.Instance.GetObject(staminaGlobe, transform.position, Quaternion.identity);

            //Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }
        if (randomNum > 3)
        {
            int randomAmountOfGold = Random.Range(1, 4);

            for (int i = 0; i < randomAmountOfGold; i++)
            {
                ObjectPool.Instance.GetObject(goinCoin, transform.position, Quaternion.identity);

                //Instantiate(goinCoin, transform.position, Quaternion.identity);
            }
        }

    }
}
