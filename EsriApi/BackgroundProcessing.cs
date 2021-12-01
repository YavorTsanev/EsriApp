namespace EsriApi
{
    using Data;
    using Data.Models;
    using Dtos;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class BackgroundProcessing
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _db;

        public BackgroundProcessing(IHttpClientFactory clientFactory, ApplicationDbContext db)
        {
            _clientFactory = clientFactory;
            _db = db;
        }

        private const string ConsumingApiEndPoint =
            "https://services.arcgis.com/P3ePLMYs2RVChkJx/ArcGIS/rest/services/USA_Counties/FeatureServer/0//query?where=1%3D1&outFields=population%2C+state_name&returnGeometry=false&f=pjson";

        public async Task RecurringUpdateDb()
        {
            var usaCountiesDto = GetResponseDto().Result;

            var dbModel = ConvertToDbModel(usaCountiesDto);

            await SaveInDb(dbModel);
        }

        private async Task<HashSet<UsaCounty>> GetResponseDto()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ConsumingApiEndPoint);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var responseAsString = response.Content.ReadAsStringAsync().Result;
            var contentAsJsonString = JObject.Parse(responseAsString)["features"].ToString();

            return JsonConvert.DeserializeObject<HashSet<UsaCounty>>(contentAsJsonString);
        }

        private HashSet<UsaState> ConvertToDbModel(IEnumerable<UsaCounty> responseModel)
        {
            return responseModel.GroupBy(x => x.Attributes.StateName).Select(x => new UsaState
            {
                StateName = x.Key,
                Population = x.Aggregate(0, (y, z) => y + z.Attributes.Population)
            }).ToHashSet();
        }

        private async Task SaveInDb(IEnumerable<UsaState> model)
        {
            if (_db.UsaStates.Any())
            {
                await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [UsaStates]");
            }

            await _db.AddRangeAsync(model);

            await _db.SaveChangesAsync();
        }
    }
}
