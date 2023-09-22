using Backend.Model;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;

namespace Backend.Tokens
{
    public class TokenIssuer
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly JwtBuilder _jwtEncoder;
        private readonly string _secret;

        public TokenIssuer(string secret)
        {

            _algorithm = new HMACSHA256Algorithm(); // change this alg later maybe (keeping it just now as it is easier for something quick)
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _secret = secret;
            _jwtEncoder = JwtBuilder.Create().WithAlgorithm(_algorithm)
                .WithSecret(_secret)
                .MustVerifySignature();
        }

        public string IssueTokenForUser(Identity identity)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
                {
                    {"uid", identity.Id },
                    {"roles", identity.Role }
                };

            string token = _jwtEncoder.Encode(identity);

            return token;
        }
        public Identity TokenDecoder(string token)
        {
            return JwtBuilder.Create()
                .WithAlgorithm(_algorithm)
                .WithSecret(_secret)
                .MustVerifySignature()
                .Decode<Identity>(token);
        }
    }
}