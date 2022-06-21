using System.Text;
using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using Microsoft.Extensions.Options;

namespace LightBoard.Application.Services;

public class MailingTemplatesService : IMailingTemplatesService
{
    private readonly IBlobService _blobService;
    private readonly EmailTemplatesOptions _templatesOptions;
    private readonly IMailingTemplatesService _templatesService;

    public MailingTemplatesService(
        IBlobService blobService,
        IOptions<EmailTemplatesOptions> templatesOptions,
        IMailingTemplatesService templatesService)
    {
        _blobService = blobService;
        _templatesService = templatesService;
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
        var template = await _blobService.GetBlobStringContentAsync(BlobContainer.MailTemplates, templateBlobName);
        var templateBuilder = new StringBuilder(template);

        foreach (var pair in valuesToReplace)
        {
            templateBuilder.Replace(pair.Key, pair.Value);
        }

        return templateBuilder.ToString();
    }
}