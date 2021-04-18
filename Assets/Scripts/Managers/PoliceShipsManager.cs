using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PoliceShipPool))]
public class PoliceShipsManager: MonoBehaviour
{
    private PoliceShipPool _policeShipsPool;

    private void Awake()
    {
        _policeShipsPool = GetComponent<PoliceShipPool>();
    }

    public IEnumerator SpawnPoliceShips(int amount, float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);

        int policeShipAmount = 0;
        while (policeShipAmount < amount)
        {
            PoliceShip policeShip = _policeShipsPool.GetAvailableObject();

            policeShip.transform.position = GameManager.FindGoodLocation();
            policeShip.gameObject.SetActive(true);
            policeShipAmount++;
            yield return new WaitForSeconds(reloadTime);
        }
    }
}
