public interface IStageController
{
    float Progress { get; }
    float Length { get; }

    void Initialize();
    void Play();
    void Pause();
    void Restart();
    void SetTweenSpeed(float speed);
}