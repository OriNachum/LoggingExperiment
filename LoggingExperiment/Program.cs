using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Serilog.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace LoggingExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            var serilogger = new LoggerConfiguration()
                .WriteTo.Log4Net("log4net")
                .WriteTo.File(@".\file.log")
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions
                {
                    BufferBaseFilename = @".\ElasticsearchBuffer.log",
                }).CreateLogger();
            using (var loggerFactory = LoggerFactory.Create(builder => builder.Services
                    .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger: serilogger, dispose: true)
                                                                .AddConsole())))
            {
                Microsoft.Extensions.Logging.ILogger logger = loggerFactory.CreateLogger("Logi");
                logger.LogInformation("It's alive! Index: {Index}, AnotherIndex: {AnotherIndex}", 1, "HelloWorld");
            }
            

            Console.WriteLine("Hello World!");
        }
    }
}
