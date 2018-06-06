# Asynchronous Programming in .NET

This repository contains some basic code samples for Dave Fancher's Asynchronous Programming in .NET Talk originally delivered to the Indy .NET Consortium user group in Fishers, IN on 6/6/2018.

The talk follows a basic flow through the history of parallel and asynchronous programming in .NET starting with a baseline synchronous process and tweaking that process to demonstrate traditional threading, the [Asynchronous Programming Model][3] (APM) approach with IAwaitResult, a variety of [Task Parallel Library][2] (TPL) examples, before wrapping up with some `async`/`await` examples.

A more detailed overview of working with the TPL is available on [Dave's blog][1].

A discussion of why `await`'s exception process is different than how raw `Task`s work is available on the [Visual Studio team blog][4].

[1]: https://davefancher.com/2011/03/10/parallel-programming-in-net-4/
[2]: https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl
[3]: https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/asynchronous-programming-model-apm
[4]: https://blogs.msdn.microsoft.com/pfxteam/2011/09/28/task-exception-handling-in-net-4-5/
