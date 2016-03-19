/**
 * Created by VolkovA on 03.03.14.
 */

namespace selenium.core.Framework.Service {
    public class ServiceMatchResult {
        private readonly BaseUrlInfo _baseUrlInfo;
        private readonly Service _service;

        public ServiceMatchResult(Service service, BaseUrlInfo baseUrlInfo) {
            _service = service;
            _baseUrlInfo = baseUrlInfo;
        }

        public Service getService() {
            return _service;
        }

        public BaseUrlInfo getBaseUrlInfo() {
            return _baseUrlInfo;
        }
    }
}