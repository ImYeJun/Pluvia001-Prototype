using System.Collections;
using UnityEngine;

public class DartTrap : TrapLogic
{
    [SerializeField] private GameObject _dartPrefab;
    [SerializeField] private GameObject _dartDestoryer;
    [SerializeField] private float _shootInterval;
    [SerializeField] private bool _isFlip;

    private Coroutine _currentShootCoroutine;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _currentShootCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if ( _currentShootCoroutine != null) { StopCoroutine(_currentShootCoroutine); }
        }
        
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            GameObject dart = Instantiate(_dartPrefab, transform.position, Quaternion.identity);
            dart.transform.SetParent(gameObject.transform);
            dart.GetComponent<Dart>().StartMove(GiveDamage, _isFlip, _dartDestoryer);

            yield return new WaitForSeconds(_shootInterval);
        }
    }
}
