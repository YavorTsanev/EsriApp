namespace Dtos
{
    using Newtonsoft.Json;

    public class UsaCounty
    {
        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }
}
