using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagBlog.Services.Media
{
    public interface IMediaManager
    {
        Task<string> SaveFileAsync(
            Stream buffer,
            string originalFileName,
            string contentType,
            CancellationToken cancellationToken = default);
           Task<bool> DeleteFileASync( string filePath,CancellationToken cancellationToken=default );
    }
}
