using UnityEngine;

[RequireComponent(typeof(CutSceneActivator))]
public class GameEnder : InteractableObject
{
    private CutSceneActivator _cutSceneActivator;
    [SerializeField] private TutorialState _checkingTutorials;

    protected override void Awake()
    {
        _cutSceneActivator = GetComponent<CutSceneActivator>();

        base.Awake();
    }

    protected override void OnInteract()
    {
        if (TutorialManager.Instance.HasCompletedTutorial())
        {
            UnsetInteractable();
            PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);
            GameSceneManager.Instance.LoadScene(SceneId.Ending);
        }
        else
        {
            _cutSceneActivator.Activate();
        }
    }
}
