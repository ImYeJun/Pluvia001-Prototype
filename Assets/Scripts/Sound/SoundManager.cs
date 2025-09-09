using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private BgmManager _bgmManager;
    [SerializeField] private SoundEffectManager _soundEffectManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBgm(AudioClip audioClip, bool isLoop = true)
    {
        if (audioClip == null)
        {
            Debug.LogError("AudioClip is Null");
            return;
        }
        _bgmManager.Play(audioClip, isLoop);
    }
    public void StopBgm() { _bgmManager.Stop(); }
    public void PauseBgm() { _bgmManager.Pause(); }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogError("AudioClip is Null");
            return;
        }
        _soundEffectManager.Play(audioClip);
    }
    
    public void PlaySoundEffectWithRandomPich(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogError("AudioClip is Null");
            return;
        }
        _soundEffectManager.PlayWithRandomPitch(audioClip);
    }

    public void SetBgmVolume(float volume) { _bgmManager.SetVolume(volume); }
    public void SetSoundEffectVolume(float volume) { _soundEffectManager.SetVolume(volume); }

    public float GetBgmVolume() { return _bgmManager.GetVolume(); }
    public float GetSoundEffectVolume() { return _soundEffectManager.GetVolume(); }

    //! Test
    // [SerializeField] private AudioClip _testBgm;
    // [SerializeField] private AudioClip _testSoundEffect;
    // [SerializeField] private AudioClip _testSoundEffectWithRandomPitch;
    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         PlayBgm(_testBgm);
    //     }
    //     if (Input.GetKeyDown(KeyCode.X))
    //     {
    //         PlaySoundEffect(_testSoundEffect);
    //     }
    //     if (Input.GetKeyDown(KeyCode.C))
    //     {
    //         PlaySoundEffectWithRandomPich(_testSoundEffectWithRandomPitch);
    //     }
    // }
}
