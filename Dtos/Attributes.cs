namespace Dtos
{
    using Newtonsoft.Json;

    public class Attributes
    {
        [JsonProperty("POPULATION")]
        public int Population { get; set; }

        [JsonProperty("STATE_NAME")]
        public string StateName { get; set; }
    }
}
