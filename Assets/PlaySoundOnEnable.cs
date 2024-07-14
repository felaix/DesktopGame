using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{

    public string _sfxName;

    private void OnEnable()
    {
        SoundManager.Instance.PlaySFX( _sfxName );
    }

}
