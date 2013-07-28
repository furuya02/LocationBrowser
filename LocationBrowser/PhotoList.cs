using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LocationBrowser {
    class PhotoList{
        private WebBrowser _webBrowser;
        List<OneBmp> _ar = new List<OneBmp>();

        public PhotoList(WebBrowser webBrowser){
            _webBrowser = webBrowser;

            //スクリプトエラーのダイアログを非表示にする
            _webBrowser.ScriptErrorsSuppressed = false;

            _webBrowser.AllowWebBrowserDrop = false;
            _webBrowser.IsWebBrowserContextMenuEnabled = false;
            _webBrowser.WebBrowserShortcutsEnabled = false;
            //_webBrowser.ObjectForScripting = this;

            string str = "<html><body>Hello world !<lu class=\"PhotoList\"><li>1</li></lu></body></html>";
            _webBrowser.DocumentText = str;

        }
        public void Refresh(List<string> list){
            if (list.Count == 0){
                return;//処理なし 
            }
            
            //最初から比較して同じかどうか？
            if (IsSame(list)){ //最初が同じなので追加のみ
                for (int i = _ar.Count; i < list.Count; i++){
                    Add(list[i]);
                }
            } else{//最初から違うので、再構築
                Clear();
                foreach (var l in list){
                    Add(l);
                }
            }
        }

        public void Clear(){
            _ar.Clear();
//            _imageList.Images.Clear();
  //          _listView.Items.Clear();
        }

        void Add(String url){
            var oneBmp = new OneBmp(url);
            if (oneBmp.Bitmap != null){
                _ar.Add(oneBmp);
            }

            _webBrowser.DocumentText = CreateHtml();

            

        }

        string CreateHtml(){
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<body style=\"background:#dddddd;color:#777777\">");
            //sb.Append("<lu>");
            foreach (var o in _ar){
                if (o.Info != ""){
                    sb.Append(String.Format("<p><div style=\"background:#aa0000;color:white\" ><img src={0} width={1} height={2} alt={0}>{3}</div></p>", o.Url, o.Bitmap.Width / 2, o.Bitmap.Height / 2, o.Info));
                } else{
                    sb.Append(String.Format("<img src={0} width={1} height={2} alt={0}>　", o.Url, o.Bitmap.Width / 2, o.Bitmap.Height / 2));
                }
            }
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();

        }

        //最初から比較して同じかどうか？
        bool IsSame(List<String> list) {
            if (_ar.Count <= list.Count){
                for (int i = 0; i < _ar.Count; i++) {
                    if (_ar[i].Url != list[i]) {
                        return true;
                    }
                }
            }
            return false;
        }


    }
}
