using Microsoft.Extensions.Localization;

namespace MyProject.Contracts.Localization;

public interface IResourceLocalizer
{
    IStringLocalizer<Error> Error { get; }
    IStringLocalizer<Message> Message { get; }
    IStringLocalizer<Module> Module { get; }
}