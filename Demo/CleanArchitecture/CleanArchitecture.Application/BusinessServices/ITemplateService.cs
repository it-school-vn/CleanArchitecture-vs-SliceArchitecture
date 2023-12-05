using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.BusinessServices;

public interface ITemplateService
{
    Task<string> GetTemplateAsync(TemplateType template);

    Task<string> BuildAsync(TemplateType template, IDictionary<string, string> bodyContent);
}