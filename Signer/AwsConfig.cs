using System;
using System.Collections.Generic;
using System.Text;

namespace AwsSigning
{
    public class AwsConfig
    {
        public string Region { get; set; }
        public string Service { get; set; }
        public string AccessId { get; set; }
        public string Secret { get; set; }
    }
}
