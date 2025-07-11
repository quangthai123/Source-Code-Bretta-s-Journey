using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SwordSkillPreview : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage previewImage;
    public RenderTexture renderTexture;
    private string playingVideoName;

    public void PlaySkillPreview(string skillName)
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "SkillVideos", skillName + ".mp4");

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        previewImage.texture = renderTexture;

        videoPlayer.isLooping = true;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
        playingVideoName = skillName;
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }
    public bool IsPlaying(string videoName) => videoName == playingVideoName ? true : false;
    public void StopPreview()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Stop();
    }
}
