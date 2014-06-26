using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DataExchange.Remote
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IServerSide
    {

        [OperationContract]
        string GetData();

        [OperationContract]
        string WriteMethod(string msg);

        [OperationContract(Action = "UploadFile")]
        void TransferFile(FileTransferMessage request);//文件传输

        [OperationContract]
        FileTransferMessage GetInfo();//获取文件

        // TODO: 在此添加您的服务操作
    }
    [MessageContract]
    public class FileTransferMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public string SavePath;//文件保存路径

        [MessageHeader(MustUnderstand = true)]
        public string FileName;//文件名称

        [MessageBodyMember(Order = 1)]
        public Stream FileData;//文件传输时间
    }
}
