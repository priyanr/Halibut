// Copyright 2012-2013 Octopus Deploy Pty. Ltd.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Newtonsoft.Json;

namespace Halibut.Transport.Protocol
{
    public class ResponseMessage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("error")]
        public ServerError Error { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }

        public static ResponseMessage FromResult(RequestMessage request, object result)
        {
            return new ResponseMessage { Id = request.Id, Result = result };
        }

        public static ResponseMessage FromError(RequestMessage request, string message)
        {
            return new ResponseMessage { Id = request.Id, Error = new ServerError { Message = message } };
        }

        public static ResponseMessage FromException(RequestMessage request, Exception ex)
        {
            return new ResponseMessage {Id = request.Id, Error = new ServerError { Message = ex.Message, Details = ex.ToString() }};
        }
    }
}