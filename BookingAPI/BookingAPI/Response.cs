using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingAPI
{
    public class Response
    {

        /// <summary>
        /// Message returned
        /// </summary>
        /// 
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Data returned
        /// </summary>
        [JsonPropertyName("data")]
        public object Data { get; set; }

        public Response( String Message, object Data)
        {
            
            this.Message = Message;
            this.Data = Data;
        }
    }
}
