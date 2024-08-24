

using System.Collections.Immutable;
using ImageManipulation.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controller;


[Route("api/[Controller]")]
[ApiController]
public class ImageController
(IFileService fileService)
: ControllerBase
{
    private readonly string imageDirectory = "./Images";
    private readonly IFileService fileService = fileService;

    private readonly ulong fileMaxSizeInBytes = 2 * 1024 * 1024;
    private readonly ImmutableArray<string> allowedExtensions
        = [".jpg", ".jpeg", ".png"];

    [HttpPost("upload")]
    [AllowAnonymous]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
    {
        if(image is null) return this.BadRequest("Invalid file.");

        FileFilterResult result = this.fileService
            .FilterFile(
                image,
                (image) => 
                    image.Length > 2 * 1024 * 1024 
                    ? FileFilterResult.TooBig : FileFilterResult.Ok,
                (image) => 
                    this.allowedExtensions.Contains(Path.GetExtension(image.FileName)) 
                    ? FileFilterResult.ExtensionNotAllowed : FileFilterResult.Ok
            );

        if(result is not FileFilterResult.Ok)
            return this.GetBadRequestResponse(result, image);
        
        //TODO: Add Image processing logic here
        string createdFileName = await this.fileService.SaveFileAsync(image);
        return this.Ok(createdFileName);
    }

    private BadRequestObjectResult GetBadRequestResponse(FileFilterResult filterResult, IFormFile image){
        return filterResult switch
        {
            FileFilterResult.ExtensionNotAllowed 
                => this.BadRequest($"Invalid file extension. Only {string.Join(",",this.allowedExtensions)} files are allowed."),
            FileFilterResult.TooBig
                => this.BadRequest($"File is too big ({image.Length} bytes). Maximum accepted file size is {this.fileMaxSizeInBytes} bytes."),
            _ 
                => this.BadRequest($"Something must have gone really wrong for yout to see this...") 
        };
    }


}