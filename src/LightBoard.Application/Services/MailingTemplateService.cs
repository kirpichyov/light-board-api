using System.Text;
using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using Microsoft.Extensions.Options;

namespace LightBoard.Application.Services;

public class MailingTemplateService : IMailingTemplateService
{
    private readonly IBlobService _blobService;
    private readonly EmailTemplatesOptions _templatesOptions;

    public MailingTemplateService(
        IBlobService blobService, 
        IOptions<EmailTemplatesOptions> templatesOptions)
    {
        _blobService = blobService;
        _templatesOptions = templatesOptions.Value;
    }

    public async Task<string> GetResetPasswordCodeTemplate(string code)
    {
        var pairs = new Dictionary<string, string>()
        {
            { "{code}", code },
        };

        return await FetchAndFillTemplate(_templatesOptions.PasswordResetTemplateFilename, pairs);
    }

    public async Task<string> GetConfirmEmailTemplate(string code, string username)
    {
        var pairs = new Dictionary<string, string>()
        {
            { "{code}", code },
            { "{username}", username }
        };

        return await FetchAndFillTemplate(_templatesOptions.EmailConfirmationTemplateFilename, pairs);
    }

    private async Task<string> FetchAndFillTemplate(string templateBlobName, Dictionary<string, string> valuesToReplace)
    {
        var template = await _blobService.GetBlobStringContentAsync(BlobContainer.MailingTemplates, templateBlobName);
        var templateBuilder = new StringBuilder(template);

        foreach (var pair in valuesToReplace)
        {
            templateBuilder.Replace(pair.Key, pair.Value);
        }

        return templateBuilder.ToString();
    }
}