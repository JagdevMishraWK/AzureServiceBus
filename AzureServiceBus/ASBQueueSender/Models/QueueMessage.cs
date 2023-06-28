namespace ASBQueueSender.Models
{
    public class QueueMessage
    {
        public Guid QueueMessageId { get; set; }
        public string MessageDetails { get; set; } = string.Empty;
        public double Amount { get; set; }  = double.MinValue;
        public DateTime MessageDate { get; set; }
    }
}
