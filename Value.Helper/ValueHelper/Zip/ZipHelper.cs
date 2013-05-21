using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Collections.Generic;
using ValueHelper.Zip.Infrastructure;

namespace ValueHelper.Zip
{
    public class ZipHelper
    {
        #region 加压

        /// <summary>
        ///  加压文件
        /// </summary>
        /// <param name="srcfileNames">带加压得文件名列表</param>
        /// <param name="dstfileName">加压后的文件名称</param>
        public Boolean CompressFiles(String[] srcfileNames, String dstfileName)
        {
            try
            {
                // 用于加压的对象
                ZipOutputStream zos = new ZipOutputStream(System.IO.File.Create(dstfileName));
                // 加压的等级
                zos.SetLevel(6);

                // 创建压缩文件
                this.compressFile(zos, srcfileNames);
                zos.Finish();
                zos.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  创建文件(用于创建压缩文件)
        /// </summary>
        private void compressFile(ZipOutputStream zos, String[] fileNames)
        {
            Crc32 crc = new Crc32();

            foreach (String name in fileNames)
            {
                FileStream fs = System.IO.File.OpenRead(name);
                Byte[] buffer = new Byte[fs.Length];

                fs.Read(buffer, 0, Convert.ToInt32(fs.Length));

                String file = Path.GetFileName(name);

                ZipEntry entry = new ZipEntry(file);
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                zos.PutNextEntry(entry);
                zos.Write(buffer, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }
            zos.CloseEntry();
            zos.Close();
            zos.Dispose();
        }

        #endregion

        #region 解压

        /// <summary>
        ///  解压到当前压缩文件 文件夹下
        /// </summary>
        public Boolean DecompressFile(String fileName, CompressType type)
        {
            String savePath = Path.GetDirectoryName(fileName);
            if (type == CompressType.CurrentFilePath)
                savePath = Path.Combine(savePath, Path.GetFileNameWithoutExtension(fileName));

            return DecompressFile(fileName, savePath);
        }

        /// <summary>
        ///  解压到指定路径
        /// </summary>
        public Boolean DecompressFile(String fileName, String savePath)
        {
            try
            {
                ZipInputStream zipInput = new ZipInputStream(System.IO.File.OpenRead(fileName));

                ZipEntry fileEntry;
                this.createDirect(savePath);

                while ((fileEntry = zipInput.GetNextEntry()) != null)
                {
                    String entryName = Path.GetFileName(fileEntry.Name);
                    if (entryName != String.Empty)
                    {
                        entryName = savePath + "\\" + entryName;
                        FileStream streamWriter = System.IO.File.Create(entryName);
                        Byte[] buffer = new Byte[zipInput.Length];
                        Int32 size = zipInput.Read(buffer, 0, Int32.Parse(zipInput.Length.ToString()));
                        streamWriter.Write(buffer, 0, size);
                        streamWriter.Close();
                    }
                }
                zipInput.Close();
                zipInput.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  创建指定目录
        /// </summary>
        /// <param name="path"></param>
        private void createDirect(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endregion
    }
}
