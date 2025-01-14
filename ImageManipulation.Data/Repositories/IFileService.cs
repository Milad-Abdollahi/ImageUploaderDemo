using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation.Data.Repositories;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtentions);
    void DeleteFile(string fileNameWithExtension);
}
