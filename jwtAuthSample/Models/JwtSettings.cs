namespace jwtAuthSample.Models
{
    public class JwtSettings
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 哪些客户端使用
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 加密(长度必须大于16字符，128字节)
        /// </summary>
        public string SecretKey { get; set; }
    }
}
