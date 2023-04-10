using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] Vector3 _moveUpSpeed = new Vector3(0, 20f, 0);
    [SerializeField] float _maxLifeDuration = 1;
    float _lifeDuration;
    [SerializeField] float _disappearSpeed = .5f;
    [SerializeField] TMP_Text _txtScpt;
    [SerializeField] Color _txtCol;
    [SerializeField] float _increaseScaleAmount = 1.12f;
    [SerializeField] float _decreaseScaleAmount = .92f;

    void Start()
    {
        _lifeDuration = _maxLifeDuration;
    }

    void Update()
    {
        transform.position += _moveUpSpeed * Time.deltaTime;
        _lifeDuration -= Time.deltaTime;

        if (_lifeDuration > _maxLifeDuration * .5f) transform.localScale += Vector3.one * _increaseScaleAmount * Time.deltaTime;
        else transform.localScale -= Vector3.one * _decreaseScaleAmount * Time.deltaTime;


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