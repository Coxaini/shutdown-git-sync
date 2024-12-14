using Shutdown.Monitor.Git.Common.Exceptions;

namespace Shutdown.Monitor.Git.Common.Results;

public class GitResultBase
{
    public Exception Exception { get; init; }

    protected GitResultBase(Exception? exception)
    {
        Exception = exception!;
    }

    public bool IsSuccess => Exception == null!;
    public bool IsFailure => !IsSuccess;
}

public class GitResult : GitResultBase
{
    protected GitResult(Exception? exception) : base(exception)
    {
    }

    public static GitResult Success() => new GitResult(null);
    public static GitResult Failure(Exception exception) => new GitResult(exception);

    public static implicit operator GitResult(GitException exception) => Failure(exception);
}

public class GitResult<T> : GitResultBase
{
    public T Value { get; init; }

    private GitResult(T? value, Exception? exception) : base(exception!)
    {
        Value = value!;
    }

    public static GitResult<T> Success(T result) => new GitResult<T>(result, null);
    public static GitResult<T> Failure(Exception exception) => new GitResult<T>(default!, exception);

    public static implicit operator GitResult<T>(T value) => Success(value);

    public static implicit operator GitResult<T>(GitException exception) => Failure(exception);
}