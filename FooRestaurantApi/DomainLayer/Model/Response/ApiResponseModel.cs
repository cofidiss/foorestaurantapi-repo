using System;
using System.Net;

namespace FooRestaurantApi.DomainLayer.Model.Response
{
    public class ApiResponseModel<Terror, Tsuccess> where Terror : class
    {

        public ApiResponseModel()
        {
            StatusCode = HttpStatusCode.OK;

        }
        public bool Success { get { return ErrorData == null; } }
        private Terror _errorData;
        private Tsuccess _successData;
        public HttpStatusCode StatusCode { get; set; }
        public Terror ErrorData
        {
            get { return _errorData; }
            set
            {

                _errorData = value;
            }
        }
        public Tsuccess SuccessData
        {
            get { return _successData; }
            set
            {
                if (_errorData != null)
                {
                    throw new Exception("Succes data can not be not set when Error Data has been set.");

                }
                _successData = value;
            }
        }

    }
}
