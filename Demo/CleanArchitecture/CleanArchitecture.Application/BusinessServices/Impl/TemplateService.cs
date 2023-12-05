using System.Text;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.BusinessServices.Impl;

public class TemplateService : ITemplateService
{
    private const string _htmlTemplate = @"<!DOCTYPE html>
                                            <html>
                                            <head>
                                            <style>
                                            a:link, a:visited {
                                            background-color: #f44336;
                                            color: white;
                                            padding: 14px 25px;
                                            text-align: center;
                                            text-decoration: none;
                                            display: inline-block;
                                            }

                                            a:hover, a:active {
                                            background-color: #FB8504;
                                            }
                                            </style>
                                            </head>
                                                <body>
                                                [HTMLCONTENT]
                                                </body>
                                            </html>";
    public async Task<string> BuildAsync(TemplateType template, IDictionary<string, string> bodyContent)
    {
        var result = new StringBuilder(_htmlTemplate);

        result.Replace("[HTMLCONTENT]", await GetTemplateAsync(template));

        foreach (var item in bodyContent)
        {
            result.Replace($"[{item.Key}]", item.Value);
        }

        return result.ToString();

    }

    public async Task<string> GetTemplateAsync(TemplateType template)
    {
        var projectInvitationTemplate =
        @"<p>Dear [FirstName] [LastName].</p>
        <h3>Your team is waiting for you to join them</h3>
            <hr>
            <h4 >
            You've been invited to collaborate on [ProjectTitle]
            </h4 >
            <hr>
            <div><a href='[HostingUrl]/projects/[ProjectId]/member/accept-invitation?token=[Token]' target=_'blank'>Join the project</a></div>
            ";

        return template switch
        {
            TemplateType.ProjectInvitation => await new ValueTask<string>(projectInvitationTemplate),
            _ => await new ValueTask<string>("{0}")
        };
    }
}