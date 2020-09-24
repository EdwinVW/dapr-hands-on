using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Simulation.Events;

namespace Simulation
{
    public class CameraSimulation
    {
        private Random _rnd;
        private int _minEntryDelayInMS = 50;
        private int _maxEntryDelayInMS = 5000;
        private int _minExitDelayInS = 4;
        private int _maxExitDelayInS = 8;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public void Start(int camNumber)
        {
            Console.WriteLine($"Start camera {camNumber} simulation.");

            // initialize state
            _rnd = new Random();
            var httpClient = new HttpClient();

            while (true)
            {
                try
                {
                    // simulate entry
                    TimeSpan entryDelay = TimeSpan.FromMilliseconds(_rnd.Next(_minEntryDelayInMS, _maxEntryDelayInMS) + _rnd.NextDouble());
                    Task.Delay(entryDelay).Wait();

                    Task.Run(() =>
                    {
                    // simulate entry
                    DateTime entryTimestamp = DateTime.Now;
                        var @event = new VehicleRegistered
                        {
                            Lane = _rnd.Next(1, 4),
                            LicenseNumber = GenerateRandomLicenseNumber(),
                            Timestamp = entryTimestamp
                        };

                        var @eventJson = new StringContent(JsonSerializer.Serialize(@event, _jsonSerializerOptions), Encoding.UTF8, "application/json");
                        httpClient.PostAsync("http://localhost:5000/trafficcontrol/entrycam", @eventJson).Wait();

                        Console.WriteLine($"Simulated ENTRY of vehicle with license-number {@event.LicenseNumber} in lane {@event.Lane}");

                    // simulate exit
                    TimeSpan exitDelay = TimeSpan.FromSeconds(_rnd.Next(_minExitDelayInS, _maxExitDelayInS) + _rnd.NextDouble());
                        Task.Delay(exitDelay).Wait();
                        @event.Timestamp = DateTime.Now;
                        @event.Lane = _rnd.Next(1, 4);

                        @eventJson = new StringContent(JsonSerializer.Serialize(@event, _jsonSerializerOptions), Encoding.UTF8, "application/json");
                        httpClient.PostAsync("http://localhost:5000/trafficcontrol/exitcam", @eventJson).Wait();

                        Console.WriteLine($"Simulated EXIT of vehicle with license-number {@event.LicenseNumber} in lane {@event.Lane}");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        #region Private helper methods

        private string _validLicenseNumberChars = "DFGHJKLNPRSTXYZ";

        private string GenerateRandomLicenseNumber()
        {
            int type = _rnd.Next(1, 9);
            string kenteken = null;
            switch (type)
            {
                case 1: // 99-AA-99
                    kenteken = string.Format("{0:00}-{1}-{2:00}", _rnd.Next(1, 99), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                    break;
                case 2: // AA-99-AA
                    kenteken = string.Format("{0}-{1:00}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 99), GenerateRandomCharacters(2));
                    break;
                case 3: // AA-AA-99
                    kenteken = string.Format("{0}-{1}-{2:00}", GenerateRandomCharacters(2), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                    break;
                case 4: // 99-AA-AA
                    kenteken = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(2), GenerateRandomCharacters(2));
                    break;
                case 5: // 99-AAA-9
                    kenteken = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                    break;
                case 6: // 9-AAA-99
                    kenteken = string.Format("{0}-{1}-{2:00}", _rnd.Next(1, 9), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                    break;
                case 7: // AA-999-A
                    kenteken = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 999), GenerateRandomCharacters(1));
                    break;
                case 8: // A-999-AA
                    kenteken = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(1), _rnd.Next(1, 999), GenerateRandomCharacters(2));
                    break;
            }

            return kenteken;
        }

        private string GenerateRandomCharacters(int aantal)
        {
            char[] chars = new char[aantal];
            for (int i = 0; i < aantal; i++)
            {
                chars[i] = _validLicenseNumberChars[_rnd.Next(_validLicenseNumberChars.Length - 1)];
            }
            return new string(chars);
        }

        #endregion
    }
}