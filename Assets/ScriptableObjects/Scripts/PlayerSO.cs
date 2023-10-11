using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//생성 이름&경로 지정
[CreateAssetMenu(fileName = "Player", menuName = "Charcters/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField] public PlayerAirData AirData { get; private set; }
}
