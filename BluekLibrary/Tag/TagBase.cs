using System.Collections.Generic;

namespace BluekLibrary.Tag
{
    abstract class TagBase
    {
        /// <summary>
        /// 테그 이름
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 원본 문자열
        /// </summary>
        protected string mTagString=null;
        /// <summary>
        /// 서브 테크 일람
        /// </summary>
        protected List<TagBase> mSubTagList = null;
        /// <summary>
        /// 어트리뷰티 일람
        /// </summary>
        protected Dictionary<string, string> mAttrList = null;
        /// <summary>
        /// 설정치
        /// </summary>
        protected string Value { get; set; }
        /// <summary>
        /// 종료테그 유무
        /// </summary>
        protected bool EndTagFlag = false;
        
        /// <summary>
        /// 파싱
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        abstract  public bool Parse(string str);

        /// <summary>
        /// 문자열 취득
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mTagString;
        }

    }
}
