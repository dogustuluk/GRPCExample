using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using grpcFileTransportServer;
using static grpcFileTransportServer.FileService;

namespace grpcServer.Services;

public class FileTransportService : FileServiceBase
{
    readonly IWebHostEnvironment _webHostEnvironment;

    public FileTransportService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public override async Task<Empty> FileUpload(IAsyncStreamReader<BytesContent> requestStream, ServerCallContext context)
    {
        //stream'in yapılacağı path'i belirle
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "files");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        FileStream fileStream = null;

        try
        {
            int count = 0;
            decimal chunkSize = 0;

            while (await requestStream.MoveNext())
            {
                if (count++ == 0)
                {
                    fileStream = new FileStream($"{path}/{requestStream.Current.Info.FileName}{requestStream.Current.Info.FileExtension}", FileMode.CreateNew);
                    fileStream.SetLength(requestStream.Current.FileSize);
                }

                var buffer = requestStream.Current.Buffer.ToByteArray(); //stream'in her bir parçası 

                await fileStream.WriteAsync(buffer, 0, buffer.Length);

                Console.WriteLine($"{Math.Round(((chunkSize += requestStream.Current.ReadedByte) * 100) / requestStream.Current.FileSize)}%");
            }

        }
        catch (Exception ex)
        {

            throw;
        }

        await fileStream.DisposeAsync();
        fileStream.Close();
        return new Empty();
    }
}
