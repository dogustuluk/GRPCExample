/* yüklenmesi gereken kütüphaneler
 * Google.Protobuf : protobuf serialization ve deserialization işlemlerini yapan kütüphanedir.
 * Grpc.Net.Client : .Net mimarisine uygun gRPC kütüphanesidir.
 * Grpc.Tools      : Proto dosyalarını derlemek için gerekli compileri ve araçları içeren kütüphanedir.
 */

// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7051"); //-> bu adresteki grpc servisine bağlanır



//var greetClient = new Greeter.GreeterClient(channel); //-> Greeter olması gerektiğini proto dosyasındaki service adımızdan biliyoruz.
//HelloReply result = await greetClient.SayHelloAsync(new HelloRequest { Name = "Doğuş Tuluk" });

