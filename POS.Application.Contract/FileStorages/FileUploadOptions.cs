using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.FileStorages
{
    public class FileUploadOptions
    {
        public List<string> AllowedFileExtensions { get; set; } = new();
    }

}
