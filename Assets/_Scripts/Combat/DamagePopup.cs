using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] Vector3 _moveUpSpeed = new Vector3(0, 20f, 0);
    [SerializeField] float _lifeDuration = 3;
    [SerializeField] float _disappearSpeed = .5f;
    [SerializeField] TMP_Text _txtScpt;
    [SerializeField] Color _txtCol;


    void Update()
    {
        transform.position += _moveUpSpeed * Time.deltaTime;
        _lifeDuration -= Time.deltaTime;
        if (_lifeDuration <= 0)
        {
            _txtCol.a -= _disappearSpeed * Time.deltaTime;
            _txtScpt.color = _txtCol;
            if (_txtCol.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

}