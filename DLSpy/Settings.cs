using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLSpy
{
    public class Settings
    {
        public static Settings _instance = null;

        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        /// <summary>
        /// {0} is search keyword, and {1} is page number.
        /// </summary>
        public const string URL_SEARCH = "https://www.dlsite.com/maniax/fsr/=/language/jp/sex_category%5B0%5D/male/keyword/{0}/order%5B0%5D/trend/per_page/30/page/{1}/?locale=ko_KR";

        /// <summary>
        /// {0} is product id(RJ Code).
        /// </summary>
        public const string URL_WORK_RJ = "https://www.dlsite.com/maniax/work/=/product_id/{0}.html/?locale=ko_KR";

        /// <summary>
        /// {0} is product id(BJ Code).
        /// </summary>
        public const string URL_WORK_BJ = "https://www.dlsite.com/books/work/=/product_id/{0}.html/?locale=ko_KR";

        /// <summary>
        /// {0} is product id(VJ Code).
        /// </summary>
        public const string URL_WORK_VJ = "https://www.dlsite.com/pro/work/=/product_id/{0}.html/?locale=ko_KR";

        /// <summary>
        /// {0} is product id(RJ/BJ/VJ code).
        /// </summary>
        public const string URL_WORK_ALL = "https://www.dlsite.com/home/work/=/product_id/{0}.html/?locale=ko_KR";

        /// <summary>
        /// {0} is product id(RJ/BJ/VJ code).
        /// </summary>
        public const string URL_WORK_PURCHASE_ALL = "https://www.dlsite.com/home/cart/=/product_id/{0}";

        /// <summary>
        /// {0} is product id(RJ code).
        /// </summary>
        public const string URL_WORK_JSON_RJ = "https://www.dlsite.com/maniax/product/info/ajax?product_id={0}&cdn_cache_min=1";

        /// <summary>
        /// {0} is product id(BJ code).
        /// </summary>
        public const string URL_WORK_JSON_BJ = "https://www.dlsite.com/books/product/info/ajax?product_id={0}&cdn_cache_min=1";

        /// <summary>
        /// {0} is product id(VJ code).
        /// </summary>
        public const string URL_WORK_JSON_VJ = "https://www.dlsite.com/pro/product/info/ajax?product_id={0}&cdn_cache_min=1";

        /// <summary>
        /// The default thumbnail image.
        /// </summary>
        public const string URL_DEFAULT_THUMBNAIL = "https://www.dlsite.com/images/web/common/logo/pc/logo-dlsite-r18.png";

        /// <summary>
        /// A regex for checking RJ code.
        /// </summary>
        public const string REGEX_MATCH_RJ = @"RJ[0-9]{1,6}";

        /// <summary>
        /// A regex for checking BJ code.
        /// </summary>
        public const string REGEX_MATCH_BJ = @"BJ[0-9]{1,6}";

        /// <summary>
        /// A regex for checking VJ code.
        /// </summary>
        public const string REGEX_MATCH_VJ = @"VJ[0-9]{1,6}";
    }
}
