using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AFGetStarted.Service
{
    public interface IQueueService
    {
        Task SendMessageAsync(string message);
    }

    public class QueueService : IQueueService
    {
        readonly CloudQueue _cloudQueue;
        public QueueService(string connectionString, string queue)
        {
            _cloudQueue = CloudStorageAccount.Parse(connectionString).CreateCloudQueueClient().GetQueueReference(queue);
        }

        public async Task SendMessageAsync(string message)
        {
            await _cloudQueue.CreateIfNotExistsAsync();
            await _cloudQueue.AddMessageAsync(new CloudQueueMessage(message));
        }
    }
}