namespace N8T.Infrastructure.Bus
{
    public class KafkaOptions
    {
        public string BootstrapServers { get; set; }

        public string GroupId { get; set; }
    }
}
