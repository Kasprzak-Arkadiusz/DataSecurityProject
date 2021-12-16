using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace UI.Utils
{
    public class SingleCertificateValidator
    {
        private readonly X509Certificate2 _trustedCertificate;

        public SingleCertificateValidator(X509Certificate2 trustedCertificate)
        {
            _trustedCertificate = trustedCertificate;
        }

        public bool Validate(HttpRequestMessage httpRequestMessage, X509Certificate2 x509Certificate2, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            var thumbprint = x509Certificate2.GetCertHashString();
            return thumbprint == _trustedCertificate.Thumbprint;
        }
    }
}
