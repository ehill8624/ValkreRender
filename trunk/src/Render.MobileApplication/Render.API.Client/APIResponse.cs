using System;
using System.Net;

namespace Render.API.Client
{
    public class APIResponse<T> //where T : BaseModel
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public HttpStatusCode Code { get; set; }

        public bool SuccessfulWithResult()
        {
            return this.Success && this.Result != null;
        }

        public bool SuccessfulWithoutResult()
        {
            return this.Success && this.Result == null;
        }

        public bool HasResult()
        {
            return this.Result != null;
        }
    }
}