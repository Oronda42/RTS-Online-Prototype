using RTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    public TextMeshProUGUI nicknametext;

    public void Init(PlayerModel pModel)
    {
        nicknametext.text = pModel.nickname;
    }


}

