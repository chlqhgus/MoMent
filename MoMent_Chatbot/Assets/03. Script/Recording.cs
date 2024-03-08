
namespace OpenAI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using System.Text;
    using System.IO;
    using System;
    using TMPro;

    public class Recording : MonoBehaviour
    {

        const int BlockSize_16Bit = 2;
        private string _microphoneID = null; //입력받을 microphone
        private AudioClip _recording = null;
        private int _recordingLengthSec = 15; //음성인식의 최대 길이
        private int _recordingHZ = 22050;
        public string usermessage = null;
        public string[] replymessage = null;


        public TextMeshProUGUI send_message;
        public TextMeshProUGUI reply_message;


        // 사용할 언어(Kor)를 맨 뒤에 붙임
        string url = "https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang=Kor";

        private void Start()
        {
            _microphoneID = Microphone.devices[0];
            replymessage = new string[1];
        }


// ********************************************* Recording *************************************************//
// *********************************************************************************************************//

        // 버튼을 OnPointerDown 할 때 호출
        public void startRecording()
        {
            Debug.Log("start recording");
            _recording = Microphone.Start(_microphoneID, false, _recordingLengthSec, _recordingHZ);
        }

        // 버튼을 OnPointerUp 할 때 호출
        public void stopRecording()
        {
            if (Microphone.IsRecording(_microphoneID))
            {
                Microphone.End(_microphoneID);

                Debug.Log("stop recording");
                if (_recording == null)
                {
                    Debug.LogError("nothing recorded");
                    return;
                }
                // audio clip to byte array
                byte[] byteData = getByteFromAudioClip(_recording);

                // 녹음된 audioclip api 서버로 보냄
                StartCoroutine(PostVoice(url, byteData));
            }
            return;
        }

        private byte[] getByteFromAudioClip(AudioClip audioClip)
        {
            MemoryStream stream = new MemoryStream();
            const int headerSize = 44;
            ushort bitDepth = 16;

            int fileSize = audioClip.samples * BlockSize_16Bit + headerSize;

            // audio clip의 정보들을 file stream에 추가(링크 참고 함수 선언)
            WriteFileHeader(ref stream, fileSize);
            WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
            WriteFileData(ref stream, audioClip, bitDepth);
            
            // stream을 array형태로 바꿈
            byte[] bytes = stream.ToArray();

            return bytes;
        }

            private static int WriteFileHeader (ref MemoryStream stream, int fileSize)
        {
            int count = 0;
            int total = 12;

            // riff chunk id
            byte[] riff = Encoding.ASCII.GetBytes ("RIFF");
            count += WriteBytesToMemoryStream (ref stream, riff, "ID");

            // riff chunk size
            int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (chunkSize), "CHUNK_SIZE");

            byte[] wave = Encoding.ASCII.GetBytes ("WAVE");
            count += WriteBytesToMemoryStream (ref stream, wave, "FORMAT");

            // Validate header
            Debug.AssertFormat (count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);

            return count;
        }

        private static int WriteFileFormat (ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
        {
            int count = 0;
            int total = 24;

            byte[] id = Encoding.ASCII.GetBytes ("fmt ");
            count += WriteBytesToMemoryStream (ref stream, id, "FMT_ID");

            int subchunk1Size = 16; // 24 - 8
            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk1Size), "SUBCHUNK_SIZE");

            UInt16 audioFormat = 1;
            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (audioFormat), "AUDIO_FORMAT");

            UInt16 numChannels = Convert.ToUInt16 (channels);
            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (numChannels), "CHANNELS");

            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (sampleRate), "SAMPLE_RATE");

            int byteRate = sampleRate * channels * BytesPerSample (bitDepth);
            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (byteRate), "BYTE_RATE");

            UInt16 blockAlign = Convert.ToUInt16 (channels * BytesPerSample (bitDepth));
            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (blockAlign), "BLOCK_ALIGN");

            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (bitDepth), "BITS_PER_SAMPLE");

            // Validate format
            Debug.AssertFormat (count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);

            return count;
        }

        private static int WriteFileData (ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
        {
            int count = 0;
            int total = 8;

            // Copy float[] data from AudioClip
            float[] data = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData (data, 0);

            byte[] bytes = ConvertAudioClipDataToInt16ByteArray (data);

            byte[] id = Encoding.ASCII.GetBytes ("data");
            count += WriteBytesToMemoryStream (ref stream, id, "DATA_ID");

            int subchunk2Size = Convert.ToInt32 (audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)
            count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk2Size), "SAMPLES");

            // Validate header
            Debug.AssertFormat (count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);

            // Write bytes to stream
            count += WriteBytesToMemoryStream (ref stream, bytes, "DATA");

            // Validate audio data
            Debug.AssertFormat (bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

            return count;
        }
            private static int BytesPerSample (UInt16 bitDepth)
        {
            return bitDepth / 8;
        }

        private static int WriteBytesToMemoryStream (ref MemoryStream stream, byte[] bytes, string tag = "")
        {
            int count = bytes.Length;
            stream.Write (bytes, 0, count);
            //Debug.LogFormat ("WAV:{0} wrote {1} bytes.", tag, count);
            return count;
        }

            private static byte[] ConvertAudioClipDataToInt16ByteArray (float[] data)
        {
            MemoryStream dataStream = new MemoryStream ();

            int x = sizeof(Int16);

            Int16 maxValue = Int16.MaxValue;

            int i = 0;
            while (i < data.Length) {
                dataStream.Write (BitConverter.GetBytes (Convert.ToInt16 (data [i] * maxValue)), 0, x);
                ++i;
            }
            byte[] bytes = dataStream.ToArray ();

            // Validate converted bytes
            Debug.AssertFormat (data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);

            dataStream.Dispose ();

            return bytes;
        }

        // 받아온 값에 간편하게 접근하기 위한 JSON 선언
    [Serializable]
    public class VoiceRecognize
    {
        public string text;
    }


    private IEnumerator PostVoice(string url, byte[] data)
    {
        // request 생성
        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        
        // 요청 헤더 설정
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "w8ssq5sjjl");
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", "TGWCEHMrew7SxYrrvt1YFI0nON1b3557nVDslhiz");
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        
        // 바디에 처리과정을 거친 Audio Clip data를 실어줌
        request.uploadHandler = new UploadHandlerRaw(data);
        
        // 요청을 보낸 후 response를 받을 때까지 대기
        yield return request.SendWebRequest();
        
        // 만약 response가 비어있다면 error
        if (request == null)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // json 형태로 받음 {"text":"인식결과"}
            string message = request.downloadHandler.text;
            VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(message);

            Debug.Log("Voice Server responded: " + voiceRecognize.text);
            // Voice Server responded: 인식결과

            if (voiceRecognize.text != null)
            {
                send_message.text = voiceRecognize.text;
                SendReply(voiceRecognize.text,replymessage);
            }
        }


    }

// ********************************************* GPT *************************************************//
// *********************************************************************************************************//

            private OpenAIApi openai = new OpenAIApi();

            private List<ChatMessage> messages = new List<ChatMessage>();
            private string prompt = "Act as this Character. {직업 : 강아지. 성격 : 온화함, 친절함, 다정함." +
            "상황 : 너는 이미 하늘나라에 있어. 오랜만에 주인이랑 다시 만난 상태야. 주인이랑 보고싶었다는 대화 중."+
            "특징 : 산책을 좋아함 } 15단어 이내로 말해. 주어진 정보를 나열하지는 마. 오랜만에 만나서 좋다고, 많이 보고싶었다는 말 위주로 해."
            + " 주어지지 않은 정보를 막 지어내지는 마. 한국어로 대답해. Don't break character. Don't ever mention that you are an AI model.";


            private async void SendReply(string sending, string[] apply_message)
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
                    Model = "ft:gpt-3.5-turbo-0613:personal::8Oi9PEeg",
                    Messages = messages
                });

                if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
                {
                    var message = completionResponse.Choices[0].Message;
                    message.Content = message.Content.Trim();
                    messages.Add(message);

                    apply_message[0] = message.Content;
                    Debug.Log("apply_message : " + apply_message[0]);
                    reply_message.text = apply_message[0];
                
                }
                else
                {
                    Debug.LogWarning("No text was generated from this prompt.");
                }

            }
        }

}



