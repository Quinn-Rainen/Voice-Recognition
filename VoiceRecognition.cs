// Weather App
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Speech.Recognition;


namespace SpeechRecognitionApp
{
  class Program
  {
    static void Main(string[] args)
    {

        // API Key for open weather map
        string API_KEY = "8b22faa095bd024044764e367f2f9062";
        string LOCATION = "Eugene,Oregon";
        // API Endpoint
        string API_ENDPOINT = "https://api.openweathermap.org/data/2.5/weather?q=" + LOCATION + "&appid=" + API_KEY;


        // Create an API request using HttpWebRequest and open weather
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_ENDPOINT);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string responseText = reader.ReadToEnd();


        // Parse the JSON response
        JObject weatherData = JObject.Parse(responseText);
    
        double temperature = (double)weatherData["main"]["temp"];
        double temperatureFahrenheit = temperature * 9.0 / 5.0 - 459.67;
        //get and convert temperature

        // Create a SpeechRecognitionEngine object for the default recognizer in the en-US locale.
        using (
        SpeechRecognitionEngine recognizer =
            new SpeechRecognitionEngine(
            new System.Globalization.CultureInfo("en-US")))
        {


            // Create a grammar for finding weather in Eugene.
            Choices weather = new Choices(new string[] { "Show me the weather"});
            Choices cities = new Choices(new string[] {"Eugene"})

            GrammarBuilder findweather = new GrammarBuilder("Find");
            findWeather.Append(weather);
            findWeather.Append("near");
            findWeather.Append(cities);


            // Create a Grammar object from the GrammarBuilder and load it to the recognizer.
            Grammar weatherGrammar = new Grammar(findWeather);
            recognizer.LoadGrammarAsync(weatherGrammar);


            // Add a handler for the speech recognized event.
            recognizer.SpeechRecognized +=
            new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);


            // Configure the input to the speech recognizer.
            recognizer.SetInputToDefaultAudioDevice();


            // Start asynchronous, continuous speech recognition.
            recognizer.RecognizeAsync(RecognizeMode.Multiple);


            // Keep the console window open.
            while (true)
            {
            Console.ReadLine();
            }
        }
        }


        // Handle the SpeechRecognized event.
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);
            Console.WriteLine("The current temperature in " + LOCATION + " is " + temperatureFahrenheit + " degrees Fahrenheit.");
        }
    }
    }


        
