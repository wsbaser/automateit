/**
 * Created by VolkovA on 03.03.14.
 */

namespace Selenium.Core.Framework.Service
{
    public class ServiceMatchResult
    {
        private readonly BaseUrlInfo _baseUrlInfo;

        private readonly Service _service;

        public ServiceMatchResult(Service service, BaseUrlInfo baseUrlInfo)
        {
            this._service = service;
            this._baseUrlInfo = baseUrlInfo;
        }

        public Service getService()
        {
            return this._service;
        }

        public BaseUrlInfo getBaseUrlInfo()
        {
            return this._baseUrlInfo;
        }
    }
}