
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/grades")]
public class GradesController : ControllerBase
{
    private readonly IGradesInterface gradesService;

    public GradesController(IGradesInterface gradesService)
    {
        this.gradesService = gradesService;
    }

    [HttpPost("grade-student")]
    public IActionResult GradeStudent([FromBody] GradesRequest gradesRequest)
    {
        var res = gradesService.GradeStudent(gradesRequest);

        if (res.Success == false)
        {
            return BadRequest(res);
        }

        return Ok(res);
    }

    [HttpGet("fetch-grades")]
    public IActionResult FetchStudentGrades()
    {
        return Ok("Endpoint hit");
    }

}
