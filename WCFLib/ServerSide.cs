using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace WCFLib
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class ServerSide : IServerSide
    {
        public string GetData()
        {
            return DateTime.Now.ToString();
        }

        public string WriteMethod(string msg)
        {
            try
            {
                //DataContainer.MsgList.Add(msg);
                return "Success!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public void TransferFile(FileTransferMessage request)
        {
            string logInfo = string.Format("开始接收文件,name={0}", request.FileName); //写日志
            string uploadFolder = Path.Combine(HttpContext.Current.Server.MapPath("/"), "upload");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            string savePath = request.SavePath;
            string fileName = request.FileName;
            Stream sourceStream = request.FileData;
            FileStream targetStream = null;
            if (!sourceStream.CanRead)
            {
                throw new Exception("数据流不可读!");
            }
            int fileSize = 0;
            string filePath = Path.Combine(uploadFolder, fileName);
            try
            {
                using (targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    const int bufferLen = 4096;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;
                    while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                    {
                        targetStream.Write(buffer, 0, count);
                        fileSize += count;
                    }
                    targetStream.Close();
                    sourceStream.Close();
                }
            }
            catch (Exception ex)
            {
                //日志
            }
        }
    }
}
