using System.Collections.Generic;
using System.Linq;

namespace BluekLibrary.Text
{
    /// <summary>
    /// 분석용 문자열 클레스
    /// </summary>
    public class AnalysisString 
    {
        /// <summary>
        /// 원본문자열
        /// </summary>
        private string mtext;

        /// <summary>
        /// 단어
        /// </summary>
        public List<string> Words { private set; get; }
        
        /// <summary>
        /// 공백상수
        /// </summary>
        private const char SPACE = ' ';
        /// <summary>
        /// 분기 기호
        /// </summary>
        private const string SplitChars = "<>/='\"+-,.()\\|{}[]";

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="str"></param>
        public AnalysisString(string str)
        {
            Words = new List<string>();
            string tmp = string.Empty;
            mtext =str;

            foreach (char c in mtext.ToCharArray())
            {

                if (SplitChars.Contains(c)){
                    if (tmp != string.Empty){
                        Words.Add(tmp);
                        
                    }
                    Words.Add(c.ToString ());
                    tmp = string.Empty ;
                }
                else
                {
                    if (c != SPACE)
                    {
                        tmp = tmp + c.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 문자열 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mtext;
        }
    }
}
