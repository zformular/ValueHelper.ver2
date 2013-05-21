using System;
using System.IO;
using ValueHelper.FileHelper.Base;
using System.Text;

namespace ValueHelper.FileHelper.Windows
{
    public class TextHelper : FileBase
    {
        public TextHelper() { }

        /// <summary>
        ///  设置要处理的文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SetFileName(string fileName)
        {
            base.SetParams(fileName);
        }

        public TextHelper(string fileName)
        {
            base.SetParams(fileName);
        }

        #region Create

        public override bool CreateFile()
        {
            return this.CreateFile(null, null);
        }

        public override bool CreateFile(string fileName)
        {
            return this.CreateFile(fileName, null);
        }

        public override bool CreateFile(string fileName, string text)
        {
            if (!String.IsNullOrEmpty(fileName))
                base.SetParams(fileName);

            if (CheckParams())
            {
                if (File.Exists(base.FileName))
                    return false;

                try
                {
                    base.CreateDirectory();
                    FileStream fileStream = new FileStream(base.FileName, FileMode.Create);
                    if (!String.IsNullOrEmpty(text))
                    {
                        Byte[] bytes = Encoding.Default.GetBytes(text);
                        fileStream.Write(bytes, 0, bytes.Length);
                    }
                    fileStream.Close();
                    fileStream.Dispose();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region Read

        public override string Read()
        {
            if (!CheckParams())
                throw new ArgumentNullException("请先绑定文件名");

            if (!File.Exists(base.FileName))
                throw new ArgumentNullException("文件不存在");

            return File.ReadAllText(base.FileName);
        }

        public override string Read(System.Text.Encoding encode)
        {
            if (!CheckParams())
                throw new ArgumentNullException("请先绑定文件名");

            if (!File.Exists(base.FileName))
                throw new ArgumentNullException("文件不存在");

            return File.ReadAllText(base.FileName, encode);
        }

        #endregion

        #region Write

        public override bool Write(string text)
        {
            return this.Write(text, false);
        }

        public override bool Write(string text, bool append)
        {
            if (!CheckParams())
                throw new ArgumentNullException("请先绑定文件名");

            if (!File.Exists(base.FileName))
                throw new ArgumentNullException("文件不存在");
            try
            {
                StreamWriter streamWriter = new StreamWriter(base.FileName, append);
                streamWriter.Write(text);
                streamWriter.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Write(string text, System.Text.Encoding encode)
        {
            if (!CheckParams())
                throw new ArgumentNullException("请先绑定文件名");

            if (!File.Exists(base.FileName))
                throw new ArgumentNullException("文件不存在");

            try
            {
                FileStream fileStream = new FileStream(base.FileName, FileMode.Open);
                Byte[] bytes = encode.GetBytes(text);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool WriteLine(string text)
        {
            return this.WriteLine(text, Encoding.Default);
        }

        public override bool WriteLine(string text, System.Text.Encoding encode)
        {
            return this.Write(String.Concat(text, Environment.NewLine), encode);
        }

        #endregion
    }
}
