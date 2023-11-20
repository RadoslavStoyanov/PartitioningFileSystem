using Partitioning.ServiceInterfaces.DistributedFileSystem;

namespace Partitioning.ServiceImplementations.DistributedFileSystem
{
    public sealed class DistributedFS : IDistributedFS
    {
        private readonly string filePath = string.Empty;
        private const string fileName = "sample3.txt";
        private const int one_hundred_megabytes_in_bytes = 100000000;
        private const int oneSecondInMilliseconds = 1000;

        private long? fileLength;

        private static object syncRoot = new object();
        private static SemaphoreSlim _readFileSemaphore = new SemaphoreSlim(1, 1);
        private static DistributedFS _instance;

        private DistributedFS()
        {
            FileInfo fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
            DirectoryInfo parentDir = fileInfo.Directory.Parent;

            filePath = GetPath(fileInfo, parentDir);
        }

        public long Threshold { get => one_hundred_megabytes_in_bytes; }
        public long FileLength { get => GetFileLength(); }

        public static DistributedFS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DistributedFS();
                        }
                    }
                }

                return _instance;
            }
        }

        private long GetFileLength()
        {
            if (!fileLength.HasValue)
            {
                Task.Delay(100);

                fileLength = new FileInfo(filePath).Length;
            }

            return fileLength.Value;
        }

        public Stream GetData(long offset)
        {
            Task.Delay(100);

            var semaphoreAcquired = false;

            try
            {
                semaphoreAcquired = _readFileSemaphore.Wait(oneSecondInMilliseconds);

                var buffer = new byte[one_hundred_megabytes_in_bytes];

                using (var fileStream = File.OpenRead(filePath))
                {
                    fileStream.Seek(offset, SeekOrigin.Begin);
                    fileStream.Read(buffer, 0, buffer.Length);

                    return new MemoryStream(buffer, 0, buffer.Length, false, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                throw;
            }
            finally
            {
                if (semaphoreAcquired)
                {
                    _readFileSemaphore.Release();
                }
                else
                {
                    throw new Exception("Semaphore could not be acquired");
                }
            }
        }

        private string GetPath(FileInfo fileInfo, DirectoryInfo parentDir)
        {
            var textFilePath = parentDir
                .EnumerateFileSystemInfos()
                .Select(x => x.FullName)
                .FirstOrDefault(x => x.Contains(@"Partitioning\Resources"));

            if (textFilePath != null)
            {
                return @$"{textFilePath}\{fileName}";
            }

            parentDir = parentDir.Parent;

            return GetPath(fileInfo, parentDir);
        }
    }
}
