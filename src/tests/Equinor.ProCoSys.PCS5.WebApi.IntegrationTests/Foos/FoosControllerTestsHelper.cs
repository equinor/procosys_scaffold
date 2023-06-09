using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos;

public static class FoosControllerTestsHelper
{
    private const string _route = "Foos";

    public static async Task<FooDetailsDto> GetFooAsync(
        UserType userType,
        string plant,
        Guid guid,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).GetAsync($"{_route}/{guid}");

        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

        if (expectedStatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<FooDetailsDto>(content);
    }

    public static async Task<List<LinkDto>> GetFooLinksAsync(
        UserType userType,
        string plant,
        Guid guid,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).GetAsync($"{_route}/{guid}/Links");

        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

        if (expectedStatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<LinkDto>>(content);
    }

    public static async Task<List<FooDto>> GetAllFoosInProjectAsync(
        UserType userType,
        string plant,
        string projectName,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var parameters = new ParameterCollection
        {
            { "projectName", projectName },
            { "includeVoided", "true" }
        };
        var url = $"{_route}{parameters}";

        var response = await TestFactory.Instance.GetHttpClient(userType, plant).GetAsync(url);

        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

        if (expectedStatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<FooDto>>(content);
    }

    public static async Task<GuidAndRowVersion> CreateFooAsync(
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
        return JsonConvert.DeserializeObject<GuidAndRowVersion>(jsonString);
    }

    public static async Task<GuidAndRowVersion> CreateFooLinkAsync(
        UserType userType,
        string plant,
        Guid guid,
        string title,
        string url,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var bodyPayload = new
        {
            title,
            url
        };

        var serializePayload = JsonConvert.SerializeObject(bodyPayload);
        var content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).PostAsync($"{_route}/{guid}/Links", content);
        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GuidAndRowVersion>(jsonString);
    }

    public static async Task<GuidAndRowVersion> CreateFooCommentAsync(
        UserType userType,
        string plant,
        Guid guid,
        string text,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var bodyPayload = new
        {
            text
        };

        var serializePayload = JsonConvert.SerializeObject(bodyPayload);
        var content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).PostAsync($"{_route}/{guid}/Comments", content);
        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<GuidAndRowVersion>(jsonString);
    }

    public static async Task<string> UpdateFooAsync(
        UserType userType,
        string plant,
        Guid guid,
        string title,
        string text,
        string rowVersion,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var bodyPayload = new
        {
            title,
            text,
            rowVersion
        };

        var serializePayload = JsonConvert.SerializeObject(bodyPayload);
        var content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).PutAsync($"{_route}/{guid}", content);

        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> UpdateFooLinkAsync(
        UserType userType,
        string plant,
        Guid guid,
        Guid linkGuid,
        string title,
        string url,
        string rowVersion,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var bodyPayload = new
        {
            title,
            url,
            rowVersion
        };

        var serializePayload = JsonConvert.SerializeObject(bodyPayload);
        var content = new StringContent(serializePayload, Encoding.UTF8, "application/json");
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).PutAsync($"{_route}/{guid}/Links/{linkGuid}", content);

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
        Guid guid,
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
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).PutAsync($"{_route}/{guid}/Void", content);

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
        Guid guid,
        string rowVersion,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var bodyPayload = new
        {
            rowVersion
        };
        var serializePayload = JsonConvert.SerializeObject(bodyPayload);
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_route}/{guid}")
        {
            Content = new StringContent(serializePayload, Encoding.UTF8, "application/json")
        };

        var response = await TestFactory.Instance.GetHttpClient(userType, plant).SendAsync(request);
        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);
    }

    public static async Task DeleteFooLinkAsync(
        UserType userType,
        string plant,
        Guid guid,
        Guid linkGuid,
        string rowVersion,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var bodyPayload = new
        {
            rowVersion
        };
        var serializePayload = JsonConvert.SerializeObject(bodyPayload);
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_route}/{guid}/Links/{linkGuid}")
        {
            Content = new StringContent(serializePayload, Encoding.UTF8, "application/json")
        };

        var response = await TestFactory.Instance.GetHttpClient(userType, plant).SendAsync(request);
        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);
    }

    public static async Task<List<CommentDto>> GetFooCommentsAsync(
        UserType userType,
        string plant,
        Guid guid,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
        string expectedMessageOnBadRequest = null)
    {
        var response = await TestFactory.Instance.GetHttpClient(userType, plant).GetAsync($"{_route}/{guid}/Comments");

        await TestsHelper.AssertResponseAsync(response, expectedStatusCode, expectedMessageOnBadRequest);

        if (expectedStatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<CommentDto>>(content);
    }
}
