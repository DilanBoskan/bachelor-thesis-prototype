namespace Application.Contracts.Cloud;

public interface ICloudManager {
    Task PullAsync();
    Task PushAsync();
}