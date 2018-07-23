using System.Linq.Expressions;
using Newtonsoft.Json;
namespace HomeQI.ADream.Infrastructure.Core
{

    public class BootTabPageResult
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("rows")]
        public object Rows { get; set; }
    }
    public class BootTabPageListPar : PageListPar
    {


        private int _currentPage;

        /// <summary>
        /// 当前页
        /// </summary>
        [JsonProperty("currentPage")]
        public override int CurrentPage
        {
            get
            {
                _currentPage = Offset < 0 ? 0 : Offset;
                int showCount = Limit <= 0 ? 10 : Limit;
                _currentPage = _currentPage / showCount; // 获取页数
                _currentPage += 1;
                return _currentPage;
            }
            set
            {
                _currentPage = value;

            }
        }
    }

    public class PageListPar
    {
        [JsonProperty("id")]
        public virtual string Id { get; set; }
        /// <summary>
        /// 搜索表达式
        /// </summary>
        [JsonProperty("search")]

        public virtual string Search { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [JsonProperty("sort")]
        public virtual string Sort { get; set; } = "Id";
        /// <summary>
        /// 偏移量
        /// </summary>
        [JsonProperty("offset")]
        public virtual int Offset { get; set; }
        /// <summary>
        /// 每页页数
        /// </summary>
        [JsonProperty("limit")]
        public virtual int Limit { get; set; } = 10;

        /// <summary>
        /// 默认是desc
        /// </summary>
        [JsonProperty("order")]
        public virtual string Order { get; set; } = "desc";
        [JsonProperty("filters")]
        public QueryableFilter[] Filters { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        [JsonProperty("currentPage")]
        public virtual int CurrentPage { get; set; } = 1;
    }
}
