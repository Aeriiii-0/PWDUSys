using Common_Class;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer
{
    public class Query
        {
            private static readonly HttpClient _httpClient = new HttpClient();

            // --- IMPORTANT: Replace with the actual HTTPS URLs of your deployed functions ---
            // You get these URLs from the Firebase Console -> Functions -> Dashboard tab after deployment.
            private const string AnalyticsSummaryFunctionUrl = "https://getanalyticssummary-6i5svmwu3q-uc.a.run.app";
            private const string HelloWorldFunctionUrl = "https://helloworld-6i5svmwu3q-uc.a.run.app";
            private const string FilteredRecordsFunctionUrl = "https://getfilteredrecords-6i5svmwu3q-uc.a.run.app";

            //static async Task Main(string[] args)
            //{
            //    Console.WriteLine("Firebase Cloud Function Tester (HTTP Triggered)");

            //    Console.WriteLine("\n--- Testing helloWorld Function ---");
            //    await TestHelloWorld();

            //    Console.WriteLine("\n--- Testing getAnalyticsSummary Function ---");
            //    await TestGetAnalyticsSummary();

            //    Console.WriteLine("\n--- Testing getFilteredRecords Function (by Barangay) ---");
            //    Console.WriteLine("Enter Barangay name to search (e.g., Bagong Silang): ");
            //    string barangayToSearch = Console.ReadLine().Trim();
            //    await TestGetFilteredRecords(barangayToSearch);

            //    //Console.WriteLine("\n--- Testing getFilteredRecords Function (with no filters, just limit) ---");
            //    //await TestGetFilteredRecords(null); // Pass null to get all (with limit)

            //    Console.WriteLine("\nPress any key to exit.");
            //    Console.ReadKey();
            //}

            public static async Task TestHelloWorld()
            {
                try
                {
                    Console.WriteLine("Calling helloWorld...");
                    var response = await _httpClient.GetAsync(HelloWorldFunctionUrl); // GET request
                    response.EnsureSuccessStatusCode(); // Throws an exception for HTTP error codes
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"  Response: {content}");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"  HTTP Request Error (helloWorld): {ex.Message}");
                    if (ex.InnerException != null) Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Unexpected Error (helloWorld): {ex.Message}");
                }
            }

            public static async Task TestGetAnalyticsSummary()
            {
                try
                {
                    Console.WriteLine("Calling getAnalyticsSummary...");
                    // Make a POST request with an empty body (or any data if your function expects it)
                    var response = await _httpClient.PostAsync(AnalyticsSummaryFunctionUrl, new StringContent("{}", Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode(); // Throws an exception for HTTP error codes

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var resultData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);

                    if (resultData != null)
                    {
                        // Use JObject for safer parsing of dynamic JSON
                        var jObjectResult = Newtonsoft.Json.Linq.JObject.FromObject(resultData);

                        long totalCases = jObjectResult["totalCases"]?.ToObject<long>() ?? 0L;
                        long activeCases = jObjectResult["activeCases"]?.ToObject<long>() ?? 0L;
                        long pendingCases = jObjectResult["pendingCases"]?.ToObject<long>() ?? 0L;
                        long casesThisMonth = jObjectResult["casesThisMonth"]?.ToObject<long>() ?? 0L;

                        Console.WriteLine("Analytics Summary Received:");
                        Console.WriteLine($"  Total Cases: {totalCases}");
                        Console.WriteLine($"  Active Cases: {activeCases}");
                        Console.WriteLine($"  Pending Cases: {pendingCases}");
                        Console.WriteLine($"  Cases This Month: {casesThisMonth}");

                        if (jObjectResult.TryGetValue("casesByBarangay", out var barangayCountsToken) && barangayCountsToken != null)
                        {
                            var casesByBarangay = barangayCountsToken.ToObject<Dictionary<string, long>>();
                            Console.WriteLine("  Cases By Barangay:");
                            foreach (var entry in casesByBarangay)
                            {
                                Console.WriteLine($"    {entry.Key}: {entry.Value}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No data returned from getAnalyticsSummary.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"  HTTP Request Error (getAnalyticsSummary): {ex.Message}");
                    if (ex.InnerException != null) Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"  JSON Deserialization Error (getAnalyticsSummary): {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Unexpected Error (getAnalyticsSummary): {ex.Message}");
                }
            }


            public static async Task<List<Entry>> TestGetFilteredRecords(string barangay = null)
            {
                try
                {
                    Console.WriteLine($"Calling getFilteredRecords for Barangay: {barangay ?? "All (with limit)"}...");

                    var functionData = new Dictionary<string, object>
                {
                    { "limit", 5 } // Request 5 records per page for testing
                };

                    if (!string.IsNullOrEmpty(barangay))
                    {
                        functionData["barangay"] = barangay;
                    }

                    string jsonContent = JsonConvert.SerializeObject(functionData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync(FilteredRecordsFunctionUrl, content);
                    response.EnsureSuccessStatusCode(); // Throws an exception for HTTP error codes

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var resultData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);

                    if (resultData != null)
                    {
                        var EntriesRaw = resultData["records"] as Newtonsoft.Json.Linq.JArray;
                        List<Entry> Entries= EntriesRaw?.ToObject<List<Entry>>() ?? new List<Entry>();

                        string lastDocId = resultData.TryGetValue("lastDocId", out object ldId) ? ldId?.ToString() ?? "N/A" : "N/A";
                        bool hasMore = resultData.TryGetValue("hasMore", out object hm) ? (hm is bool ? (bool)hm : false) : false;

                        //string lastDocId = resultData.ContainsKey("lastDocId") ? resultData["lastDocId"].ToString() : "N/A";
                        //bool hasMore = resultData.ContainsKey("hasMore") ? (bool)resultData["hasMore"] : false;

                        Console.WriteLine($"  Received {Entries.Count} records.");
                        Console.WriteLine($"  Last Document ID: {lastDocId}, Has More Pages: {hasMore}");

                        if (Entries.Any())
                        {
                            //Console.WriteLine("  First few records details:");
                            //foreach (var Entry in Entries.Take(3))
                            //{
                            //    Console.WriteLine($"    ID: {Entry.caseId}, Name: {Entry.lastName}, Barangay: {Entry.barangay}, Status: {Entry.status}");
                            //}

                        return Entries;
                    }
                        else
                        {
                            Console.WriteLine("  No records found for the given criteria.");
                        return new List<Entry>();

                    }
                }
                    else
                    {
                    
                        Console.WriteLine("  No data returned from getFilteredRecords.");
                    return new List<Entry>();

                }
            }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"  HTTP Request Error (getFilteredRecords): {ex.Message}");
                    if (ex.InnerException != null) Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
                return new List<Entry>();

            }
            catch (JsonException ex)
                {
                    Console.WriteLine($"  JSON Deserialization Error (getFilteredRecords): {ex.Message}");
                return new List<Entry>();

            }
            catch (Exception ex)
                {
                    Console.WriteLine($"  Unexpected Error (getFilteredRecords): {ex.Message}");
                return new List<Entry>();

            }
        }
        }
    }


