using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private AudioSource audioSource_BGM;
    private AudioSource audioSource_Effect;
    private AudioSource audioSource_Effect2;

    public AudioClip[] Audio;
    private AudioClip dowloClip;
    IEnumerator audioFadeOutCoroutine;


    private void Awake()
    {
        Instance = this;
        audioSource_BGM = gameObject.GetComponents<AudioSource>()[0];
        audioSource_Effect = gameObject.GetComponents<AudioSource>()[1];
        audioSource_Effect2 = gameObject.GetComponents<AudioSource>()[2];
    }

    /*   public void LoadAudioClip()
       {
           //StartCoroutine(GetAudioClip("Jump"));

           //     DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "BGM_Play", typeof(AudioClip));

           Audio[0] = (AudioClip)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Audio, "BGM_Story", typeof(AudioClip));
       }

       IEnumerator GetAudioClip(string fileName)
       {
           using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("http://localhost:8080/games/games011/Audio/" + fileName+".mp3", AudioType.MPEG))
           {
               yield return www.Send();

               if (www.isNetworkError)
               {
                   Debug.Log(www.error);
               }
               else
               {
                   dowloClip = DownloadHandlerAudioClip.GetContent(www);
               }
           }

           Audio[0] = dowloClip;
        //  PlayAudio(0, 0, false);


       }
       */
   /* public void PlayAudio(int AudioSource, int ClipID, bool loop)//AudioSource : 1 =>BGM  AudioSource : 1 =>Effect
    {

        switch (AudioSource)
        {
            case 0:
                if (audioFadeOutCoroutine != null)
                {
                    StopCoroutine(audioFadeOutCoroutine);

                }
                audioSource_BGM.volume = 1;
                audioSource_BGM.Stop();
                audioSource_BGM.loop = loop;
                audioSource_BGM.clip = Audio[ClipID];
                audioSource_BGM.Play();
                break;

            case 1:
                audioSource_Effect.volume = 1;
                audioSource_Effect.Stop();
                audioSource_Effect.loop = loop;
                audioSource_Effect.clip = Audio[ClipID];
                audioSource_Effect.Play();
                break;
            case 2:
                audioSource_Effect2.volume = 1;
                audioSource_Effect2.Stop();
                audioSource_Effect2.loop = loop;
                audioSource_Effect2.clip = Audio[ClipID];
                audioSource_Effect2.Play();
                break;

        }

    }
    
 /*   public void AudioFadeOut_BGM(float fadeOutSpeed)
    {
        audioFadeOutCoroutine = audioFade(fadeOutSpeed);
        StartCoroutine(audioFadeOutCoroutine);
    }*/

  /*  IEnumerator audioFade(float fadeOutSpeed)
    {
        while (audioSource_BGM.volume > 0)
        {
            yield return null;

            audioSource_BGM.volume -= fadeOutSpeed * Time.deltaTime;
           // Debug.Log(audioSource_BGM.volume);
        }
        audioSource_BGM.Stop();
        audioSource_BGM.volume = 1;
    }*/
}
