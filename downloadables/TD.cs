using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

// You may ask why I use "var" rather than the type of the variable... I know it's debatable, but it makes the code easier to read and refactorable.
// P.S. Hover over "var" to see the type if you are in Visual Studio.
public class TD
{
    private HttpClient client = new HttpClient();
    private string baseURI;
    private HttpResponseMessage response = new HttpResponseMessage();

	public TD(string baseURI)
	{
        this.baseURI = baseURI;
    }

    /// <summary>
    /// Logs into TeamDynamix and stores the Bearer toekn for subsequent requests.
    /// </summary>
    /// <param name="username">The username to authenticate with.</param>
    /// <param name="password">The passwird to authenticate with.</param>
    /// <returns>A Task because it is async; for all intents and purposes, it can be considered void.</returns>
    public async Task LoginAsync(string username, string password)
    {
        string json = "{'username': '" + username + "', 'password': '" + password + "'}";
        client.BaseAddress = new Uri(baseURI);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        response = await client.PostAsync("auth", new StringContent(json, Encoding.UTF8, "application/json"));
        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("---FAILURE---");
            Console.WriteLine("StatusCode=" + response.StatusCode);
            Console.WriteLine("ReasonPhrase=" + response.ReasonPhrase);
            Console.WriteLine("Response content = " + sResponse);
            return;
        }
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + sResponse);
        Console.WriteLine("Successfully logged in");
    }
    /// <summary>
    /// Logs into TeamDynamix and stores the Bearer token for subsequent requests.
    /// </summary>
    /// <param name="username">The username to authenticate with.</param>
    /// <param name="password">The passwird to authenticate with.</param>
    /// <returns>Boolean, whether it was succesfully logged in</returns>
    public bool LoginSync(string username, string password)
    {
        string json = "{'username': '" + username + "', 'password': '" + password + "'}";
        client.BaseAddress = new Uri(baseURI);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        response = client.PostAsync("auth", new StringContent(json, Encoding.UTF8, "application/json")).Result;
        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("---FAILURE---");
            Console.WriteLine("StatusCode=" + response.StatusCode);
            Console.WriteLine("ReasonPhrase=" + response.ReasonPhrase);
            Console.WriteLine("Response content = " + sResponse);
            return false;
        }
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + sResponse);
        Console.WriteLine("Successfully logged in");
        return true;
    }

    /// <summary>
    /// Gets a ticket.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="id">The id of the ticket to get.</param>
    /// <returns>A Task containing the ticket.</returns>
    public async Task<TeamDynamix.Api.Tickets.Ticket> GetTicketAsync(int appId, int id)
    {
        response = await client.GetAsync(appId + "/tickets/" + id); // GET ticket
        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }
        var ticket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return ticket;
    }

    /// <summary>
    /// Gets a ticket.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="id">The id of the ticket to get.</param>
    /// <returns>The Ticket.</returns>
    public TeamDynamix.Api.Tickets.Ticket GetTicketSync(int appId, int id)
    {
        response = client.GetAsync(appId + "/tickets/" + id).Result; // GET ticket
        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }
        var ticket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return ticket;
    }

    /// <summary>
    /// Updates a ticket by using the Post method.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="ticket">The Ticket to Post.</param>
    /// <param name="notify">Whether to notify the person responsible.</param>
    /// <returns>A Task containing the updated Ticket.</returns>
    public async Task<TeamDynamix.Api.Tickets.Ticket> PostTicketAsync(int appId, TeamDynamix.Api.Tickets.Ticket ticket, bool notify = false)
    {
        // convert to JSON
        var json = JsonConvert.SerializeObject(ticket);
        // convert to HttpContent, but more specifically StringContent
        var sTicket = new StringContent(json, Encoding.UTF8, "application/json");
        // make the request
        response = await client.PostAsync(appId + "/tickets/" + ticket.ID + "?notifyNewResponsible=" + notify, sTicket);

        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }

        var tTicket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return tTicket;
    }

    /// <summary>
    /// Updates a ticket by using the Post method.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="ticket">The Ticket to Post.</param>
    /// <param name="notify">Whether to notify the person responsible.</param>
    /// <returns>The updated Ticket.</returns>
    public TeamDynamix.Api.Tickets.Ticket PostTicketSync(int appId, TeamDynamix.Api.Tickets.Ticket ticket, bool notify = false)
    {
        // convert to JSON
        var json = JsonConvert.SerializeObject(ticket);
        // convert to HttpContent, but more specifically StringContent
        var sTicket = new StringContent(json, Encoding.UTF8, "application/json");
        // make the request
        response = client.PostAsync(appId + "/tickets/" + ticket.ID + "?notifyNewResponsible=" + notify, sTicket).Result;

        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }

        var tTicket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return tTicket;
    }

    /// <summary>
    /// Updates a ticket by using the Patch method.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="id">The id of the ticket to Patch.</param>
    /// <param name="content">The content to patch in the ticket. See https://api.teamdynamix.com/TDWebApi/Home/AboutPatching for more details.</param>
    /// <param name="notify">Whether to notify the person responsible.</param>
    /// <returns>A Task containing the updated Ticket.</returns>
    public async Task<TeamDynamix.Api.Tickets.Ticket> PatchTicketAsync(int appId, int id, TeamDynamix.Api.JsonPatchOperation[] content, bool notify = false)
    {
        // declare the method
        var method = new HttpMethod("PATCH");
        // convert to JSON
        var json = JsonConvert.SerializeObject(content);
        // format it into an HttpContent
        var iContent = new StringContent(json, Encoding.UTF8, "application/json");
        // mix it all together
        var message = new HttpRequestMessage(method, baseURI + appId + "/tickets/" + id + "?notifyNewResponsible=" + notify) { Content = iContent };
        // send it off
        response = await client.SendAsync(message);

        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }

        TeamDynamix.Api.Tickets.Ticket tTicket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return tTicket;
    }

    /// <summary>
    /// Updates a ticket by using the Patch method.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="id">The id of the ticket to Patch.</param>
    /// <param name="content">The content to patch in the ticket. See https://api.teamdynamix.com/TDWebApi/Home/AboutPatching for more details.</param>
    /// <param name="notify">Whether to notify the person responsible.</param>
    /// <returns>The updated Ticket.</returns>
    public TeamDynamix.Api.Tickets.Ticket PatchTicketSync(int appId, int id, TeamDynamix.Api.JsonPatchOperation[] content, bool notify = false)
    {
        // declare the method
        var method = new HttpMethod("PATCH");
        // convert to JSON
        var json = JsonConvert.SerializeObject(content);
        // format it into an HttpContent
        var iContent = new StringContent(json, Encoding.UTF8, "application/json");
        // mix it all together
        var message = new HttpRequestMessage(method, baseURI + appId + "/tickets/" + id + "?notifyNewResponsible=" + notify) { Content = iContent };
        // send it off
        response = client.SendAsync(message).Result;

        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }

        TeamDynamix.Api.Tickets.Ticket tTicket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return tTicket;
    }

    /// <summary>
    /// Gets all the Feed Entries from the ticket.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="id">The id of the ticket to get the Feed from.</param>
    /// <returns></returns>
    public async Task<TeamDynamix.Api.Feed.ItemUpdate[]> GetFeedEntriesAsync(int appId, int id)
    {
        response = await client.GetAsync(appId + "/tickets/" + id + "/feed"); // GET ticket
        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }
        TeamDynamix.Api.Feed.ItemUpdate[] items = JsonConvert.DeserializeObject<TeamDynamix.Api.Feed.ItemUpdate[]>(sResponse); // convert response to ticket
        return items;
    }

    /// <summary>
    /// Adds a feed item to the ticket
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="id">The id of the ticket.</param>
    /// <param name="entry">The Feed Entry that will be used to upadte the ticket.</param>
    /// <returns>The Feed Item that was added.</returns>
    public async Task<TeamDynamix.Api.Feed.ItemUpdate> CreateNewFeedEntryAsync(int appId, int id, TeamDynamix.Api.Feed.TicketFeedEntry entry)
    {
        // convert to JSON
        var json = JsonConvert.SerializeObject(entry);
        // convert to HttpContent, but more specifically StringContent
        var sTicket = new StringContent(json, Encoding.UTF8, "application/json");
        // make the request
        response = await client.PostAsync(appId + "/tickets/" + id + "/feed", sTicket);

        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }

        var update = JsonConvert.DeserializeObject<TeamDynamix.Api.Feed.ItemUpdate>(sResponse); // convert response to ticket
        return update;
    }

    /// <summary>
    /// Creates a ticket.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="ticket">The ticket to create in TD.</param>
    /// <param name="options">OPTIONAL: Any options that should be included with the ticket.</param>
    /// <returns>A Task containing the ticket that was created</returns>
    public async Task<TeamDynamix.Api.Tickets.Ticket> CreateTicketAsync(int appId, TeamDynamix.Api.Tickets.Ticket ticket, TeamDynamix.Api.Tickets.TicketCreateOptions options = null)
    {
        var newOptions = new TeamDynamix.Api.Tickets.TicketCreateOptions
        {
            EnableNotifyReviewer = false,
            NotifyRequestor = false,
            NotifyResponsible = false,
            AllowRequestorCreation = false
        };
        options ??= newOptions; // a way to get around complie time constant arguments

        // convert to JSON
        var json = JsonConvert.SerializeObject(ticket);
        // convert to HttpContent, but more specifically StringContent
        var sTicket = new StringContent(json, Encoding.UTF8, "application/json");
        // make the request
        response = await client.PostAsync(appId + "/tickets/?EnableNotifyReviewer=" + options.EnableNotifyReviewer + "&NotifyRequestor=" + options.NotifyRequestor + "&NotifyResponsible=" + options.NotifyResponsible + "&AllowRequestorCreation=" + options.AllowRequestorCreation, sTicket);

        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }

        var tTicket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return tTicket;
    }

    /// <summary>
    /// Creates a ticket.
    /// </summary>
    /// <param name="appId">The Application ID that the ticket is in.</param>
    /// <param name="ticket">The ticket to create in TD.</param>
    /// <param name="options">OPTIONAL: Any options that should be included with the ticket.</param>
    /// <returns>The created Ticket</returns>
    public TeamDynamix.Api.Tickets.Ticket CreateTicketSync(int appId, TeamDynamix.Api.Tickets.Ticket ticket, TeamDynamix.Api.Tickets.TicketCreateOptions options = null)
    {
        var newOptions = new TeamDynamix.Api.Tickets.TicketCreateOptions
        {
            EnableNotifyReviewer = false,
            NotifyRequestor = false,
            NotifyResponsible = false,
            AllowRequestorCreation = false
        };
        options ??= newOptions; // a way to get around complie time constant arguments

        // convert to JSON
        var json = JsonConvert.SerializeObject(ticket);
        // convert to HttpContent, but more specifically StringContent
        var sTicket = new StringContent(json, Encoding.UTF8, "application/json");
        // make the request
        response = client.PostAsync(appId + "/tickets/?EnableNotifyReviewer=" + options.EnableNotifyReviewer + "&NotifyRequestor=" + options.NotifyRequestor + "&NotifyResponsible=" + options.NotifyResponsible + "&AllowRequestorCreation=" + options.AllowRequestorCreation, sTicket).Result;

        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }

        var tTicket = JsonConvert.DeserializeObject<TeamDynamix.Api.Tickets.Ticket>(sResponse); // convert response to ticket
        return tTicket;
    }

    /// <summary>
    /// Checks for the rate limit headers of the last request. If there was a request with no rate limit, it returns negative data.
    /// </summary>
    /// <returns>A Tuple that has the rate limit details.</returns>
    public (int limit, int remaining, DateTime reset) GetRateLimitDetails()
    {
        int limit = -1;
        if (response.Headers.Contains("X-RateLimit-Limit"))
        {
            string rateLimit = response.Headers.GetValues("X-RateLimit-Limit").FirstOrDefault();
            if (!int.TryParse(rateLimit, out limit))
                Console.WriteLine("Could not convert to Int");
        }

        int remaining = -1;
        if (response.Headers.Contains("X-RateLimit-Remaining"))
        {
            string rateLimitRemaining = response.Headers.GetValues("X-RateLimit-Remaining").FirstOrDefault();
            if (!int.TryParse(rateLimitRemaining, out remaining))
                Console.WriteLine("Could not convert to Int");
        }

        DateTime reset = DateTime.Now;
        if (response.Headers.Contains("X-RateLimit-Reset"))
        {
            string rateLimitReset = response.Headers.GetValues("X-RateLimit-Reset").FirstOrDefault();
            if (!DateTime.TryParse(rateLimitReset, out reset))
                Console.WriteLine("Could not convert to Int");
        }

        return (limit, remaining, reset);
    }

    /// <summary>
    /// Gets a report with optional data
    /// </summary>
    /// <param name="id">The id of the report to get</param>
    /// <param name="withData">Whether you the data in the rerport, true or false</param>
    /// <param name="dataSortExpression">Only applicable if data is retrieved; a sorting expression used to sort the data</param>
    /// <returns>The Report</returns>
    public TeamDynamix.Api.Reporting.Report GetReportSync(int id, bool withData = false, string dataSortExpression = "")
    {
        response = client.GetAsync("reports/" + id + "?withData=" + withData + "&dataSortExpression=" + dataSortExpression).Result; // GET report
        string sResponse = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("ERROR: " + response.ReasonPhrase);
            return null;
        }
        var report = JsonConvert.DeserializeObject<TeamDynamix.Api.Reporting.Report>(sResponse); // convert response to a report
        return report;
    }
}
