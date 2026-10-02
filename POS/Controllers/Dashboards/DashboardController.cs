using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.Dashboards;

namespace POS.API.Controllers.Dashboards;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase, IDashboardAppService
{
    private readonly IDashboardAppService _dashboardAppService;

    public DashboardController(IDashboardAppService dashboardAppService)
    {
        _dashboardAppService = dashboardAppService;
    }
    [HttpGet("GetSummary")]
    public Task<DashboardSummaryDto> GetSummaryAsync([FromQuery] GetDashboardInput input, CancellationToken ct)
    {
        return _dashboardAppService.GetSummaryAsync(input, ct);
    }
}
