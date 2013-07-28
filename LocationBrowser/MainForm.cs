using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;

namespace LocationBrowser {
    public partial class MainForm : Form{
        private int lastSize = 0;
        private PhotoList _photoList;

        Timer t;
        public MainForm() {
            InitializeComponent();

            _photoList = new PhotoList(webBrowserPhoto);

            t = new Timer();
            t.Enabled = true;
            t.Tick += t_Tick;
            t.Interval = 1000;


            Navigate();
        }


        private void buttonRefresh_Click(object sender, EventArgs e){
            if (textBoxUrl.Text.IndexOf("http") != 0){
                textBoxUrl.Text = "http://" + textBoxUrl.Text;
            }
            Navigate();
        }

        private void buttonHome_Click(object sender, EventArgs e){
            textBoxUrl.Text = "www.twitter.com";
            Navigate();
        }

        void Navigate(){
            webBrowser.Navigate(textBoxUrl.Text);
        }

        private String keep = "";
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            if (e.Url != null){
                
                if (e.Url.ToString().IndexOf("about:blank") != -1){
                    return;
                }

                if (webBrowser.Document == null) {
                    return;
                }
                if (webBrowser.Document.Body == null) {
                    return;
                }
                if (webBrowser.Document.Body.InnerHtml == null) {
                    return;
                }

                var url = webBrowser.Document.Url.ToString();
                textBoxUrl.Text = url;
                var index = url.IndexOf('?');
                if(index!=-1){
                    url = url.Substring(0, index);
                }
                if (keep != url){
                    keep = url;
                    _photoList.Clear();
                }



            }
        }

        void t_Tick(object sender, EventArgs e){
            if (webBrowser.Document == null) {
                return;
            }
            if (webBrowser.Document.Body == null) {
                return;
            }
            if (webBrowser.Document.Body.InnerHtml == null) {
                return;
            }
            var len = webBrowser.Document.Body.InnerHtml.Length;

            if (lastSize != len) {
                lastSize = len;
                var list = GetImg(textBoxUrl.Text);

                _photoList.Refresh(list);
            }
        }

        private List<String> GetImg(string url){
            var ar = new List<String>();
            var hDoc = (IHTMLDocument2)webBrowser.Document.DomDocument;
            foreach (IHTMLImgElement img in hDoc.images){
                if (img.src != "") {
                    if (ar.IndexOf(img.src) == -1){
                        ar.Add(img.src);
                    }
                }
            }
            return ar;
        }

        //enterキー
        private void textBoxUrl_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter){
                Navigate();
            }
        }
        //Back
        private void buttonBack_Click(object sender, EventArgs e){
            webBrowser.GoBack();
        }
        //Forward
        private void buttonForward_Click(object sender, EventArgs e){
            webBrowser.GoForward();
        }

        //Stop
        private void buttonStop_Click(object sender, EventArgs e) {
            webBrowser.Stop();
        }

        class ExifInfo{
            public int Id { get; private set; }
            public String Name { get; private set; }
            public ExifInfo(int id, string name) {
                Id = id;
                Name = name;
            }
        }
        private void buttonDebug_Click(object sender, EventArgs e){
            //textBoxUrl.Text = "https://si0.twimg.com/profile_images/1821614329/twitter3_normal.jpg";
            //Navigate();

            List<ExifInfo> list = new List<ExifInfo>(){new ExifInfo(0x001, "exif:InteroperabilityIndex"), new ExifInfo(0x002, "exif:InteroperabilityVersion"), new ExifInfo(0x100, "exif:ImageWidth"), new ExifInfo(0x101, "exif:ImageLength"), new ExifInfo(0x102, "exif:BitsPerSample"), new ExifInfo(0x103, "exif:Compression"), new ExifInfo(0x106, "exif:PhotometricInterpretation"), new ExifInfo(0x10a, "exif:FillOrder"), new ExifInfo(0x10d, "exif:DocumentName"), new ExifInfo(0x10e, "exif:ImageDescription"), new ExifInfo(0x10f, "exif:Make"), new ExifInfo(0x110, "exif:Model"), new ExifInfo(0x111, "exif:StripOffsets"), new ExifInfo(0x112, "exif:Orientation"), new ExifInfo(0x115, "exif:SamplesPerPixel"), new ExifInfo(0x116, "exif:RowsPerStrip"), new ExifInfo(0x117, "exif:StripByteCounts"), new ExifInfo(0x11a, "exif:XResolution"), new ExifInfo(0x11b, "exif:YResolution"), new ExifInfo(0x11c, "exif:PlanarConfiguration"), new ExifInfo(0x11d, "exif:PageName"), new ExifInfo(0x11e, "exif:XPosition"), new ExifInfo(0x11f, "exif:YPosition"), new ExifInfo(0x118, "exif:MinSampleValue"), new ExifInfo(0x119, "exif:MaxSampleValue"), new ExifInfo(0x120, "exif:FreeOffsets"), new ExifInfo(0x121, "exif:FreeByteCounts"), new ExifInfo(0x122, "exif:GrayResponseUnit"), new ExifInfo(0x123, "exif:GrayResponseCurve"), new ExifInfo(0x124, "exif:T4Options"), new ExifInfo(0x125, "exif:T6Options"), new ExifInfo(0x128, "exif:ResolutionUnit"), new ExifInfo(0x12d, "exif:TransferFunction"), new ExifInfo(0x131, "exif:Software"), new ExifInfo(0x132, "exif:DateTime"), new ExifInfo(0x13b, "exif:Artist"), new ExifInfo(0x13e, "exif:WhitePoint"), new ExifInfo(0x13f, "exif:PrimaryChromaticities"), new ExifInfo(0x140, "exif:ColorMap"), new ExifInfo(0x141, "exif:HalfToneHints"), new ExifInfo(0x142, "exif:TileWidth"), new ExifInfo(0x143, "exif:TileLength"), new ExifInfo(0x144, "exif:TileOffsets"), new ExifInfo(0x145, "exif:TileByteCounts"), new ExifInfo(0x14a, "exif:SubIFD"), new ExifInfo(0x14c, "exif:InkSet"), new ExifInfo(0x14d, "exif:InkNames"), new ExifInfo(0x14e, "exif:NumberOfInks"), new ExifInfo(0x150, "exif:DotRange"), new ExifInfo(0x151, "exif:TargetPrinter"), new ExifInfo(0x152, "exif:ExtraSample"), new ExifInfo(0x153, "exif:SampleFormat"), new ExifInfo(0x154, "exif:SMinSampleValue"), new ExifInfo(0x155, "exif:SMaxSampleValue"), new ExifInfo(0x156, "exif:TransferRange"), new ExifInfo(0x157, "exif:ClipPath"), new ExifInfo(0x158, "exif:XClipPathUnits"), new ExifInfo(0x159, "exif:YClipPathUnits"), new ExifInfo(0x15a, "exif:Indexed"), new ExifInfo(0x15b, "exif:JPEGTables"), new ExifInfo(0x15f, "exif:OPIProxy"), new ExifInfo(0x200, "exif:JPEGProc"), new ExifInfo(0x201, "exif:JPEGInterchangeFormat"), new ExifInfo(0x202, "exif:JPEGInterchangeFormatLength"), new ExifInfo(0x203, "exif:JPEGRestartInterval"), new ExifInfo(0x205, "exif:JPEGLosslessPredictors"), new ExifInfo(0x206, "exif:JPEGPointTransforms"), new ExifInfo(0x207, "exif:JPEGQTables"), new ExifInfo(0x208, "exif:JPEGDCTables"), new ExifInfo(0x209, "exif:JPEGACTables"), new ExifInfo(0x211, "exif:YCbCrCoefficients"), new ExifInfo(0x212, "exif:YCbCrSubSampling"), new ExifInfo(0x213, "exif:YCbCrPositioning"), new ExifInfo(0x214, "exif:ReferenceBlackWhite"), new ExifInfo(0x2bc, "exif:ExtensibleMetadataPlatform"), new ExifInfo(0x301, "exif:Gamma"), new ExifInfo(0x302, "exif:ICCProfileDescriptor"), new ExifInfo(0x303, "exif:SRGBRenderingIntent"), new ExifInfo(0x320, "exif:ImageTitle"), new ExifInfo(0x5001, "exif:ResolutionXUnit"), new ExifInfo(0x5002, "exif:ResolutionYUnit"), new ExifInfo(0x5003, "exif:ResolutionXLengthUnit"), new ExifInfo(0x5004, "exif:ResolutionYLengthUnit"), new ExifInfo(0x5005, "exif:PrintFlags"), new ExifInfo(0x5006, "exif:PrintFlagsVersion"), new ExifInfo(0x5007, "exif:PrintFlagsCrop"), new ExifInfo(0x5008, "exif:PrintFlagsBleedWidth"), new ExifInfo(0x5009, "exif:PrintFlagsBleedWidthScale"), new ExifInfo(0x500A, "exif:HalftoneLPI"), new ExifInfo(0x500B, "exif:HalftoneLPIUnit"), new ExifInfo(0x500C, "exif:HalftoneDegree"), new ExifInfo(0x500D, "exif:HalftoneShape"), new ExifInfo(0x500E, "exif:HalftoneMisc"), new ExifInfo(0x500F, "exif:HalftoneScreen"), new ExifInfo(0x5010, "exif:JPEGQuality"), new ExifInfo(0x5011, "exif:GridSize"), new ExifInfo(0x5012, "exif:ThumbnailFormat"), new ExifInfo(0x5013, "exif:ThumbnailWidth"), new ExifInfo(0x5014, "exif:ThumbnailHeight"), new ExifInfo(0x5015, "exif:ThumbnailColorDepth"), new ExifInfo(0x5016, "exif:ThumbnailPlanes"), new ExifInfo(0x5017, "exif:ThumbnailRawBytes"), new ExifInfo(0x5018, "exif:ThumbnailSize"), new ExifInfo(0x5019, "exif:ThumbnailCompressedSize"), new ExifInfo(0x501a, "exif:ColorTransferFunction"), new ExifInfo(0x501b, "exif:ThumbnailData"), new ExifInfo(0x5020, "exif:ThumbnailImageWidth"), new ExifInfo(0x5021, "exif:ThumbnailImageHeight"), new ExifInfo(0x5022, "exif:ThumbnailBitsPerSample"), new ExifInfo(0x5023, "exif:ThumbnailCompression"), new ExifInfo(0x5024, "exif:ThumbnailPhotometricInterp"), new ExifInfo(0x5025, "exif:ThumbnailImageDescription"), new ExifInfo(0x5026, "exif:ThumbnailEquipMake"), new ExifInfo(0x5027, "exif:ThumbnailEquipModel"), new ExifInfo(0x5028, "exif:ThumbnailStripOffsets"), new ExifInfo(0x5029, "exif:ThumbnailOrientation"), new ExifInfo(0x502a, "exif:ThumbnailSamplesPerPixel"), new ExifInfo(0x502b, "exif:ThumbnailRowsPerStrip"), new ExifInfo(0x502c, "exif:ThumbnailStripBytesCount"), new ExifInfo(0x502d, "exif:ThumbnailResolutionX"), new ExifInfo(0x502e, "exif:ThumbnailResolutionY"), new ExifInfo(0x502f, "exif:ThumbnailPlanarConfig"), new ExifInfo(0x5030, "exif:ThumbnailResolutionUnit"), new ExifInfo(0x5031, "exif:ThumbnailTransferFunction"), new ExifInfo(0x5032, "exif:ThumbnailSoftwareUsed"), new ExifInfo(0x5033, "exif:ThumbnailDateTime"), new ExifInfo(0x5034, "exif:ThumbnailArtist"), new ExifInfo(0x5035, "exif:ThumbnailWhitePoint"), new ExifInfo(0x5036, "exif:ThumbnailPrimaryChromaticities"), new ExifInfo(0x5037, "exif:ThumbnailYCbCrCoefficients"), new ExifInfo(0x5038, "exif:ThumbnailYCbCrSubsampling"), new ExifInfo(0x5039, "exif:ThumbnailYCbCrPositioning"), new ExifInfo(0x503A, "exif:ThumbnailRefBlackWhite"), new ExifInfo(0x503B, "exif:ThumbnailCopyRight"), new ExifInfo(0x5090, "exif:LuminanceTable"), new ExifInfo(0x5091, "exif:ChrominanceTable"), new ExifInfo(0x5100, "exif:FrameDelay"), new ExifInfo(0x5101, "exif:LoopCount"), new ExifInfo(0x5110, "exif:PixelUnit"), new ExifInfo(0x5111, "exif:PixelPerUnitX"), new ExifInfo(0x5112, "exif:PixelPerUnitY"), new ExifInfo(0x5113, "exif:PaletteHistogram"), new ExifInfo(0x1000, "exif:RelatedImageFileFormat"), new ExifInfo(0x1001, "exif:RelatedImageLength"), new ExifInfo(0x1002, "exif:RelatedImageWidth"), new ExifInfo(0x800d, "exif:ImageID"), new ExifInfo(0x80e3, "exif:Matteing"), new ExifInfo(0x80e4, "exif:DataType"), new ExifInfo(0x80e5, "exif:ImageDepth"), new ExifInfo(0x80e6, "exif:TileDepth"), new ExifInfo(0x828d, "exif:CFARepeatPatternDim"), new ExifInfo(0x828e, "exif:CFAPattern2"), new ExifInfo(0x828f, "exif:BatteryLevel"), new ExifInfo(0x8298, "exif:Copyright"), new ExifInfo(0x829a, "exif:ExposureTime"), new ExifInfo(0x829d, "exif:FNumber"), new ExifInfo(0x83bb, "exif:IPTC/NAA"), new ExifInfo(0x84e3, "exif:IT8RasterPadding"), new ExifInfo(0x84e5, "exif:IT8ColorTable"), new ExifInfo(0x8649, "exif:ImageResourceInformation"), new ExifInfo(0x8769, "exif:ExifOffset"), new ExifInfo(0x8773, "exif:InterColorProfile"), new ExifInfo(0x8822, "exif:ExposureProgram"), new ExifInfo(0x8824, "exif:SpectralSensitivity"), new ExifInfo(0x8825, "exif:GPSInfo"), new ExifInfo(0x8827, "exif:ISOSpeedRatings"), new ExifInfo(0x8828, "exif:OECF"), new ExifInfo(0x8829, "exif:Interlace"), new ExifInfo(0x882a, "exif:TimeZoneOffset"), new ExifInfo(0x882b, "exif:SelfTimerMode"), new ExifInfo(0x9000, "exif:ExifVersion"), new ExifInfo(0x9003, "exif:DateTimeOriginal"), new ExifInfo(0x9004, "exif:DateTimeDigitized"), new ExifInfo(0x9101, "exif:ComponentsConfiguration"), new ExifInfo(0x9102, "exif:CompressedBitsPerPixel"), new ExifInfo(0x9201, "exif:ShutterSpeedValue"), new ExifInfo(0x9202, "exif:ApertureValue"), new ExifInfo(0x9203, "exif:BrightnessValue"), new ExifInfo(0x9204, "exif:ExposureBiasValue"), new ExifInfo(0x9205, "exif:MaxApertureValue"), new ExifInfo(0x9206, "exif:SubjectDistance"), new ExifInfo(0x9207, "exif:MeteringMode"), new ExifInfo(0x9208, "exif:LightSource"), new ExifInfo(0x9209, "exif:Flash"), new ExifInfo(0x920a, "exif:FocalLength"), new ExifInfo(0x920b, "exif:FlashEnergy"), new ExifInfo(0x920c, "exif:SpatialFrequencyResponse"), new ExifInfo(0x920d, "exif:Noise"), new ExifInfo(0x9211, "exif:ImageNumber"), new ExifInfo(0x9212, "exif:SecurityClassification"), new ExifInfo(0x9213, "exif:ImageHistory"), new ExifInfo(0x9214, "exif:SubjectArea"), new ExifInfo(0x9215, "exif:ExposureIndex"), new ExifInfo(0x9216, "exif:TIFF-EPStandardID"), new ExifInfo(0x927c, "exif:MakerNote"), new ExifInfo(0x9C9b, "exif:WinXP-Title"), new ExifInfo(0x9C9c, "exif:WinXP-Comments"), new ExifInfo(0x9C9d, "exif:WinXP-Author"), new ExifInfo(0x9C9e, "exif:WinXP-Keywords"), new ExifInfo(0x9C9f, "exif:WinXP-Subject"), new ExifInfo(0x9286, "exif:UserComment"), new ExifInfo(0x9290, "exif:SubSecTime"), new ExifInfo(0x9291, "exif:SubSecTimeOriginal"), new ExifInfo(0x9292, "exif:SubSecTimeDigitized"), new ExifInfo(0xa000, "exif:FlashPixVersion"), new ExifInfo(0xa001, "exif:ColorSpace"), new ExifInfo(0xa002, "exif:ExifImageWidth"), new ExifInfo(0xa003, "exif:ExifImageLength"), new ExifInfo(0xa004, "exif:RelatedSoundFile"), new ExifInfo(0xa005, "exif:InteroperabilityOffset"), new ExifInfo(0xa20b, "exif:FlashEnergy"), new ExifInfo(0xa20c, "exif:SpatialFrequencyResponse"), new ExifInfo(0xa20d, "exif:Noise"), new ExifInfo(0xa20e, "exif:FocalPlaneXResolution"), new ExifInfo(0xa20f, "exif:FocalPlaneYResolution"), new ExifInfo(0xa210, "exif:FocalPlaneResolutionUnit"), new ExifInfo(0xa214, "exif:SubjectLocation"), new ExifInfo(0xa215, "exif:ExposureIndex"), new ExifInfo(0xa216, "exif:TIFF/EPStandardID"), new ExifInfo(0xa217, "exif:SensingMethod"), new ExifInfo(0xa300, "exif:FileSource"), new ExifInfo(0xa301, "exif:SceneType"), new ExifInfo(0xa302, "exif:CFAPattern"), new ExifInfo(0xa401, "exif:CustomRendered"), new ExifInfo(0xa402, "exif:ExposureMode"), new ExifInfo(0xa403, "exif:WhiteBalance"), new ExifInfo(0xa404, "exif:DigitalZoomRatio"), new ExifInfo(0xa405, "exif:FocalLengthIn35mmFilm"), new ExifInfo(0xa406, "exif:SceneCaptureType"), new ExifInfo(0xa407, "exif:GainControl"), new ExifInfo(0xa408, "exif:Contrast"), new ExifInfo(0xa409, "exif:Saturation"), new ExifInfo(0xa40a, "exif:Sharpness"), new ExifInfo(0xa40b, "exif:DeviceSettingDescription"), new ExifInfo(0xa40c, "exif:SubjectDistanceRange"), new ExifInfo(0xa420, "exif:ImageUniqueID"), new ExifInfo(0xc4a5, "exif:PrintImageMatching"), new ExifInfo(0xa500, "exif:Gamma"), new ExifInfo(0xc640, "exif:CR2Slice"), new ExifInfo(0x10000, "exif:GPSVersionID"), new ExifInfo(0x10001, "exif:GPSLatitudeRef"), new ExifInfo(0x10002, "exif:GPSLatitude"), new ExifInfo(0x10003, "exif:GPSLongitudeRef"), new ExifInfo(0x10004, "exif:GPSLongitude"), new ExifInfo(0x10005, "exif:GPSAltitudeRef"), new ExifInfo(0x10006, "exif:GPSAltitude"), new ExifInfo(0x10007, "exif:GPSTimeStamp"), new ExifInfo(0x10008, "exif:GPSSatellites"), new ExifInfo(0x10009, "exif:GPSStatus"), new ExifInfo(0x1000a, "exif:GPSMeasureMode"), new ExifInfo(0x1000b, "exif:GPSDop"), new ExifInfo(0x1000c, "exif:GPSSpeedRef"), new ExifInfo(0x1000d, "exif:GPSSpeed"), new ExifInfo(0x1000e, "exif:GPSTrackRef"), new ExifInfo(0x1000f, "exif:GPSTrack"), new ExifInfo(0x10010, "exif:GPSImgDirectionRef"), new ExifInfo(0x10011, "exif:GPSImgDirection"), new ExifInfo(0x10012, "exif:GPSMapDatum"), new ExifInfo(0x10013, "exif:GPSDestLatitudeRef"), new ExifInfo(0x10014, "exif:GPSDestLatitude"), new ExifInfo(0x10015, "exif:GPSDestLongitudeRef"), new ExifInfo(0x10016, "exif:GPSDestLongitude"), new ExifInfo(0x10017, "exif:GPSDestBearingRef"), new ExifInfo(0x10018, "exif:GPSDestBearing"), new ExifInfo(0x10019, "exif:GPSDestDistanceRef"), new ExifInfo(0x1001a, "exif:GPSDestDistance"), new ExifInfo(0x1001b, "exif:GPSProcessingMethod"), new ExifInfo(0x1001c, "exif:GPSAreaInformation"), new ExifInfo(0x1001d, "exif:GPSDateStamp"), new ExifInfo(0x1001e, "exif:GPSDifferential"), new ExifInfo(0x00000, "null")};


            var lines = File.ReadAllLines("1.txt");
            List<String> ar= new List<string>();
            foreach(var l in lines){
                var s = l.Trim(' ');
                var tmp = s.Split(new char[]{',', ' ', '}', '{'});
                if (tmp.Length >= 5){
                    var id = tmp[3];
                    var name = tmp[5].Trim('"');
                    if (name == ""){
                        id = tmp[2];
                        name = tmp[4].Trim('"');
                    }
                    ar.Add(String.Format("new ExifInfo({0},\"{1}\"),", id, name));
                }
            }

            File.WriteAllLines("2.txt",ar.ToArray());


        }

    }
}
