using Newtonsoft.Json;
using SmiTranslate.Api.Google.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Google.Text
{
    public enum language:int
    {
        English = 0,
        Japan,
        Korean,
    }
    public class GoogleTransApi
    {
        private string[]  constLang = { "en", "ja", "ko" };

        const string URL = @"https://translation.googleapis.com/language/translate/v2?key={0}&source={1}&target={2}&";
        const string Data = "q=";
        public string Key { get; set; }
        public language SourceLan { get; set; }
        public language TagetLan { get; set; }
        /// <summary>
        /// 입력번역 문장 수
        /// </summary>
        public int TransInBufferCount { get; set; }
        /// <summary>
        /// 입력 문자열
        /// </summary>
        public List<object> TransInTexts { set; get; }
        /// <summary>
        /// 출력 문자열
        /// </summary>
        public List<string> TransOutTexts { private set; get; }

        public GoogleTransApi()
        {
            this.TransInTexts = new List<object>();
            this.TransOutTexts = new List<string>();
            this.Key = "AIzaSyCO6o9wEYvFh-Y-zIDEpFCc5-llxj1O9ac";
        }

        public void Clear()
        {
            this.TransInTexts.Clear();
            this.TransOutTexts.Clear();
        }

        public async Task<bool> RunTransAsync()
        {
            int icount = 0;
            string DataParam = "";
            string AccUrl = string.Format(URL, this.Key, constLang[(int)this.SourceLan], constLang[(int)this.TagetLan]);
            HttpClient client = new HttpClient();

            while (icount< TransInTexts.Count )
            { 
                for(int i=0;i<this.TransInBufferCount;i++)
                {
                    if (icount < TransInTexts.Count)
                    {
                        break;
                    }
                    DataParam = DataParam + string.Format("q={0}&", TransInTexts[icount].ToString ());
                    icount++;
                }

                HttpResponseMessage response = await client.GetAsync(AccUrl+ DataParam);
                response.EnsureSuccessStatusCode();
                string jsonstrong = await response.Content.ReadAsStringAsync();
                TransJsonData GetData = JsonConvert.DeserializeObject<TransJsonData>(jsonstrong);

                foreach(TransJsonValue str in GetData.data.translations)
                {
                    TransOutTexts.Add(str.translatedText);
                }
               
            }

            return true;
        }







    }
}
