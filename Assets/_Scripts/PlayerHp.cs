using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [SerializeField] int _maxHP;
    public int _currentHP { get; private set; }

}
