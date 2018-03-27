using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace SearchFight.Search
{
    public class Provider
    {
        string _requestURL;
        string _code;
        public Dictionary<string, long> resultsByProvider = new Dictionary<string, long>();

        public string code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string requestURL
        {
            get { return _requestURL;  }
            set { _requestURL = value; }
        }

        public Provider(string link)
        {
            requestURL = link;
        }    
        
        public virtual long SearchCounter(string parameter)
        {
            long resultado = 0;
            return resultado;
        }
    }
}
