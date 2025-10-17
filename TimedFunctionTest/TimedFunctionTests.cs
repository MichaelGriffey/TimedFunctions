using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TimedFunctionExample;

namespace TimedFunctionTest;
[TestClass]
    public class TimedFunctionTests
    {
        /// <summary>
        /// Ensures a quick synchronous function completes successfully within timeout.
        /// </summary>
        [TestMethod]
        public async Task Execute_SyncFunction_CompletesSuccessfully()
        {
            // Arrange
            int input = 5;
            int expected = input * 2;

            // Act
            int result = await TimedFunction.Execute<int, int>(
                x => x * 2,
                input,
                timeoutSeconds: 3);

            // Assert
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Ensures an asynchronous function completes successfully before the timeout.
        /// </summary>
        [TestMethod]
        public async Task ExecuteAsync_AsyncFunction_CompletesSuccessfully()
        {
            // Arrange
            int input = 3;

            // Act
            int result = await TimedFunction.ExecuteAsync<int, int>(
                async x =>
                {
                    await Task.Delay(500); // Simulate short async work
                    return x * 10;
                },
                input,
                timeoutSeconds: 3);

            // Assert
            Assert.AreEqual(30, result);
        }

        /// <summary>
        /// Ensures a TimeoutException is thrown if the task exceeds the timeout.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public async Task ExecuteAsync_Timeout_ThrowsTimeoutException()
        {
            // Arrange & Act
            await TimedFunction.ExecuteAsync<int, int>(
                async x =>
                {
                    await Task.Delay(3000); // Will exceed timeout
                    return x * 10;
                },
                2,
                timeoutSeconds: 1);
        }

        /// <summary>
        /// Ensures the provided CancellationToken triggers cancellation before timeout.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(OperationCanceledException))]
        public async Task ExecuteAsync_CancelledBeforeTimeout_ThrowsOperationCanceledException()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(500); // Cancel after half a second

            // Act
            await TimedFunction.ExecuteAsync<int, int>(
                async (x, token) =>
                {
                    await Task.Delay(5000, token); // Long delay that respects cancellation
                    return x * 10;
                },
                2,
                timeoutSeconds: 10,
                cancellationToken: cts.Token);
        }

        /// <summary>
        /// Ensures a synchronous function that takes multiple parameters via object[] works correctly.
        /// </summary>
        [TestMethod]
        public async Task Execute_SyncFunction_WithMultipleParams_CompletesSuccessfully()
        {
            // Arrange
            Func<object[], int> addNumbers = args =>
            {
                int a = (int)args[0];
                int b = (int)args[1];
                return a + b;
            };

            // Act
            int result = await TimedFunction.Execute(addNumbers, 2, CancellationToken.None, 3, 7);

            // Assert
            Assert.AreEqual(10, result);
        }

        /// <summary>
        /// Ensures an async function that uses multiple params completes correctly.
        /// </summary>
        [TestMethod]
        public async Task ExecuteAsync_AsyncFunction_WithMultipleParams_CompletesSuccessfully()
        {
            // Arrange
            Func<object[], Task<int>> multiplyNumbers = async args =>
            {
                await Task.Delay(200);
                int a = (int)args[0];
                int b = (int)args[1];
                return a * b;
            };

            // Act
            int result = await TimedFunction.ExecuteAsync(multiplyNumbers, 3, CancellationToken.None, 4, 5);

            // Assert
            Assert.AreEqual(20, result);
        }

        /// <summary>
        /// Ensures a synchronous function with a CancellationToken input works as expected when not cancelled.
        /// </summary>
        [TestMethod]
        public async Task Execute_SyncFunction_WithCancellationToken_CompletesSuccessfully()
        {
            // Arrange
            using var cts = new CancellationTokenSource();

            // Act
            int result = await TimedFunction.Execute<int, int>(
                (x, token) => x * 5,
                4,
                timeoutSeconds: 2,
                cancellationToken: cts.Token);

            // Assert
            Assert.AreEqual(20, result);
        }
    }


