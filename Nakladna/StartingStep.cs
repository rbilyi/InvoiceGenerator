﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<bool> RunAsync()
        {
            try
            {
                var r = await Task.Run(Action);
                Failed = !r;
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