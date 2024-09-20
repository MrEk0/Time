using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public static class WebRequestHelper
{
    public static async Task<string> HttpGetAsync(string url)
    {
        return await HttpGetAsync(url, new Dictionary<HttpRequestHeader, string>());
    }

    private static async Task<string> HttpGetAsync(string url, Dictionary<HttpRequestHeader, string> headers)
    {
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;

            foreach (var pair in headers.ToList())
            {
                switch (pair.Key)
                {
                    case HttpRequestHeader.ContentType:
                    case HttpRequestHeader.Accept:
                    case HttpRequestHeader.Referer:
                    case HttpRequestHeader.UserAgent:
                        request.Accept = pair.Value;
                        headers.Remove(pair.Key);
                        break;
                }
            }

            foreach (var pair in headers)
                request.Headers.Add(pair.Key, pair.Value);

            using var response = (HttpWebResponse)await request.GetResponseAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await using var stream = response.GetResponseStream();
                if (stream == null)
                    return string.Empty;

                var read = new StreamReader(stream);
                return await read.ReadToEndAsync();
            }

            Debug.Log($"{url}: {response.StatusCode}, {response.StatusDescription}");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"{url}: {ex}");
        }

        return string.Empty;
    }
}
