namespace AspNetCore.StartupTemplate.CustomRbMQ;

public interface IDispatcher
{
    void EnqueueToPublish(MediumMessage message);

    void EnqueueToExecute(MediumMessage message, ConsumerExecutorDescriptor descriptor);
}