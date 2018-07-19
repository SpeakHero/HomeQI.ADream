using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
namespace HomeQI.ADream.Infrastructure.Utilities
{
    public class CheakCode
    {
        #region 构造

        /// <summary>
        /// 验证码类,随机生成4到5个字符
        /// <example>
        ///CheakCode cheakcode = new CheakCode();
        ///
        ///Response.ClearContent();
        ///
        ///Response.ContentType = "image/Gif";
        ///
        ///Response.BinaryWrite(cheakcode.GetImgWithValidateCode().ToArray());
        ///
        ///cheakcode = null;
        /// </example>
        /// </summary>
        public CheakCode() { }

        public CheakCode(string codeStr, int h, int w, int codeNum, int fSize)
        {
            CodeStr = codeStr;
            Height = h;
            Width = w;
            CodeNum = codeNum;
            FontSize = fSize;
        }

        public CheakCode(string codeStr)
        {
            CodeStr = codeStr;
        }

        public CheakCode(int fSize, string codeStr)
        {
            CodeStr = codeStr;
            FontSize = fSize;
        }

        public CheakCode(string codeStr, int codeNum)
        {
            CodeNum = codeNum;
            CodeStr = codeStr;
        }

        public CheakCode(int w, int h)
        {
            Height = h;
            Width = w;
        }

        /// <summary>
        /// 验证码彩条宽度
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// 验证码彩条高度
        /// </summary>
        public int Height { get; set; } = 40;

        /// <summary>
        /// 验证码彩条需要产生的个数
        /// </summary>
        public int CodeNum { get; set; } = new Random().Next(4, 6);

        /// <summary>
        /// 验证码字符串 默认就拿Guid 产生字符
        /// </summary>
        public string CodeStr { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

        /// <summary>
        /// 验证码彩条字体大小
        /// </summary>
        public int FontSize { get; set; } = 15;

        /// <summary>
        /// 彩条字体,我这里定义了一些,使用中随机抽取一种 使用系统中的 随机凑齐
        /// </summary>
        private String[] fontFamily = new InstalledFontCollection().Families.AsQueryable().Select(d => d.Name).ToArray();
        //{ "Montezumas Revenge","Algerian", "Verdana", "Comic Sans MS", "Impact", "Haettenschweiler",
        // "Lucida Sans Unicode", "Garamond", "Courier New", "Book Antiqua", "Arial Narrow" };
        /// <summary>
        /// 产生验证码字符串
        /// </summary>                                                                                                              
        protected string GetValidateCode
        {
            get
            {
                Random rd = new Random(); //创建随机数对象

                string tempStr = null;
                for (int i = 0; i < CodeNum; i++)
                {
                    tempStr = tempStr + CodeStr[rd.Next(CodeStr.Length - 1)];
                }
                return tempStr;
            }
            private set { }
        }

        #endregion 属性

        /// <summary>
        /// 随机字符串，随机颜色背景，和随机线条产生的Image
        /// 返回 MemoryStream
        /// </summary>
        /// <returns>MemoryStream</returns>
        public MemoryStream GetImgWithValidateCode(out string code)
        {
            //声明位图
            Bitmap bitMap = null;
            ///画笔
            var pen = new Pen(Color.Black, 2);
            Graphics gph = null;
            var vcode = GetValidateCode;
            MemoryStream memStream = new MemoryStream();
            Random random = new Random();
            int fontWidth = (int)Math.Round(Width / (CodeNum + 2) / 1.0);  //这里主要是适应字符的字符,让其能全部显示出来
            int fontHeight = (int)Math.Round(Height / 1.0);
            FontSize = fontWidth <= fontHeight ? fontWidth : fontHeight; //取字体大小中间值
            using (bitMap = new Bitmap(Width, Height))
            {
                gph = Graphics.FromImage(bitMap);
                gph.Clear(GetControllableColor(200));
                PointF Cpoint1 = new PointF(5, 5);
                GraphicsPath graphPath = new GraphicsPath();
                int x1 = 0, y1 = 0;
                ///通过循环控制每一个字符
                for (int i = 0; i < vcode.Length; i++)
                {
                    //随机字符位置
                    x1 = random.Next(10) + 15 * i;
                    y1 = random.Next(bitMap.Height / 4);
                    Cpoint1 = new PointF(x1, y1);
                    SolidBrush solidBrush = new SolidBrush(Color.FromArgb(random.Next(100), random.Next(100), random.Next(100)));  //调整颜色,使其文字能看见
                    Font textFont = new Font(fontFamily[random.Next(fontFamily.Length - 1)], FontSize, FontStyle.Bold);
                    gph.TranslateTransform(10, 0);
                    Matrix transform = gph.Transform;
                    var chars = vcode.Substring(i, 1);
                    if (i % 2 == 0)  //这里我做了大小写处理 true 大写了
                    {
                        chars = chars.ToUpper();
                    }
                    gph.DrawString(vcode.Substring(i, 1), textFont, solidBrush, Cpoint1);  //写字符
                    gph.ResetTransform(); //重置矩阵
                }
                RectangleF rect = new RectangleF(random.Next(0, 1), random.Next(0, 1), Width, Height);
                float startAngle = random.Next(4, Height);
                float sweepAngle = random.Next(5, Width);
                gph.DrawArc(pen, rect, startAngle, sweepAngle);
                gph.DrawRectangle(pen, 0, 0, bitMap.Width - 1, bitMap.Height - 1);// //画图片的边框线
                try
                {
                    bitMap.Save(memStream, ImageFormat.Gif);//保存内存流
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    bitMap.Dispose();
                    random = null;
                    gph.Dispose();
                    pen.Dispose();
                    graphPath.Dispose();
                }
                code = vcode;
                return memStream;
            }
        }

        /// <summary>
        /// 产生随机颜色(R,G,B)
        /// </summary>
        /// <param name="colorBase"></param>
        /// <returns>背景色</returns>
        private Color GetControllableColor(int colorBase)
        {
            Color color = Color.Black;
            if (colorBase > 200)
            {
                return color;
            }
            Random random = new Random();
            color = Color.FromArgb(random.Next(56) + colorBase, random.Next(56) + colorBase, random.Next(56) + colorBase);
            random = null;
            return color;
        }

        /// <summary>
        /// 判断验证码是否正确
        /// </summary>
        /// <param name="inputValCode">待判断的验证码</param>
        /// <returns>正确返回 true,错误返回 false</returns>
        public bool IsRight(string inputValCode)
        {
            return GetValidateCode.ToUpper().Equals(inputValCode.ToUpper());
        }
    }
}
