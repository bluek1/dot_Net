using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTranslate.Api.Google.Text
{
    public class TransJsonData
    {
        public TransJsonDataList data;
    }
    public class TransJsonDataList
    {
        public List<TransJsonValue> translations=new List<TransJsonValue>();
    }
    public class TransJsonValue
    {
        public string translatedText;
    }

}
