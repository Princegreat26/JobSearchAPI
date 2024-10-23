// See https://aka.ms/new-console-template for more information
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JobSearchApi
{ 
    public class JobProgram
    {

        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;
            string apiEndpoint = "https://jsearch.p.rapidapi.com/search?";
            string apiKey = "your-own-api-key";

            Console.WriteLine("Enter the desired job title: ");
            string jobTitle = Console.ReadLine();
            
            if (string.IsNullOrEmpty(jobTitle))
            {
                jobTitle = "Node.js developer in New-York,USA";
            }
            var client = new HttpClient();

            try
            {
                string query = Uri.EscapeDataString(jobTitle);

                string requestUri = $"{apiEndpoint}query={query}&page=1&num_pages=1&date_posted=all";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                request.Headers.Add("x-rapidapi-key", apiKey);

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();

                // var jobs = JsonConvert.DeserializeObject<List<JobSearchResult>>(jsonString);

                var jobSearchResult = JsonConvert.DeserializeObject<JobSearchResult>(jsonString);

                var jobs = jobSearchResult.Data;

                Console.WriteLine("{0} {1} {2} {3} {4}", "Title", "Employer Name", "Country", "Employment Type", "Salary");

                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------");

                foreach (var job in jobs)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4}", job.JobTitle ?? "N/A",  job.JobEmployerName ?? "N/A", job.JobCountry ?? "N/A", job.JobEmploymentType ?? "N/A", job.JobSalary ?? "N/A");
                }

                //Console.WriteLine(jsonOutput);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching data: " + ex.Message);
            } 
        }



        public class JobSearchResult
        {
            public List<Job> Data { get; set; }
        }

        public class Job 
        {
            [JsonProperty("job_id")]
            public string JobId { get; set; }

            [JsonProperty("job_title")]
            public string JobTitle { get; set; }

            [JsonProperty("job_employer_name")]
            public string JobEmployerName { get; set; }

            [JsonProperty("job_country")]
            public string JobCountry { get; set; }

            [JsonProperty("job_employment_type")]
            public string JobEmploymentType { get; set; }

            [JsonProperty("job_max_salary")]
            public string JobSalary { get; set; }
        }
    }
}

/*`
 * if (response.IsSuccessful)
            {
                var jobs = response.Data;

                Console.WriteLine("{0,-10} {1,-30} {2,-20} {3,-20} {4,-15} {5,-10}", "Id", "Title", "Company/Employer", "Location", "Employment Type", "Salary");
                Console.WriteLine("--------------------------------------------------------------------------");
                foreach (var job in jobs)
                {
                    Console.WriteLine("{0,-10} {1,-30} {2,-20} {3,-20} {4,-15} {5,-10}", job.Id, job.Title, job.EmployerName, job.Country, job.EmploymentType, job.Salary);
                }
            }
            else
*/