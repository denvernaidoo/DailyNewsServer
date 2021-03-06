using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Interfaces.Files;
using DailyNewsServer.Core.Models.Communication.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsServer.Infrastructure.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private IAmazonS3 _s3Client;

        public FilesRepository(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles)
        {
            var response = new List<string>();

            foreach(var file in formFiles)
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = file.FileName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                using (var fileTransferUtility = new TransferUtility(_s3Client))
                {
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }

                var expiryUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    Expires = DateTime.Now.AddDays(1)
                };

                var url = _s3Client.GetPreSignedURL(expiryUrlRequest);

                response.Add(url);

            }

            return new AddFileResponse
            {
                PreSignedUrl = response
            };
        }
    }
}
