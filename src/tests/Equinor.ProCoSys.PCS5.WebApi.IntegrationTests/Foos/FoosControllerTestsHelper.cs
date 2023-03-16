using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos
{
    public static class FoosControllerTestsHelper
    {
        private const string _route = "Foos";
        
        public static async Task<FooDto> GetFooAsync(
            UserType userType,
            string plant,
            int id,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string expectedMessageOnBadRequest = null)
        {
            var response = await TestFactory.Instance.GetHttpClient(userType, plant).GetAsync($"{_route}/{id}");

            await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

            if (expectedStatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FooDto>(content);
        }

        public static async Task<IdAndRowVersion> CreateFooAsync(
            UserType userType,
            string plant,
            string title,
            string projectName,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string expectedMessageOnBadRequest = null)
        {
            var bodyPayload = new
            {
                title,
                projectName
            };

            var serializePayload = JsonConvert.SerializeObject(bodyPayload);
            var content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
            var response = await TestFactory.Instance.GetHttpClient(userType, plant).PostAsync(_route, content);
            await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IdAndRowVersion>(jsonString);
        }

        public static async Task<string> UpdateFooAsync(
            UserType userType,
            string plant,
            int id,
            string title,
            string rowVersion,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string expectedMessageOnBadRequest = null)
        {
            var bodyPayload = new
            {
                title,
                rowVersion
            };

            var serializePayload = JsonConvert.SerializeObject(bodyPayload);
            var content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
            var response = await TestFactory.Instance.GetHttpClient(userType, plant).PutAsync($"{_route}/{id}", content);

            await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> VoidFooAsync(
            UserType userType,
            string plant,
            int id,
            string rowVersion,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string expectedMessageOnBadRequest = null)
        {
            var bodyPayload = new
            {
                rowVersion
            };

            var serializePayload = JsonConvert.SerializeObject(bodyPayload);
            var content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
            var response = await TestFactory.Instance.GetHttpClient(userType, plant).PutAsync($"{_route}/{id}/Void", content);

            await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task DeleteFooAsync(
            UserType userType,
            string plant,
            int id,
            string rowVersion,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string expectedMessageOnBadRequest = null)
        {
            var bodyPayload = new
            {
                rowVersion
            };
            var serializePayload = JsonConvert.SerializeObject(bodyPayload);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_route}/{id}")
            {
                Content = new StringContent(serializePayload, Encoding.UTF8, "application/json")
            };

            var response = await TestFactory.Instance.GetHttpClient(userType, plant).SendAsync(request);
            await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);
        }
    }
}
