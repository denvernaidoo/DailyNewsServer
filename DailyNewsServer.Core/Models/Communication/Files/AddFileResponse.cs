using System;
using System.Collections.Generic;
using System.Text;

namespace DailyNewsServer.Core.Models.Communication.Files
{
    public class AddFileResponse
    {
        public List<string> PreSignedUrl { get; set; }
    }
}
