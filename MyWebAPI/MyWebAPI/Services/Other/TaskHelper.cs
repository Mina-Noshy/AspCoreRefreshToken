using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPI.Services.Other
{
    public static class TaskHelper
    {
        public static string GetResponceStatus(string message)
        {
            if (message.ToUpper().Contains("SUCCESS"))
                return "Success";
            else if (message.ToUpper().Contains("ERROR"))
                return "Error";
            else
                return "Warning";
        }
    }
}
