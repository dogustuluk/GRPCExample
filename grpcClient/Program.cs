/* yüklenmesi gereken kütüphaneler
 * Google.Protobuf : protobuf serialization ve deserialization işlemlerini yapan kütüphanedir.
 * Grpc.Net.Client : .Net mimarisine uygun gRPC kütüphanesidir.
 * Grpc.Tools      : Proto dosyalarını derlemek için gerekli compileri ve araçları içeren kütüphanedir.
 */

// See https://aka.ms/new-console-template for more information
using Google.Protobuf;
using Grpc.Net.Client;
using grpcFileTransportClient;

var channel = GrpcChannel.ForAddress("https://localhost:7051"); //-> bu adresteki grpc servisine bağlanır

var client = new FileService.FileServiceClient(channel);

string file = @"C:\Users\dogus\Downloads\CLove2006_flac_vnltue.rar";

using FileStream fileStream = new FileStream(file, FileMode.Open);

var content = new BytesContent
{
    FileSize = fileStream.Length,
    ReadedByte = 0,
    Info = new grpcFileTransportClient.FileInfo { FileName = Path.GetFileNameWithoutExtension(fileStream.Name), FileExtension = Path.GetExtension(fileStream.Name) }

};

var upload = client.FileUpload();
byte[] buffer = new byte[2048];

while ((content.ReadedByte = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
{
    content.Buffer = ByteString.CopyFrom(buffer);
    await upload.RequestStream.WriteAsync(content);
}

await upload.RequestStream.CompleteAsync();

fileStream.Close();


