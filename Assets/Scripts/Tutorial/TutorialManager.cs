using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TutorialHandler(TutorialState currentState, TutorialState changedState);

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<CutSceneActivator> _cutSceneActivators;
    [SerializeField] private TutorialState _currentState;
    public static TutorialManager Instance { get; private set; }
    private event TutorialHandler _onStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public bool HasState(TutorialState state)
    {
        return (_currentState & state) == state;
    }

    private void AddState(TutorialState state)
    {
        _currentState |= state;
        OnStateChanged(state);
    }

    private void DeleteState(TutorialState state)
    {
        _currentState &= ~state;
        OnStateChanged(state);
    }

    private void OnStateChanged(TutorialState changedState)
    {
        StartCoroutine(OnStateChangedCoroutine(changedState));
    }

    private IEnumerator OnStateChangedCoroutine(TutorialState changedState)
    {
        yield return null;

        _onStateChanged?.Invoke(_currentState, changedState);
    }

    public void ActivateTutorial(TutorialState type)
    {
        if (HasState(type)) { return; }

        _cutSceneActivators[(int)Math.Log((double)type, 2.0)].Activate();
        AddState(type);
    }
    
    public bool HasCompletedTutorial()
    {
        TutorialState allStates = 0;
        foreach (TutorialState state in Enum.GetValues(typeof(TutorialState)))
        {
            allStates |= state;
        }
        return HasState(allStates);
    }

    public void Subscribe(TutorialHandler handler) { _onStateChanged += handler; }
    public void Unsubscribe(TutorialHandler handler) { _onStateChanged -= handler; }
}

[Flags]
public enum TutorialState
{
    Move = 1,
    Interact = 1 << 1,
    GameGoal = 1 << 2,
    Trap = 1 << 3,
    InventoryOpen = 1 << 4,
    InventoryUse = 1 << 5,
    Puzzle = 1 << 6,
    GetEther = 1 << 7
}