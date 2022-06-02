
using TMPro;
using UnityEngine;

public class PlayerCityUI : MonoBehaviour
{

    public TextMeshProUGUI pseudo;
    public void Init(PlayerCity pPlayerCity)
    {
        pseudo.text = "Player " + pPlayerCity.playerCityModel.playerId.ToString();
    }
}
