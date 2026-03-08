
using System.Net.WebSockets;
using Audit.Core;
using MultiTenantOrderService.Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenSearch.Client;

namespace  MultiTenantOrderService.Infrastructure.Utilities
{
    
}

public class AwsOpenSearchDataProvider : AuditDataProvider
{
    private readonly Uri _nodeAddress;
    private readonly OpenSearchClient _openSearchClient;
    private readonly IConfiguration _configuration;

    public AwsOpenSearchDataProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        var nodeAddress = new Uri(_configuration["AwsOpenSearch:Endpoint"] ?? string.Empty);
        var connectionSettings1 = new ConnectionSettings(nodeAddress).DefaultIndex(_configuration["AwsOpenSearch:Index"]);
        connectionSettings1.BasicAuthentication(_configuration["AwsOpenSearch:Username"], _configuration["AwsOpenSearch:Password"]);
        _openSearchClient = new OpenSearchClient(connectionSettings1);
        this._nodeAddress = new Uri(_configuration["AwsOpenSearch:Endpoint"] ?? string.Empty);
    }

    public override object InsertEvent(AuditEvent auditEvent)
    {
        var auditEventParser = JsonConvert.DeserializeObject<AuditEventParser>(auditEvent.ToJson());
        var response = _openSearchClient.Index(auditEventParser, i => i.Index(_configuration["AwsOpenSearch:Index"]));
        return response.Id;
    }

    public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
    {
        var auditEventParser = JsonConvert.DeserializeObject<AuditEventParser>(auditEvent.ToJson());
        _openSearchClient.Update<AuditEventParser>(eventId.ToString(), u => u
            .Index(_configuration["AwsOpenSearch:Index"])
            .Doc(auditEventParser));
    }

    public override async Task<object> InsertEventAsync(AuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        var auditEventParser = JsonConvert.DeserializeObject<AuditEventParser>(auditEvent.ToJson());
        var response = await _openSearchClient.IndexAsync(auditEventParser, i => i.Index(_configuration["AwsOpenSearch:Index"]), cancellationToken);
        return response.Id;
    }

    public override async Task ReplaceEventAsync(object eventId, AuditEvent auditEvent, CancellationToken cancellationToken = default)
    {
        var auditEventParser = JsonConvert.DeserializeObject<AuditEventParser>(auditEvent.ToJson());
        await _openSearchClient.UpdateAsync<AuditEventParser>(eventId.ToString(), u => u
            .Index(_configuration["AwsOpenSearch:Index"])
            .Doc(auditEventParser), cancellationToken);
    }
}

public class A
{
    public string Test1 { get; set; }
    public B B { get; set; }
}

public class B
{
    public string Tesst2 { get; set; }
}