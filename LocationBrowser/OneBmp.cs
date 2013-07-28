using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace LocationBrowser{
    internal class OneBmp{
        public String Url { get; set; }
        public String Info { get; set; }

        public Bitmap Bitmap { get; set; }

        public OneBmp(String url){
            Url = url;
            Info = "";
            
            //キャッシュ検索
            var info = IeCache.GetUrlCacheEntryInfo(Url);

            try {
                Bitmap = new Bitmap(info.lpszLocalFileName);

                Info = Exif.All(Bitmap);
                //Info = Exif.All(Bitmap) + "" + Exif.IdList(Bitmap);


            } catch (Exception){
                Bitmap = null;
            }
        }
    }
}

