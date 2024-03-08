using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {

    

        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "Act as a random stranger in a chat room and reply to the questions. Don't break character. Don't ever mention that you are an AI model.";

        private void Start()
        {
        }


        private async void SendReply(string sending, string apply_message)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = sending
            };

            if (messages.Count == 0) newMessage.Content = prompt + "\n"; 
            messages.Add(newMessage);
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                messages.Add(message);

                apply_message = message.Content;
                Debug.Log("apply_message : " + apply_message);

               
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

        }
    }
}
