using System.Net;

namespace Cineder_UI.Web.Models.Common;

public record ApiResponse<T>(T Data, HttpStatusCode StatusCode)
{
    public ApiResponse(HttpStatusCode stat) : this(default!, stat) { }
}
