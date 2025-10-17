namespace TimedFunctions;

/// <summary>
/// Provides methods to execute synchronous or asynchronous functions with a timeout, optional cancellation, and flexible support for multiple input parameters.
/// </summary>
public static class TimedFunction
{
    #region --- Synchronous Execution Wrappers ---

    /// <summary>
    /// Executes a synchronous function that returns <typeparamref name="TResult"/> but takes no parameters.
    /// </summary>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Function that returns <typeparamref name="TResult"/>.</param>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the asynchronous execution of the function, returning its result.</returns>
    /// <exception cref="TimeoutException">Thrown if the function takes longer than <paramref name="timeoutSeconds"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the provided cancellation token requests cancellation.</exception>
    public static Task Execute<TResult>(
        Func<Task> func,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
        => ExecuteAsync(Task.Run(func, cancellationToken), timeoutSeconds, cancellationToken);

    /// <summary>
    /// Executes a synchronous function that returns <typeparamref name="TResult"/> but takes no parameters.
    /// </summary>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Function that returns <typeparamref name="TResult"/>.</param>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the asynchronous execution of the function, returning its result.</returns>
    /// <exception cref="TimeoutException">Thrown if the function takes longer than <paramref name="timeoutSeconds"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the provided cancellation token requests cancellation.</exception>
    public static Task<TResult> Execute<TResult>(
        Func<Task<TResult>> func,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
        => ExecuteAsync(Task.Run(func, cancellationToken), timeoutSeconds, cancellationToken);

    /// <summary>
    /// Executes a synchronous function with a single strongly typed input parameter.
    /// </summary>
    /// <typeparam name="TInput">Type of the input parameter.</typeparam>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Function that accepts <typeparamref name="TInput"/> and returns <typeparamref name="TResult"/>.</param>
    /// <param name="input">The input parameter to pass to the function.</param>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the asynchronous execution of the function, returning its result.</returns>
    /// <exception cref="TimeoutException">Thrown if the function takes longer than <paramref name="timeoutSeconds"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the provided cancellation token requests cancellation.</exception>
    public static Task<TResult> Execute<TInput, TResult>(
        Func<TInput, TResult> func,
        TInput input,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
        => ExecuteAsync(Task.Run(() => func(input), cancellationToken), timeoutSeconds, cancellationToken);

    /// <summary>
    /// Executes a synchronous function with a single strongly typed input parameter and a CancellationToken.
    /// </summary>
    /// <typeparam name="TInput">Type of the input parameter.</typeparam>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Function that accepts <typeparamref name="TInput"/> and returns <typeparamref name="TResult"/>.</param>
    /// <param name="input">The input parameter to pass to the function.</param>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the asynchronous execution of the function, returning its result.</returns>
    /// <exception cref="TimeoutException">Thrown if the function takes longer than <paramref name="timeoutSeconds"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the provided cancellation token requests cancellation.</exception>
    public static Task<TResult> Execute<TInput, TResult>(
        Func<TInput, CancellationToken, TResult> func,
        TInput input,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
        => ExecuteAsync(Task.Run(() => func(input, cancellationToken), cancellationToken), timeoutSeconds, cancellationToken);

    /// <summary>
    /// Executes a synchronous function that accepts multiple parameters (via <c>params object[]</c>).
    /// The function is wrapped in a task and observed with a timeout.
    /// </summary>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">The function to execute. It accepts an object array and returns <typeparamref name="TResult"/>.</param>
    /// <param name="timeoutSeconds">Maximum time allowed (in seconds) before a timeout occurs.</param>
    /// <param name="cancellationToken">Optional cancellation token that allows early termination.</param>
    /// <param name="args">Parameters to pass into the function.</param>
    /// <returns>A task representing the asynchronous execution of the function, returning its result.</returns>
    /// <exception cref="TimeoutException">Thrown if the function takes longer than <paramref name="timeoutSeconds"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the provided cancellation token requests cancellation.</exception>
    public static Task<TResult> Execute<TResult>(
        Func<object[], TResult> func,
        int timeoutSeconds,
        CancellationToken cancellationToken = default,
        params object[] args)
        => ExecuteAsync(Task.Run(() => func(args), cancellationToken), timeoutSeconds, cancellationToken);

    #endregion

    #region --- Asynchronous Execution Wrappers ---

    /// <summary>
    /// Executes an asynchronous function (no parameters) with a timeout.
    /// </summary>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Asynchronous function that returns <see cref="Task{TResult}"/>.</param>
    /// <param name="timeoutSeconds">Maximum duration (in seconds) before timing out.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the function execution, returning its result.</returns>
    /// <exception cref="TimeoutException">Thrown if the operation exceeds the timeout.</exception>
    public static Task<TResult> ExecuteAsync<TResult>(
        Func<Task<TResult>> func,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
        => ExecuteAsync(func(), timeoutSeconds, cancellationToken);

    /// <summary>
    /// Executes an asynchronous function with a strongly typed input parameter and a timeout.
    /// </summary>
    /// <typeparam name="TInput">Input parameter type.</typeparam>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Asynchronous function that accepts <typeparamref name="TInput"/> and returns <see cref="Task{TResult}"/>.</param>
    /// <param name="input">Strongly typed input parameter to pass to the function.</param>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the function execution, returning its result.</returns>
    public static Task<TResult> ExecuteAsync<TInput, TResult>(
        Func<TInput, Task<TResult>> func,
        TInput input,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
        => ExecuteAsync(func(input), timeoutSeconds, cancellationToken);

    /// <summary>
    /// Executes an asynchronous function with a strongly typed input parameter and a timeout and a CancellationToken.
    /// </summary>
    /// <typeparam name="TInput">Input parameter type.</typeparam>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Asynchronous function that accepts <typeparamref name="TInput"/> and returns <see cref="Task{TResult}"/>.</param>
    /// <param name="input">Strongly typed input parameter to pass to the function.</param>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A task representing the function execution, returning its result.</returns>
    public static Task<TResult> ExecuteAsync<TInput, TResult>(
        Func<TInput, CancellationToken, Task<TResult>> func,
        TInput input,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
        => ExecuteAsync(func(input, cancellationToken), timeoutSeconds, cancellationToken);

    /// <summary>
    /// Executes an asynchronous function that accepts multiple parameters (via <c>params object[]</c>).
    /// </summary>
    /// <typeparam name="TResult">Return type of the function.</typeparam>
    /// <param name="func">Asynchronous function that accepts an object array and returns <see cref="Task{TResult}"/>.</param>
    /// <param name="timeoutSeconds">Timeout in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <param name="args">Parameters to pass to the function.</param>
    /// <returns>A task representing the function execution, returning its result.</returns>
    public static Task<TResult> ExecuteAsync<TResult>(
        Func<object[], Task<TResult>> func,
        int timeoutSeconds,
        CancellationToken cancellationToken = default,
        params object[] args)
        => ExecuteAsync(func(args), timeoutSeconds, cancellationToken);

    #endregion

    #region --- Task Timeout ---

    /// <summary>
    /// Executes a task that returns a <see cref="Task{TResult}"/> with a timeout and optional cancellation.
    /// </summary>
    /// <typeparam name="TResult">The result type of the task.</typeparam>
    /// <param name="task">The task to execute and observe.</param>
    /// <param name="timeoutSeconds">Timeout duration in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel before timeout.</param>
    /// <returns>The result of the task if it completes within the timeout.</returns>
    /// <exception cref="TimeoutException">Thrown if the function takes longer than <paramref name="timeoutSeconds"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the provided cancellation token requests cancellation.</exception>
    public static async Task<TResult> ExecuteAsync<TResult>(
        Task<TResult> task,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
    {
        await ExecuteCoreAsync(task, timeoutSeconds, cancellationToken).ConfigureAwait(false);
        return await task.ConfigureAwait(false);
    }


    /// <summary>
    /// Executes a function with a timeout and optional cancellation.
    /// </summary>
    /// <param name="task">The task to execute and observe.</param>
    /// <param name="timeoutSeconds">Timeout duration in seconds.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel before timeout.</param>
    /// <returns>void if it completes within the timeout.</returns>
    /// <exception cref="TimeoutException">Thrown if the function takes longer than <paramref name="timeoutSeconds"/>.</exception>
    /// <exception cref="OperationCanceledException">Thrown if the provided cancellation token requests cancellation.</exception>
    public static async Task ExecuteAsync(
        Task task,
        int timeoutSeconds,
        CancellationToken cancellationToken = default)
    {
        await ExecuteCoreAsync(task, timeoutSeconds, cancellationToken).ConfigureAwait(false);
        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Shared core logic for handling timeout and cancellation.
    /// </summary>
    private static async Task ExecuteCoreAsync(
        Task task,
        int timeoutSeconds,
        CancellationToken cancellationToken)
    {
        // Create a linked token source so we can cancel the delay task
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var delayTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), linkedCts.Token);

        // Wait for either the main task or the delay to complete
        var completedTask = await Task.WhenAny(task, delayTask).ConfigureAwait(false);

        if (completedTask == task)
        {
            // Cancel the delay to release resources
            linkedCts.Cancel();
            return;
        }

        // Timeout occurred
        cancellationToken.ThrowIfCancellationRequested();
        throw new TimeoutException("The function exceeded the timeout.");
    }

    #endregion
}
