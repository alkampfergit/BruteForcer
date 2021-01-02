using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using BruteForce.Core.Http;
using BruteForce.Core.Http.Concrete;
using BruteForce.Core.Storage;
using Serilog;

namespace BruteForce.Core.ExecutionModels
{
    /// <summary>
    /// First version of execution with dataflow that actually
    /// works only with Http request
    /// </summary>
    public class TplExecution
    {
        private IRequester _requester;
        private Func<GenericResponse, Task> _callback;
        private BufferBlock<IDictionary<string, string>> _bufferBlock;
        private TransformBlock<IDictionary<string, string>, ResultRecord> _executeBlock;
        private ActionBlock<ResultRecord> _finalActionBlock;
        private IStorage _storage;
        private ActionBlock<ResultRecord[]> _storageBlock;

        public TplExecution WithRequester(IRequester requester)
        {
            _requester = requester;
            return this;
        }

        public TplExecution WithHttpGetRequester(string url)
        {
            _requester = new HttpRequester(new StandardHttpRequestEngine(), url, null);
            return this;
        }

        public TplExecution WriteToStorage(IStorage storage)
        {
            _storage = storage;
            return this;
        }

        /// <summary>
        /// Setup the callback that should be called in the final
        /// stage of execution.
        /// </summary>
        /// <param name="callback"></param>
        public TplExecution SetCallback(Func<GenericResponse, Task> callback)
        {
            _callback = callback;
            return this;
        }

        /// <summary>
        /// Initialize the engine, allows you to create the queue and
        /// it must be called before you can call <see cref="QueueAsync"/>
        /// </summary>
        /// <returns></returns>
        public TplExecution Init()
        {
            _bufferBlock = new BufferBlock<IDictionary<string, string>>(
                new DataflowBlockOptions()
                {
                    BoundedCapacity = 100
                });
            
            _executeBlock = new TransformBlock<IDictionary<string, string>, ResultRecord>(
                PerformSearch, new ExecutionDataflowBlockOptions()
                {
                    MaxDegreeOfParallelism = 50, //really make it parametric
                });

            var standardLinkoptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true,
            };
            _bufferBlock.LinkTo(_executeBlock, standardLinkoptions);
            
            //now depending on if we have a storage or does not have a storage, we change
            //form of the dataflow
            _finalActionBlock = new ActionBlock<ResultRecord>(r => _callback(r.Response), new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = 100,
            });
            
            if (_storage == null)
            {
                _executeBlock.LinkTo(_finalActionBlock, standardLinkoptions);
            }
            else
            {
                var propagationBlock = new BroadcastBlock<ResultRecord>(r => r);
                var accumulator = new BatchBlock<ResultRecord>(1000);
                _storageBlock = new ActionBlock<ResultRecord[]>(async b => _storage.AppendRecordsAsync(b));

                _executeBlock.LinkTo(propagationBlock, standardLinkoptions);
                propagationBlock.LinkTo(_finalActionBlock, standardLinkoptions);
                propagationBlock.LinkTo(accumulator, standardLinkoptions);
                accumulator.LinkTo(_storageBlock, standardLinkoptions);
            }

            return this;
        }

        private async Task<ResultRecord> PerformSearch(IDictionary<string, string> arg)
        {
            var id = GenerateId(arg);
            try
            {
                var response =await _requester.RunAsync(arg);
                return new ResultRecord(id, response);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in TPL Perform search: {message}", e.Message);
                return new ResultRecord("id", new GenericResponse($"ERROR: {e.Message}"));
            }
        }

        private string GenerateId(IDictionary<string, string> tokens)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var token in tokens)
            {
                sb.Append($"{token.Key}/{token.Value}-");
            }

            sb.Length--;

            return sb.ToString();
        }

        /// <summary>
        /// Queue a new request with tokens.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public Task QueueAsync(IDictionary<string, string> tokens)
        {
            return _bufferBlock.SendAsync(tokens);
        }

        public Task Finish()
        {
            _bufferBlock.Complete();
            if (_storageBlock == null)
            {
                return _finalActionBlock.Completion;
            }

            return Task.WhenAll( _finalActionBlock.Completion, _storageBlock.Completion);
        }
    }
}