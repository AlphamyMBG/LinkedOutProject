using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ImageManipulation.Data.Services;

public enum FileFilterResult
{
    Ok,
    ExtensionNotAllowed,
    TooBig
}

public interface IFileService
{
    FileFilterResult FilterFile(IFormFile formFile, params IEnumerable<Func<IFormFile, FileFilterResult>> filters);
    Task<string> SaveFileAsync(IFormFile imageFile);
    void DeleteFile(string fileNameWithExtension);
}

public class FileService(IWebHostEnvironment environment) : IFileService
{
    public FileFilterResult FilterFile(
        IFormFile formFile, 
        params IEnumerable<Func<IFormFile, FileFilterResult>> filters
    )
    {
        foreach( var filter in filters )
        {
            FileFilterResult result = filter.Invoke(formFile);
            if(result is not FileFilterResult.Ok) return result;
        }
        return FileFilterResult.Ok;
    }

    public async Task<string> SaveFileAsync(IFormFile imageFile)
    {
        ArgumentNullException.ThrowIfNull(imageFile);

        var contentPath = environment.ContentRootPath;
        var path = contentPath;
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // generate a unique filename
        var ext = Path.GetExtension(imageFile.Name);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        return fileName;
    }


    public void DeleteFile(string fileNameWithExtension)
    {
        if (string.IsNullOrEmpty(fileNameWithExtension))
        {
            throw new ArgumentNullException(nameof(fileNameWithExtension));
        }
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Invalid file path");
        }
        File.Delete(path);
    }

}