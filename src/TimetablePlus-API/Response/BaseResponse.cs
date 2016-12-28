using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Response
{
    public class BaseResponse<T>
    {
        public static readonly int RESULT_OK = 0;
        public static readonly int RESULT_FAIL = 1;
        public static readonly string EXEC_SUCCESS = "success";

        public int code { get; set; }
        public string message { get; set; }
        public T content;

        public BaseResponse()
        {
            setSuccess();
        }

        public BaseResponse(T data)
        {
            this.content = data;
            setSuccess();
        }

        public void setSuccess()
        {
            code = RESULT_OK;
            message = EXEC_SUCCESS;
        }

        public void setFailed(string message)
        {
            code = RESULT_FAIL;
            this.message = message;
        }

        public void setContent(T content)
        {
            this.content = content;
        }
    }
}
