using System;
using System.IO;
using ValueHelper.FileHelper.Windows;
using ValueHelper.FileHelper.Base;

namespace ValueHelper.FileHelper
{
    public class FileManager
    {
        public static Boolean CreateFile(String fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            FileBase fileBase = new TextHelper(fileName);
            var result = fileBase.CreateFile();
            fileBase.Dispose();

            return result;
        }

        public static TextHelper GetTextHelper()
        {
            return new TextHelper();
        }

        public static BinaryHelper GetBinaryHelper()
        {
            return new BinaryHelper();
        }
    }
}
