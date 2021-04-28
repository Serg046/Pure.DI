﻿namespace Pure.DI.Core
{
    using System;
    using System.Collections.Generic;

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Tracer : ITracer, IDisposable
    {
        private readonly IDiagnostic _diagnostic;
        private readonly ILog<Tracer> _log;
        private readonly Stack<Dependency> _path = new();

        public Tracer(
            IDiagnostic diagnostic,
            ILog<Tracer> log)
        {
            _diagnostic = diagnostic;
            _log = log;
        }

        public IDisposable RegisterResolving(Dependency dependency)
        {
            if (_path.Count <= 256)
            {
                _path.Push(dependency);
                _log.Trace(() => 
                {
                    List<string> messages = new();
                    var offset = 0;
                    foreach (var item in _path)
                    {
                        messages.Add($"{new string(' ', offset)}{item}");
                        offset += 2;
                    }

                    return messages.ToArray();
                });
                return this;
            }

            var error = $"Circular dependency detected resolving {dependency}.";
            _diagnostic.Error(Diagnostics.CircularDependency, error, dependency.Binding.Location);
            throw new HandledException(error);
        }

        void IDisposable.Dispose()
        {
            if (_path.Count == 0)
            {
                _log.Warning("The path is empty.");
                return;
            }

            _path.Pop();
        }
    }
}