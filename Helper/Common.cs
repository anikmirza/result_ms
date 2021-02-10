using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace result_ms.Helper
{
    public class Common
    {
        public static string GetError(Exception ex)
        {
            string InnerExText = "An error occurred while updating the entries. See the inner exception for details.";
            string RequiredDataMissing = "The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value.\r\nThe statement has been terminated.";

            if (ex.Message == InnerExText && ex.InnerException != null)
            {
                return GetError(ex.InnerException);
            }
            if (ex.Message == RequiredDataMissing)
            {
                return "Required data is missing from Entity Object!";
            }
            return ex.Message;
        }
    }
}
