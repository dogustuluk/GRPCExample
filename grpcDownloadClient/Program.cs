using Grpc.Net.Client;
using grpcFileTransportDownloadClient;

var channel = GrpcChannel.ForAddress("https://localhost:7051"); //-> bu adresteki grpc servisine bağlanır

var client = new FileService.FileServiceClient(channel);

string downloadPath = @"D:\\repos\\GRPCExample\\grpcDownloadClient\\downloadFiles\\";

var fileInfo = new grpcFileTransportDownloadClient.FileInfo
{
    FileExtension = ".rar",
    FileName = "CLove2006_flac_vnltue"
};

FileStream fileStream = null;

var request = client.FileDownload(fileInfo);

CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


int count = 0;
decimal chunkSize = 0;
while (await request.ResponseStream.MoveNext(cancellationTokenSource.Token))
{
    if (count++ == 0)
    {
        fileStream = new FileStream($@"{downloadPath}\{request.ResponseStream.Current.Info.FileName}{request.ResponseStream.Current.Info.FileExtension}", FileMode.CreateNew);

        fileStream.SetLength(request.ResponseStream.Current.FileSize);
    }

    var buffer = request.ResponseStream.Current.Buffer.ToByteArray();
    await fileStream.WriteAsync(buffer, 0, request.ResponseStream.Current.ReadedByte);

    Console.WriteLine($"{Math.Round(((chunkSize += request.ResponseStream.Current.ReadedByte) * 100) / request.ResponseStream.Current.FileSize)}%");
}

System.Console.WriteLine("...Yüklendi");
await fileStream.DisposeAsync();
fileStream.Close();
