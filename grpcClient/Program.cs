/* yüklenmesi gereken kütüphaneler
 * Google.Protobuf : protobuf serialization ve deserialization işlemlerini yapan kütüphanedir.
 * Grpc.Net.Client : .Net mimarisine uygun gRPC kütüphanesidir.
 * Grpc.Tools      : Proto dosyalarını derlemek için gerekli compileri ve araçları içeren kütüphanedir.
 */

// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using grpcFileTransportClient;

var channel = GrpcChannel.ForAddress("https://localhost:7051"); //-> bu adresteki grpc servisine bağlanır

var client = new FileService.FileServiceClient(channel);

string file = "";

FileStream fileStream = new FileStream(file, FileMode.Open);

var content = new BytesContent
{
    FileSize = fileStream.Length,
    ReadedByte = 0,
    Info = new grpcFileTransportClient.FileInfo { FileName = Path.GetFileNameWithoutExtension(fileStream.Name), FileExtension = Path.GetExtension(fileStream.Name) }

};