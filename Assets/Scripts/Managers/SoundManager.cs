using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>, IManager
{
    private SoundObject BGMAudioObject;
    private float current, percent;

    public AudioClip BGM;

    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    private SoundObject prevAudio;

    public AudioClip hitSound;
    public AudioClip dieSound;

    public Pool soundPool;

    public void Init()
    {
        //if (transform.GetChild(0).TryGetComponent<Pool>(out soundPool))
        //    soundPool?.Init();

        GetSoundValue();

        if (BGM != null)
            ChangeBGM(BGM);

        //Monster.OnMonsterHit += (monster) => { if (hitSound) PlaySound(hitSound); };
        //Monster.OnMonsterDie += (monster) => { if (dieSound) PlaySound(dieSound); };
    }


    public void StopSound()
    {
        if (!prevAudio) return;

        if (prevAudio.gameObject.activeSelf)
            prevAudio.Stop();
        //for (int i = 0; i < PoolManager.Instance.soundPool.transform.childCount; i++)
        //{
        //    Transform t = PoolManager.Instance.soundPool.transform.GetChild(i);

        //    if (t.TryGetComponent<SoundObject>(out var sound))
        //    {
        //        if (sound == BGMAudioObject)
        //            continue;
        //        sound.ReturnToPool();
        //    }
        //}
    }

    public void ChangeBGMOneTime()
    {
        StartCoroutine("ChangeBGMOne");
    }


    public void ChangeBGMVolume(float value)
    {
        bgmVolume = value;
        if (BGMAudioObject)
            BGMAudioObject.AudioSource.volume = bgmVolume;
    }

    public void ChangeSFXVolume(float value)
    {
        sfxVolume = value;

        for (int i = 0; i < SoundManager.Instance.soundPool.transform.childCount; i++)
        {
            Transform t = SoundManager.Instance.soundPool.transform.GetChild(i);

            if (t.TryGetComponent<SoundObject>(out var sound))
            {
                if (sound == BGMAudioObject)
                    continue;
                sound.AudioSource.volume = sfxVolume;
            }
        }
    }

    public void ChangeBGM(AudioClip audioClip)
    {
        if (!BGMAudioObject)
        {
            BGMAudioObject = PlaySound(audioClip, true);
            BGMAudioObject.AudioSource.loop = true;
            BGMAudioObject.AudioSource.pitch = 1f;
            BGMAudioObject.name = "BGM Object";

        }
        else
            StartCoroutine("ChangeBGMClip", audioClip);
    }

    public SoundObject PlaySound(AudioClip audioClip, bool imortal = false)
    {
        if (SoundManager.Instance.soundPool.GetPoolObject().TryGetComponent<SoundObject>(out SoundObject soundObject))
        {
            soundObject.Init(audioClip, imortal);
            if (!imortal)
                prevAudio = soundObject;
            return soundObject;
        }
        return null;
    }

    IEnumerator ChangeBGMClip(AudioClip newClip)
    {
        current = percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(bgmVolume, 0f, percent);
            yield return null;
        }

        BGMAudioObject.AudioSource.clip = newClip;
        BGMAudioObject.AudioSource.Play();
        current = percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(0f, bgmVolume, percent);
            yield return null;
        }
    }

    IEnumerator ChangeBGMOne(AudioClip newClip)
    {
        var nowClip = BGMAudioObject.AudioSource;

        current = percent = 0f;

        while (percent < 0.5f)
        {
            current += Time.deltaTime;
            percent = current / 0.5f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(bgmVolume, 0f, percent);
            yield return null;
        }

        BGMAudioObject.AudioSource.clip = newClip;
        BGMAudioObject.AudioSource.Play();
        current = percent = 0f;

        while (percent < 0.5f)
        {
            current += Time.deltaTime;
            percent = current / 0.5f;
            BGMAudioObject.AudioSource.volume = Mathf.Lerp(0f, bgmVolume, percent);
            yield return null;
        }


    }


    public void GetSoundValue()
    {
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume", 1);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
    }

    public void SaveSoundValue()
    {
        PlayerPrefs.SetFloat("bgmVolume", bgmVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

}