using FluentValidation;

namespace MinimalApi.Constants;

public class GithubIssueValidator : AbstractValidator<GithubIssue>
{
    public GithubIssueValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.Author).NotEmpty();
    }
}