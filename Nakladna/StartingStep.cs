using System;
using System.Threading.Tasks;

namespace Nakladna
{
    internal class StartingStep
    {
        public string Title { get; protected set; }
        public bool Completed { get; protected set; }
        public bool Failed { get; protected set; }
        public string Message { get; protected set; }
        public Func<bool> Action { get; }

        public StartingStep(string title, Func<bool> action)
        {
            Title = title;
            Action = action;
        }

        /// <summary>
        /// Runs the step asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RunAsync()
        {
            try
            {
                var result = await Task.Run(Action);

                Failed = !result;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Failed = true;
            }
            finally
            {
                Completed = true;
            }
            return !Failed;
        }
    }
}
