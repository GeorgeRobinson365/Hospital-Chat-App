using Microsoft.AspNetCore.Http;

namespace Backend.Tokens;

public class TokenExtractor
{
    public TokenExtractor(){}

    public static string TryGet(HttpRequest request)
    {
        string authorizationHeader = request.Headers["JWT"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader))
            return null;

        if (authorizationHeader.StartsWith("Bearer"))
            authorizationHeader = authorizationHeader.Substring(7);

        return authorizationHeader;
    } 
}