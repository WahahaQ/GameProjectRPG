using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds wave data.

[System.Serializable]
public class Wave 
{
	public EnemyType[] enemies;
	public float[] spawnRates;
	public bool spawnBoss;
}
