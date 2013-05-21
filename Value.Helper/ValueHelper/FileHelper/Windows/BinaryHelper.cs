using System;
using ValueHelper.FileHelper.Base;
using System.IO;
using System.Text;

namespace ValueHelper.FileHelper.Windows
{
    public class BinaryHelper : FileBase
    {
        public BinaryHelper() { }

        /// <summary>
        ///  设置要处理的文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SetFileName(string fileName)
        {
            base.SetParams(fileName);
        }

        public BinaryHelper(string fileName)
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
                        BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                        binaryWriter.Write(text);
                        binaryWriter.Flush();
                        binaryWriter.Close();
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

        #region Write

        public override bool Write(string text)
        {
            return this.Write(text, Encoding.Default);
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
                BinaryWriter binaryWriter = new BinaryWriter(fileStream, encode);
                binaryWriter.Write(text);
                binaryWriter.Flush();
                binaryWriter.Close();
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Write(string text, bool append)
        {
            if (append)
                return this.Write(text, Encoding.Default);
            else
            {
                if (!CheckParams())
                    throw new ArgumentNullException("请先绑定文件名");

                if (!File.Exists(base.FileName))
                    throw new ArgumentNullException("文件不存在");
                try
                {
                    FileStream fileStream = new FileStream(base.FileName, FileMode.Create);
                    BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.Default);
                    binaryWriter.Write(text);
                    binaryWriter.Flush();
                    binaryWriter.Close();
                    fileStream.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public override bool WriteLine(string text)
        {
            return this.Write(String.Concat(text, Environment.NewLine));
        }

        public override bool WriteLine(string text, System.Text.Encoding encode)
        {
            return this.Write(String.Concat(text, Environment.NewLine), encode);
        }

        #endregion

        #region Read

        public override string Read()
        {
            return this.Read(Encoding.Default);
        }

        public override string Read(System.Text.Encoding encode)
        {
            if (!CheckParams())
                throw new ArgumentNullException("请先绑定文件名");

            if (!File.Exists(base.FileName))
                throw new ArgumentNullException("文件不存在");

            FileStream fileStream = new FileStream(base.FileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream, encode);
            Byte[] buffer = new Byte[fileStream.Length];
            binaryReader.Read(buffer, 0, (Int32)fileStream.Length);
            return encode.GetString(buffer);
        }

        #endregion
    }
}
