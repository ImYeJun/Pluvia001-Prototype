using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    private IInteractable _interactObject;
    private List<Collider2D> _currentColliders = new List<Collider2D>();
    private PlayerState _currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        _currentState = PlayerState.Idle;
    }


    private void Update()
    {
        if (_currentState == PlayerState.Idle)
        {
            UpdateClosestInteractable();

            //* 상호작용 키를 누르면, 플레이어와 가장 가까이 있는 상호작용 물체와 상호작용한다.
            if (_interactObject != null && Input.GetKeyDown(KeyCode.F))
            {
                _interactObject.Interact();
            }
        }
    }

    //* 상호작용 범위 내에 있는 상호작용 가능 물체 중에서, 플레이어와 가장 가까운 물체가 무엇인지 업데이트 하는 함수이다.
    private void UpdateClosestInteractable()
    {
        float shortestDistance = float.MaxValue;
        IInteractable closest = null;

        foreach (var col in _currentColliders)
        {
            if (col == null) continue;

            float distance = Vector2.Distance(transform.position, col.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = col.GetComponent<IInteractable>();
            }
        }

        _interactObject = closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (!_currentColliders.Contains(collision))
            {
                _currentColliders.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            _currentColliders.Remove(collision);

            if (collision.GetComponent<IInteractable>() == _interactObject)
            {
                _interactObject = null;
            }
        }
    }
    
    public void SetState(PlayerState playerState) { _currentState = playerState; }
}

