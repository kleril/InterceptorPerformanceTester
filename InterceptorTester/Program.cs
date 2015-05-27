﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Timers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace ConsoleApplication1{

    class Program
    {
		static string certPath = "../../Data/unittestcert.pfx";
        static string certPass = "unittest";

        // Create a collection object and populate it using the PFX file
        static X509Certificate cert;

        static List<Test> tests;

        static double timeTaken;

        public static void Main()
        {
			Console.WriteLine("Try giving the program some actual tests to run.");
        }

        public Program()
        {
            try
            {
                cert = new X509Certificate(certPath, certPass);
                Console.WriteLine("SLL certificate created successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not initialize SLL certificate");
                Console.WriteLine(e.ToString());
            }
        }

        //Do test, output results to file.
        public async Task<double> runTest(Test currentTest)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Test starting");
            //Do tests
            timer.Start();
            await testType(currentTest);
            timer.Stop();
            double time = timer.Elapsed.TotalMilliseconds;
            Console.WriteLine("Test ending");

            return time;
        }

        //TODOIF: Tweak console output to be a little clearer. Console is made redundant by logs, but it could be useful.
        static async Task testType (Test currentTest)
        {
            KeyValuePair<JObject, string> result;
            //results.WriteLine("Raw test results:");
            switch (currentTest.ToString())
            {
                case "iCmd":
                    result = await RunGetAsync(currentTest.getOperation().getUri());
                    currentTest.setActualResult(result.Key.GetValue("StatusCode").ToString());
                    Console.WriteLine(result.Value + " Is the result of the iCmd test");
                    //results.WriteLine(result.ToString());
                    break;
                case "DeviceScan":
                    result = await RunPostAsync(currentTest.getOperation().getUri(), currentTest.getOperation().getJson());
                    currentTest.setActualResult(result.Key.GetValue("StatusCode").ToString());
                    Console.WriteLine(result.Value + "Is the result of the DeviceScan test");
                    //results.WriteLine(result.ToString());
                    break;
                case "DeviceSetting":
                    result = await RunGetAsync(currentTest.getOperation().getUri());
                    currentTest.setActualResult(result.Key.GetValue("StatusCode").ToString());
                    Console.WriteLine(result.Value + " Is the result of the DeviceSetting test");
                    //results.WriteLine(result.ToString());
                    break;
                case "DeviceBackup":
                    result = await RunPostAsync(currentTest.getOperation().getUri(), currentTest.getOperation().getJson());
                    currentTest.setActualResult(result.Key.GetValue("StatusCode").ToString());
                    Console.WriteLine(result.Value + "Is the result of the DeviceBackup test");
                    //results.WriteLine(result.ToString());
                    break;
                case "DeviceStatus":
                    result = await RunPostAsync(currentTest.getOperation().getUri(), currentTest.getOperation().getJson());
                    currentTest.setActualResult(result.Key.GetValue("StatusCode").ToString());
                    Console.WriteLine(result.Value + "Is the result of the DeviceStatus test");
                    //results.WriteLine(result.ToString());
                    break;
               default:
                    Console.WriteLine("Unrecognized test type!");
                    Console.WriteLine(currentTest.ToString());
                    break;
            }
        }

        //GET call
        static async Task<KeyValuePair<JObject, string>> RunGetAsync(Uri qUri)
        {
            // ... Use HttpClient.
            try
            {

				WebRequestHandlerWithClientcertificates handler = new WebRequestHandlerWithClientcertificates();
                handler.ClientCertificates.Add(cert);
                using (HttpClient client = new HttpClient(handler))
                using (HttpResponseMessage response = await client.GetAsync(qUri.AbsoluteUri))
                {
                    JObject jResponse = JObject.FromObject(response);
                    string content = await response.Content.ReadAsStringAsync();
                    return new KeyValuePair<JObject, string>(jResponse, content);
                }
            }
            catch (Exception e)
            {
				Console.WriteLine("GET request failed: " + e.ToString());
                Console.WriteLine("URL: " + qUri.ToString());
                Console.WriteLine(e);
                return new KeyValuePair<JObject, string>(null, null);
            }
        }

        //POST call
        static async Task<KeyValuePair<JObject, string>> RunPostAsync(Uri qUri, Object contentToPush)
        {
            try
            {
                // ... Use HttpClient.

				WebRequestHandlerWithClientcertificates handler = new WebRequestHandlerWithClientcertificates();
                handler.ClientCertificates.Add(cert);
                using (HttpClient client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Newtonsoft Json serialization
                    Console.WriteLine(contentToPush.ToString());
                    var upContent = JObject.FromObject(contentToPush);
                    Console.WriteLine(upContent.ToString());
                    var strContent = new System.Net.Http.StringContent(upContent.ToString(), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(qUri, strContent))
                    {
                        JObject jResponse = JObject.FromObject(response);
                        string content = await response.Content.ReadAsStringAsync();
                        return new KeyValuePair<JObject, string>(jResponse, content);
                    }
                }
                return new KeyValuePair<JObject, string>(null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("POST request failed.");
                Console.WriteLine("URL: " + qUri.ToString());
                Console.WriteLine("Content: " + contentToPush.ToString());
                Console.WriteLine(e);
                return new KeyValuePair<JObject, string>(null, null);
            }
        }

        //PUT call
        static async Task<KeyValuePair<JObject, string>> RunPutAsync(Uri qUri, HttpContent contentToPut)
        {
            try
            {
                // ... Use HttpClient.
				WebRequestHandlerWithClientcertificates handler = new WebRequestHandlerWithClientcertificates();
                handler.ClientCertificates.Add(cert);
                using (HttpClient client = new HttpClient(handler))
                using (HttpResponseMessage response = await client.PutAsync(qUri, contentToPut))
                {
                    JObject jResponse = JObject.FromObject(response);
                    string content = await response.Content.ReadAsStringAsync();
                    return new KeyValuePair<JObject, string>(jResponse, content);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("PUT request failed.");
                Console.WriteLine("URL: " + qUri.ToString());
                Console.WriteLine("Content: " + contentToPut.ToString());
                Console.WriteLine(e);
                return new KeyValuePair<JObject, string>(null, null);
            }
        }

        //DELETE call
        static async Task<KeyValuePair<JObject, string>> RunDeleteAsync(Uri qUri)
        {
            try
            {
                // ... Use HttpClient.
				WebRequestHandlerWithClientcertificates handler = new WebRequestHandlerWithClientcertificates();
                handler.ClientCertificates.Add(cert);
                using (HttpClient client = new HttpClient(handler))
                using (HttpResponseMessage response = await client.DeleteAsync(qUri))
                {
                    JObject jResponse = JObject.FromObject(response);
                    string content = await response.Content.ReadAsStringAsync();
                    return new KeyValuePair<JObject, string>(jResponse, content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("DELETE request failed.");
                Console.WriteLine("URL: " + qUri.ToString());
                Console.WriteLine(e);
                return new KeyValuePair<JObject, string>(null,null);
            }
        }
    }
}
