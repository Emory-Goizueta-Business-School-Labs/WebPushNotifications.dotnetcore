using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebPush;
using WebPushNotifications.Data;
using WebPushNotifications.Models;

namespace WebPushNotifications.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WebPushNotificationsContext _db;
    private readonly VapidOptions _vapid;

    public HomeController(
        ILogger<HomeController> logger,
        WebPushNotificationsContext context,
        IOptions<VapidOptions> options)
    {
        _logger = logger;
        _db = context;
        _vapid = options.Value;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterSubscription([FromBody]Subscription sub)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sub.Endpoint))
            {
                return BadRequest(new RegisterSubscriptionError("Endpoint is required."));
            }

            _db.Subscriptions.Add(sub);
            await _db.SaveChangesAsync();

            return new JsonResult(new RegisterSubscriptionSuccess(sub));
        }
        catch(Exception e)
        {
            return new JsonResult(new RegisterSubscriptionError(e.Message));
        }
    }

    public IActionResult SendNotification()
    {
        return View(new Notification());
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification(Notification notification)
    {
        List<Subscription> subscriptions = await _db.Subscriptions.Include(s => s.Keys).ToListAsync();

        WebPushClient client = new();
        VapidDetails vapidDetails = new(_vapid.Subject, _vapid.Public, _vapid.Private);
                
        //await Task.WhenAll(subscriptions.Select(s => client.SendNotificationAsync(s.ToWebPushSubscription(), notification.GetPayload(), vapidDetails)));

        foreach (Subscription s in subscriptions)
        {
            try
            {
                await client.SendNotificationAsync(s.ToWebPushSubscription(), notification.GetPayload(), vapidDetails);
            }
            catch(WebPushException e)
            {
                _logger.LogError(e.Message);

                if (e.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.Gone)
                {
                    _db.Subscriptions.Remove(s);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(message: e.Message);
            }
                
        }

        return RedirectToAction("Index");
    }
}

