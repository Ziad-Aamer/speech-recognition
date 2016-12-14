using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Runtime;

namespace StartingWithSpeechRecognition
{
    class Program
    {
        static SpeechRecognitionEngine _recognizer = null;
        static ManualResetEvent manualResetEvent = null;
        string talks = "";
        static string Name = "islam";
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name :");
            Name = Console.ReadLine();
            while (true)
            {
                manualResetEvent = new ManualResetEvent(false);
                Console.WriteLine("speech to make sure the computer speaks to you");
                
                RecognizeSpeechAndMakeSureTheComputerSpeaksToYou();

                manualResetEvent.WaitOne();

                if (_recognizer != null)
                {
                    _recognizer.Dispose();
                }
            }
        }
        #region Recognize speech and write to console
        static void RecognizeSpeechAndWriteToConsole()
        {
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("test"))); // load a "test" grammar
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("exit"))); // load a "exit" grammar
            _recognizer.SpeechRecognized += _recognizeSpeechAndWriteToConsole_SpeechRecognized; // if speech is recognized, call the specified method
            _recognizer.SpeechRecognitionRejected += _recognizeSpeechAndWriteToConsole_SpeechRecognitionRejected; // if recognized speech is rejected, call the specified method
            _recognizer.SetInputToDefaultAudioDevice(); // set the input to the default audio device
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous

        }
        static void _recognizeSpeechAndWriteToConsole_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "test")
            {
                Console.WriteLine("test");
            }
            else if (e.Result.Text == "exit")
            {
                manualResetEvent.Set();
            }
        }
        static void _recognizeSpeechAndWriteToConsole_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Speech rejected. Did you mean:");
            foreach (RecognizedPhrase r in e.Result.Alternates)
            {
                Console.WriteLine("    " + r.Text);
            }
        }
        #endregion

        #region Recognize speech and make sure the computer speaks to you (text to speech)
        static void RecognizeSpeechAndMakeSureTheComputerSpeaksToYou()
        {
            _recognizer = new SpeechRecognitionEngine();

            Choices talks = new Choices("what is your name","hello computer", "how are you", "where are you from", "fuck you", "yabn el weskha", "ahha");
                          


             GrammarBuilder gb = new GrammarBuilder();

            gb.Append(talks);

          //  gb.Culture = new System.Globalization.CultureInfo("ar-EG");

            _recognizer.LoadGrammar(new Grammar(gb)); // load a "hello computer" grammar
            _recognizer.SpeechRecognized += _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognized; // if speech is recognized, call the specified method
            _recognizer.SpeechRecognitionRejected += _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognitionRejected;
            _recognizer.SetInputToDefaultAudioDevice(); // set the input to the default audio device
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous
        }

        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            talks = e.Result.Text;
        }
        static void _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(e.Result.Text)));

            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

            if (e.Result.Text == "hello computer")
            {
                speechSynthesizer.Speak("hello "+ Name);
                speechSynthesizer.Dispose();
            }
            else if(e.Result.Text == "what is your name")
            {
                speechSynthesizer.Speak("my name is computer ebdel salam computer");
                speechSynthesizer.Dispose();

            }
            else if(e.Result.Text == "how are you")
            {
                speechSynthesizer.Speak("fine thank you and you");
                speechSynthesizer.Dispose();
            }
            else if (e.Result.Text == "where are you from")
            {
                speechSynthesizer.Speak("i am from egypt");
                speechSynthesizer.Dispose();
            }
            else if(e.Result.Text == "fuck you")
            {
                speechSynthesizer.Speak("kos omak ya "+ Name);
                speechSynthesizer.Dispose();
            }
            else if(e.Result.Text == "yabn el weskha")
            {
                speechSynthesizer.Speak("yabn elweskha ya " + Name);
                speechSynthesizer.Dispose();
            }
            else
            {
                speechSynthesizer.Speak("ahha ya " + Name);
                speechSynthesizer.Dispose();
            }

            manualResetEvent.Set();
        }
        static void _recognizeSpeechAndMakeSureTheComputerSpeaksToYou_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (e.Result.Alternates.Count == 0)
            {
                Console.WriteLine("No candidate phrases found.");
                return;
            }
            Console.WriteLine("Speech rejected. Did you mean:");
            foreach (RecognizedPhrase r in e.Result.Alternates)
            {
                if(!r.Text.Equals("fuck you") && !r.Text.Equals("ahha") && !r.Text.Equals("yabn el weskha"))
                    Console.WriteLine("    " + r.Text);
            }
        }
        #endregion
    }
}
