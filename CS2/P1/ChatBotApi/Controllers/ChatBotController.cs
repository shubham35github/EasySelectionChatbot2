using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SelectMonitorsLib;
using DbDataAccessLayerLib;
using ChatBotApiProcessorLib;
using QuestionOptionModelLib;

namespace ChatBotApi.Controllers
{
    public class ChatBotController : ApiController
    {
        /// <summary>
        ///   Get monitors
        /// </summary>
        /// <param name="Feature_No">Used to indicate The previous Question</param>
        /// <param name="FeatureValue">Used to indicate The option selected to the previous question</param>
        /// <remarks>This request yeilds the list of monitors or a single monitor by taking in the last question number and the option selected</remarks>
        /// <remarks></remarks>
        /// <returns>List of Monitors Suiting the Conditions</returns>
        /// 
        [Route("Api/ChatBot/GetMonitors/{Feature_No}/{FeatureValue}")]
        public IHttpActionResult GetMonitors(int Feature_No,string FeatureValue)
        {
            try
            {
                var monitors = new SelectMonitors(new DbDataAccessLayer()).GetAllSelectedItems(Feature_No, FeatureValue);
                if (monitors.Count == 0)
                {
                    return NotFound();
                }
                return Ok(monitors);
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Parameter entered are incorrect");
            }
            
        }
        /// <summary>
        ///   Get  All monitors
        /// </summary>
        /// <remarks>This request yeilds the list of all monitors available</remarks>
        /// <remarks></remarks>
        /// <returns>All Monitors</returns>
        /// 
        [Route("Api/ChatBot/GetMonitors")]
        public IHttpActionResult GetMonitors()
        {
            var monitors = new SelectMonitors(new DbDataAccessLayer()).GetAllSelectedItems(0, "FirstValue");
            if (monitors.Count() == 0)
            {
                return NotFound();
            }
            return Ok(monitors);
        }

        /// <summary>
        ///   Get next Question and Options 
        /// </summary>
        /// <remarks>This request yeilds next qusetion to be asked with options</remarks>
        /// <param name="PreviousQuestion">Used to indicate The previous Question </param>
        /// <param name="OptionSelected">Used to indicate The option selected to the previous question</param>
        /// <returns>A QuestionOPtion model which consists of next question and its Options</returns>
        [Route("Api/ChatBot/GetQuestionAndOption/{PreviousQuestion}/{OptionSelected}")]
        public IHttpActionResult GetQuestionOption(int PreviousQuestion,string OptionSelected)
        {
            ChatBotProcessor processor = new ChatBotProcessor(new DbDataAccessLayer());
            try
            {
                QuestionOptionModel questionOption = processor.Process(PreviousQuestion, OptionSelected);
                if (questionOption == null)
                {
                    return NotFound();
                }
                return Ok(questionOption);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,"Parameter entered are incorrect");
            }
        }
        /// <summary>
        /// Get the first Question and options
        /// </summary>
        /// <remarks>This request yeilds First qusetion to be asked with options</remarks>
        /// <returns>A QuestionOPtion model which consists of first question and its Options</returns>
        [Route("Api/ChatBot/GetQuestionAndOption")]
        public IHttpActionResult GetQuestionOption()
        {
            ChatBotProcessor processor = new ChatBotProcessor(new DbDataAccessLayer());
            try
            {
                QuestionOptionModel questionOption = processor.Process(0,null);
                if (questionOption == null)
                {
                    return NotFound();
                }
                return Ok(questionOption);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
