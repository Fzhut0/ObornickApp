using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Append("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }

        public static bool CheckLinkValidity(string link)
        {
            Uri uriResult;
            var result = Uri.TryCreate(link, UriKind.Absolute, out uriResult) && 
            (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);

            return result;
        }
    }
}