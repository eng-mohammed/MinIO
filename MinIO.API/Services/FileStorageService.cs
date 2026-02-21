using Minio;
using Minio.DataModel.Args;
namespace MinIO.API.Services
{

    public class FileStorageService
    {
        private readonly IMinioClient _minio;
        private const string BucketName = "appstore";

        public FileStorageService(IMinioClient minio)
        {
            _minio = minio;
        }

        public async Task UploadAsync(IFormFile file)
        {
            // تأكد من وجود الـ Bucket
            bool exists = await _minio.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(BucketName));

            if (!exists)
            {
                await _minio.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(BucketName));
            }

            using var stream = file.OpenReadStream();

            await _minio.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(Guid.NewGuid() + Path.GetExtension(file.FileName))
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType));
        }

        public async Task<string> GenerateUploadUrl(string fileName)
        {
            return await _minio.PresignedPutObjectAsync(
                new PresignedPutObjectArgs()
                    .WithBucket("appstore")
                    .WithObject(fileName)
                    .WithExpiry(60 * 60) // ساعة
            );
        }
    }
}
