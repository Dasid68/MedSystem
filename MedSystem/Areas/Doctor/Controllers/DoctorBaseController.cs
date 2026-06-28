using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSystem.Areas.Doctor.Controllers;
[Area("Doctor")]
[Authorize(Roles = "Doctor")]
[Route("doctor")]
public abstract class DoctorBaseController : Controller
{
}